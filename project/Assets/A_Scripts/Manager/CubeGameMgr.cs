using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EazyGF
{
    public class CubeLaoutOff
    {
        public float offx;
        public float offy;
        public CubeLaoutOff(float offx, float offy)
        {
            this.offx = offx;
            this.offy = offy;
        }
    }

    [System.Serializable]
    public class CubeSerData
    {
        public int level;
    }

    public class CubeGameMgr
    {
        string[] clearAudio = new string[] {
            "c_cube_clear_1",
            "c_cube_clear_2",
            "c_cube_clear_3",
            "c_cube_clear_4",
            "c_cube_clear_5",
            "c_cube_clear_6",
            "c_cube_clear_7",
            "c_cube_clear_8" };
        public static CubeGameMgr cubeGameMgr;

        public CubeItem selectedCube;

        public Transform cubeGridTf;
        public Transform selectFrame1;
        public Transform selectFrame2;

        Transform leftFlyTf;
        Transform rightFlyTf;
        Transform flyTfEffect;

        private Dictionary<int, List<Cube_Property>> levelDatas = new Dictionary<int, List<Cube_Property>>();

        List<Cube_Property> cubeLayoutData = new List<Cube_Property>();

        private Dictionary<int, List<CubeItem>> mCubeLayoutDic = new Dictionary<int, List<CubeItem>>();

        int cubeWidth = 78;
        int cubeHight = 78;
        int colLenght = 8;
        int rowLenght = 11;

        bool isDebug = false;
        public int curLevel = 1;

        public static CubeGameMgr Instance
        {
            get
            {
                if (cubeGameMgr == null)
                {
                    cubeGameMgr = new CubeGameMgr();
                    cubeGameMgr.Init();
                }

                return cubeGameMgr;
            }
        }

        public string GetRandomClearEff()
        {
            return clearAudio[UnityEngine.Random.Range(0, clearAudio.Length)];
        }
        public int GetCurLevelTimer()
        {
            return AppConst_Data.DataArray.CubeGameTotalTime;
        }

        public bool isPause = true;

        private void Init()
        {
            if (!ReadData())
            {
                curLevel = 1;
                SaveCubeData();
            }
        }

        private string savaDataName = "Cube.data";
        private string cubeGameSavePath;
        public string CubeGameSavePath
        {
            get
            {
                if (string.IsNullOrEmpty(cubeGameSavePath))
                {
#if !UNITY_EDITOR
                cubeGameSavePath = Path.Combine(Application.persistentDataPath, savaDataName);
#else
                    cubeGameSavePath = Path.Combine(Directory.GetCurrentDirectory(), $"OtherAssets/PlayerData/{savaDataName}");
#endif
                }
                return cubeGameSavePath;
            }
        }


        private void SaveCubeData()
        {
            CubeSerData cubeSerData = new CubeSerData();
            cubeSerData.level = curLevel;
            SerializHelp.SerializeFile(CubeGameSavePath, cubeSerData);
        }

        public bool ReadData()
        {
            CubeSerData cubeSerData = SerializHelp.DeserializeFileToObj<CubeSerData>(CubeGameSavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                curLevel = cubeSerData.level;
            }

            return loadSuccess;
        }

        public bool IsUnlock(int index, int layout)
        {
            //if (unLockNeedIndexs == null || unLockNeedIndexs.Count == 0)
            //{
            //    return true;
            //}

            if (layout == cubeLayoutData.Count - 1)
            {
                return true;
            }
            else
            {
                //CubeLaoutOff curOff = new CubeLaoutOff(cubeLayoutData[layout].PosX, cubeLayoutData[layout].PosY);

                for (int i = layout + 1; i < cubeLayoutData.Count; i++)
                {
                    if (mCubeLayoutDic.ContainsKey(i))
                    {
                        List<CubeItem> cubeItems = mCubeLayoutDic[i];

                        //List<int> unLockNeedIndexs = CubeGameMgr.Instance.GetLinkCubeIndexs(index, cubeLayoutData[i].PosX - curOff.offx, cubeLayoutData[i].PosX - curOff.offy);

                        List<int> unLockNeedIndexs = GetOffData(index, layout, i, 1);

                        for (int j = 0; j < cubeItems.Count; j++)
                        {
                            if (unLockNeedIndexs.Contains(cubeItems[j].PosIndex))
                            {
                                return false;
                            }
                        }
                    }

                }
            }

            return true;
        }

        public List<int> GetOffData(int index, int curLaout, int targetLayout, int type = 2)
        {
            CubeLaoutOff curOff = new CubeLaoutOff(cubeLayoutData[curLaout].PosX, cubeLayoutData[curLaout].PosY);
            CubeLaoutOff targetOff = new CubeLaoutOff(cubeLayoutData[targetLayout].PosX, cubeLayoutData[targetLayout].PosY);

            return CubeGameMgr.Instance.GetLinkCubeIndexs(index, targetOff.offx - curOff.offx, targetOff.offy - curOff.offy, curLaout, type);
        }

        public List<int> GetLinkCubeIndexs(int index, float offx, float offy, int layout = 0, int type = 0)
        {
            List<int> datas = new List<int>();

            int col = index % colLenght;
            int row = index / colLenght;

            int interval = 0;

            float aOffx = Mathf.Abs(offx);
            float aOffy = Mathf.Abs(offy);

            if ((offx == 0 && offy == 0) || (aOffx <= interval && aOffy <= interval))
            {
                datas.Add(index);

                return datas;
            }
            else if (offx != 0 && offy == 0)
            {
                if (aOffx <= interval)
                {
                    datas.Add(index);

                    return datas;
                }

                int index1 = col - (int)(offx / cubeWidth);

                int index2 = index1 + 1;
                if (offx > 0)
                {
                    index2 = index - 1;
                }

                if (index1 >= 0 && index1 <= colLenght - 1)
                {
                    datas.Add(index1 + row * colLenght);
                }

                if (index2 >= 0 && index2 <= colLenght - 1 && (int)offx % cubeWidth != 0)
                {
                    datas.Add(index2 + row * colLenght);
                }

            }
            else if (offx == 0 && offy != 0)
            {
                if (aOffy <= interval)
                {
                    datas.Add(index);

                    return datas;
                }

                int index1 = row + (int)(offy / cubeHight);

                int index2 = index1 + 1;
                if (offy < 0)
                {
                    index2 = index1 - 1;
                }

                if (index1 >= 0 && index1 <= rowLenght - 1)
                {
                    datas.Add(col + index1 * colLenght);
                }
                if (index2 >= 0 && index2 <= rowLenght - 1 && (int)offy % cubeHight != 0)
                {
                    datas.Add(col + index2 * colLenght);
                }
            }
            else if (offx != 0 && offy != 0)
            {
                int mCol = col;
                int mCow = row;

                mCol -= (int)(offx / cubeWidth);
                mCow += (int)(offy / cubeHight);

                int mCol1 = mCol;
                int mCow1 = mCow;

                if (mCol1 >= 0 && mCol1 <= colLenght - 1 && mCow1 >= 0 && mCow1 <= rowLenght - 1)
                {
                    datas.Add(mCol1 + mCow1 * colLenght);
                }

                if (Mathf.Abs(cubeWidth) % offx >= interval)
                {
                    int mCol2 = mCol + 1;
                    int mCow2 = mCow;
                    if (offx > 0)
                    {
                        mCol2 = mCol - 1;
                    }

                    if (mCol2 >= 0 && mCol2 <= colLenght - 1 && mCow2 >= 0 && mCow2 <= rowLenght - 1 && (int)offx % cubeWidth != 0)
                    {
                        datas.Add(mCol2 + mCow2 * colLenght);
                    }
                }


                if (Mathf.Abs(cubeHight) % offy >= interval)
                {
                    int mCol3 = mCol;
                    int mCow3 = mCow + 1;
                    if (offy < 0)
                    {
                        mCow3 = mCow - 1;
                    }

                    if (mCol3 >= 0 && mCol3 <= colLenght - 1 && mCow3 >= 0 && mCow3 <= rowLenght - 1 && (int)offy % cubeWidth != 0)
                    {
                        datas.Add(mCol3 + mCow3 * colLenght);
                    }
                }

                if (Mathf.Abs(cubeWidth) % offx >= interval && Mathf.Abs(cubeHight) % offy >= interval)
                {
                    int mCol4 = mCol + 1;
                    int mCow4 = mCow + 1;

                    if (offx > 0)
                    {
                        mCol4 = mCol - 1;
                    }

                    if (offy < 0)
                    {
                        mCow4 = mCow - 1;
                    }

                    if (mCol4 >= 0 && mCol4 <= colLenght - 1 && mCow4 >= 0 && mCow4 <= rowLenght - 1 && (int)offx % cubeWidth != 0 && (int)offy % cubeWidth != 0)
                    {
                        datas.Add(mCol4 + mCow4 * colLenght);
                    }
                }

            }

            if (isDebug)
            {
                string log = $"needUnLockCube  {index}   {offx}  {offy}   {layout}   {type} \n";
                for (int i = 0; i < datas.Count; i++)
                {
                    log += datas[i] + "   ";
                }

                Debug.LogError(log);
            }

            return datas;
        }

        public int startNum = 3;

        public bool IsVictory()
        {
            bool isVictory = true;

            foreach (var item in mCubeLayoutDic)
            {
                if (item.Value.Count > 0)
                {
                    isVictory = false;
                    break;
                }
            }

            if (isVictory)
            {
                isPause = true;
                UIMgr.ShowPanel<VictoryPanel>(new VictoryPanelData(startNum, curLevel));
                AddCubeLevel();
            }

            return false;
        }

        public void AddCubeLevel()
        {
            curLevel++;

            int max = Cube_Data.DataArray[Cube_Data.DataArray.Length - 1].ID;

            if (curLevel > max)
            {
                curLevel = 1;
            }

            SaveCubeData();
        }
        public void SetCubeLevel(int level)
        {
            curLevel = level;

            int max = Cube_Data.DataArray[Cube_Data.DataArray.Length - 1].ID;

            if (curLevel > max)
            {
                curLevel = 1;
            }

            SaveCubeData();
        }


        public void InitComponent(Transform cubeGridTf, Transform selectFrame1, Transform selectFrame2, Transform leftFlyTf,
            Transform rightFlyTf, Transform flyTfEffect)
        {
            this.cubeGridTf = cubeGridTf;
            this.selectFrame1 = selectFrame1;
            this.selectFrame2 = selectFrame2;
            this.leftFlyTf = leftFlyTf;
            this.rightFlyTf = rightFlyTf;
            this.flyTfEffect = flyTfEffect;
        }

        public void AddCubeItem(CubeItem cubeItem)
        {
            int layout = cubeItem.MLayout;
            if (mCubeLayoutDic.ContainsKey(layout))
            {
                if (!mCubeLayoutDic[layout].Contains(cubeItem))
                {
                    mCubeLayoutDic[layout].Add(cubeItem);
                }
            }
            else
            {
                mCubeLayoutDic.Add(layout, new List<CubeItem> { cubeItem });
            }
        }

        public void AddCubeItem(int layout, List<CubeItem> cubeItems)
        {
            mCubeLayoutDic.Add(layout, cubeItems);
        }


        public void RemoveCubeItem(CubeItem cubeItem)
        {
            int layout = cubeItem.MLayout;
            if (mCubeLayoutDic.ContainsKey(layout))
            {
                if (mCubeLayoutDic[layout].Contains(cubeItem))
                {
                    if (isDebug)
                    {
                        Debug.LogError("item: +  " + cubeItem.name);
                    }
                    mCubeLayoutDic[layout].Remove(cubeItem);
                }
            }
        }

        public List<Cube_Property> GetCurLevelData()
        {
            Cube_Property[] tabls = Cube_Data.DataArray;

            cubeLayoutData.Clear();
            mCubeLayoutDic.Clear();

            for (int i = 0; i < tabls.Length; i++)
            {
                if (tabls[i].ID > curLevel)
                {
                    break;
                }

                if (tabls[i].ID == curLevel)
                {
                    cubeLayoutData.Add(tabls[i]);
                }
            }

            return cubeLayoutData;
        }


        public int[][] GetCubeItemIds()
        {
            int[][] cIds2 = new int[cubeLayoutData.Count][];

            List<int> sLineIds = new List<int>();

            for (int i = 0; i < cubeLayoutData.Count; i++)
            {
                Cube_Property data = cubeLayoutData[i];
                int length = data.IndexPos.Length;

                if (length % 2 != 0)
                {
                    length--;
                    sLineIds.Add(i);
                }

                int index = 0;
                int[] ids = CubeConfig_DataBase.GetPropertyByID(data.CubeLibId).BoxIds;
                ids = SortArray(ids);

                int type = data.Type;
                int typeNum = data.TypeNum;
                int[] boxIds = new int[type];
                for (int k = 0; k < type; k++)
                {
                    boxIds[k] = ids[k];
                }

                int[] rIds = new int[data.IndexPos.Length];

                for (int j = 0; j < length; j++)
                {
                    if (index == boxIds.Length * 2)
                    {
                        index = 0;
                    }

                    int id = 0;
                    if (j < boxIds.Length * 2)
                    {
                        id = boxIds[j / 2];
                    }
                    else
                    {
                        id = boxIds[index / (2 * typeNum)];
                    }

                    index++;

                    rIds[j] = id;
                }
                cIds2[i] = rIds;
            }

            int rId = cIds2[0][0];

            for (int i = 0; i < sLineIds.Count; i++)
            {
                int layout = sLineIds[i];
                int[] a = cIds2[layout];

                cIds2[layout][a.Length - 1] = rId;
            }

            return cIds2;
        }

        public void SetSelectedCube(CubeItem cubeItem)
        {

            if (cubeItem == selectedCube)
            {
                return;
            }

            SetSelectFram(selectFrame2, cubeGridTf, false);

            if (selectedCube == null)
            {
                selectedCube = cubeItem;

                SetSelectFram(selectFrame1, selectedCube.sFPosTf);
            }
            else if (selectedCube.Data.ID == cubeItem.Data.ID)
            {
                //float disX = Mathf.Abs(selectedCube.transform.position.x - cubeItem.transform.position.x);

                //float disY = Mathf.Abs(selectedCube.transform.position.y - cubeItem.transform.position.y);

                if (!isUseItem)
                {
                    MusicMgr.Instance.PlayMusicEff(cubeItem.Data.SoundName);
                }

                Vector3 centerPos = (selectFrame1.transform.position + cubeItem.transform.position) / 2;

                SetSelectFram(selectFrame1, cubeGridTf, false);

                List<Vector3> pos1 = new List<Vector3>();
                List<Vector3> pos2 = new List<Vector3>();

                Vector3 leftOff = Vector3.left / 5;
                Vector3 rightOff = -leftOff;
                Vector3 up = Vector3.up * 5;

                if (selectedCube.transform.position.x < cubeItem.transform.position.x)
                {
                    //pos1.Add(centerPos + leftOff);
                    //pos1.Add(pos1[0] + up);

                    //pos2.Add(centerPos + rightOff);
                    //pos2.Add(pos2[0] + up);

                    pos1.Add(leftFlyTf.position);
                    pos2.Add(rightFlyTf.position);
                }
                else
                {
                    //pos1.Add(centerPos + rightOff);
                    //pos1.Add(pos1[0] + up);

                    //pos2.Add(centerPos + leftOff);
                    //pos2.Add(pos2[0] + up);

                    pos1.Add(rightFlyTf.position);
                    pos2.Add(leftFlyTf.position);
                }

                FlyCubeItem(selectedCube, pos1);
                FlyCubeItem(cubeItem, pos2, true);

                //ReadyFlyCubeItem(selectedCube, pos1);
                //ReadyFlyCubeItem(cubeItem, pos2);

                selectedCube = null;

                EventManager.Instance.TriggerEvent(EventKey.SuccClearCEvent, null);
            }
            else if (selectedCube.Data.ID != cubeItem.Data.ID)
            {
                selectedCube = cubeItem;

                SetSelectFram(selectFrame1, selectedCube.sFPosTf);
            }
        }

        public void SetSelectFram(Transform fram, Transform parent, bool isSelect = true)
        {
            if (isSelect)
            {
                fram.gameObject.SetActive(isSelect);
                fram.SetParent(parent);
            }
            else
            {
                fram.gameObject.SetActive(isSelect);
                fram.SetParent(parent);
            }

            fram.localPosition = Vector3.zero;
        }

        Queue<CubeItem> rCIs = new Queue<CubeItem>();
        Queue<List<Vector3>> rCIPos = new Queue<List<Vector3>>();

        public void ClearRFlayCI()
        {
            rCIs.Clear();
            rCIPos.Clear();
        }

        private void ReadyFlyCubeItem(CubeItem cubeItem, List<Vector3> pos)
        {
            rCIs.Enqueue(cubeItem);
            rCIPos.Enqueue(pos);

            EventManager.Instance.TriggerEvent(EventKey.UpdateCubeEvent, cubeItem);

            QueueFly();
        }


        bool isFlyCI = true;
        private void QueueFly()
        {
            if (rCIs.Count >= 2 && isFlyCI)
            {
                //UIMgr.ShowPanel<MaskPanel>();
                //isFlyCI = false;
                for (int i = 0; i < 2; i++)
                {
                    FlyCubeItem(rCIs.Dequeue(), rCIPos.Dequeue(), i == 1);
                }
            }
        }

        private void FlyCubeItem(CubeItem cubeItem, List<Vector3> pos, bool isHandle = false)
        {
            cubeItem.transform.SetParent(cubeGridTf);

            EventManager.Instance.TriggerEvent(EventKey.UpdateCubeEvent, cubeItem);

            Vector3[] path = new Vector3[pos.Count];

            for (int i = 0; i < pos.Count; i++)
            {
                path[i] = pos[i];
            }

            var tweenPath = cubeItem.transform.DOPath(path, 0.5f, PathType.CatmullRom);

            tweenPath.onComplete = () =>
            {
                PoolMgr.Instance.DespawnOne(cubeItem.transform);

                if (isHandle)
                {
                    flyTfEffect.gameObject.SetActive(false);
                    flyTfEffect.gameObject.SetActive(true);

                    //UIMgr.HideUI<MaskPanel>();

                    //isFlyCI = true;

                    QueueFly();
                }
            };

            tweenPath.SetEase(Ease.OutQuart);
        }

        public int[] SortArray(int[] rangArr)
        {
            for (int i = 0; i < rangArr.Length; i++)
            {
                int r = UnityEngine.Random.Range(0, rangArr.Length);

                if (i != r)
                {
                    int v = rangArr[i];
                    rangArr[i] = rangArr[r];
                    rangArr[r] = v;
                }
            }

            return rangArr;
        }

        public static bool IsRectTransformOverlap(RectTransform rect1, RectTransform rect2)
        {
            Vector3[] corners1 = new Vector3[4];
            rect1.GetWorldCorners(corners1);
            corners1[2].x = Mathf.Abs(corners1[2].x - corners1[0].x);
            corners1[2].y = Mathf.Abs(corners1[2].y - corners1[0].y);
            Rect b1 = new Rect(corners1[0].x, corners1[0].y, corners1[2].x, corners1[2].y);

            rect2.GetWorldCorners(corners1);
            corners1[2].x = Mathf.Abs(corners1[2].x - corners1[0].x);
            corners1[2].y = Mathf.Abs(corners1[2].y - corners1[0].y);
            Rect b2 = new Rect(corners1[0].x, corners1[0].y, corners1[2].x, corners1[2].y);
            return b1.Overlaps(b2);
        }

        public List<CubeItem> GetUpLayoutCIData()
        {
            List<CubeItem> data = new List<CubeItem>();

            for (int i = cubeLayoutData.Count - 1; i >= 0; i--)
            {
                if (mCubeLayoutDic.ContainsKey(i) && mCubeLayoutDic[i].Count > 0)
                {
                    data.AddRange(mCubeLayoutDic[i]);

                    //data = mCubeLayoutDic[i];
                    //break;
                }
            }

            //data.Sort((a, b) =>
            //{
            //    if (a.MLayout != b.MLayout)
            //    {
            //        return b.MLayout.CompareTo(a.MLayout);
            //    }
            //    else if (a.isUnLock != b.isUnLock)
            //    {
            //        return a.isUnLock.CompareTo(b.isUnLock);
            //    }

            //    return a.Data.ID.CompareTo(b.Data.ID);
            //});


            return data;
        }

        public List<CubeItem_Property> GetUpLayoutCISortData()
        {
            List<CubeItem> data = new List<CubeItem>();

            for (int i = cubeLayoutData.Count - 1; i >= 0; i--)
            {
                if (mCubeLayoutDic.ContainsKey(i) && mCubeLayoutDic[i].Count > 0)
                {
                    data.AddRange(mCubeLayoutDic[i]);

                    //data = mCubeLayoutDic[i];
                    //break;
                }
            }

            List<CubeItem_Property> cData = new List<CubeItem_Property>();


            for (int i = 0; i < data.Count; i++)
            {
                cData.Add(data[i].Data);
            }

            //int rang = UnityEngine.Random.Range(1, 10);

            cData.Sort((a, b) =>
            {
                return a.ID.CompareTo(b.ID);
            });

            return cData;
        }

        public List<CubeItem> GetRangeUpCIData(int pairNum = 1)
        {
            List<CubeItem> data = new List<CubeItem>();
            Dictionary<int, List<CubeItem>> dataDic = new Dictionary<int, List<CubeItem>>();

            for (int i = cubeLayoutData.Count - 1; i >= 0; i--)
            {
                if (mCubeLayoutDic.ContainsKey(i) && mCubeLayoutDic[i].Count > 0)
                {
                    for (int j = 0; j < mCubeLayoutDic[i].Count; j++)
                    {

                        CubeItem cubeItem = mCubeLayoutDic[i][j];

                        if (cubeItem.isUnLock)
                        {
                            int key = cubeItem.Data.ID;
                            if (dataDic.ContainsKey(key))
                            {
                                dataDic[key].Add(cubeItem);
                            }
                            else
                            {
                                dataDic.Add(key, new List<CubeItem>() { cubeItem });
                            }
                        }
                    }
                }
            }

            if (pairNum != -1)
            {
                foreach (var item in dataDic)
                {
                    if (item.Value.Count >= 2)
                    {
                        data.Add(item.Value[0]);
                        data.Add(item.Value[1]);
                    }

                    if (data.Count == pairNum * 2)
                    {
                        return data;
                    }
                }
            }

            return data;
        }

        bool isUseItem = false;
        public bool UserItem(int id)
        {
            Item_Property ItemP = Item_DataBase.GetPropertyByID(id);

            switch (ItemP.UserType)
            {
                case 10:

                    List<CubeItem> data = GetRangeUpCIData(1);

                    if (data.Count > 0 && data[0].Data.ID == data[1].Data.ID)
                    {
                        selectedCube = null;
                        SetSelectedCube(data[0]);

                        SetSelectFram(selectFrame2, data[1].sFPosTf, true);

                        selectedCube = null;

                        return true;
                    }

                    break;

                case 11:

                    List<CubeItem> data2 = GetRangeUpCIData(2);
                    if (data2.Count >= 2)
                    {
                        isUseItem = true;
                        for (int i = 0; i < data2.Count / 2; i++)
                        {
                            selectedCube = null;

                            SetSelectedCube(data2[i * 2]);

                            SetSelectedCube(data2[i * 2 + 1]);
                        }
                        isUseItem = false;
                        return true;
                    }

                    break;

                case 12:

                    List<CubeItem> data3 = GetUpLayoutCIData();

                    //List<CubeItem_Property> dataSort = GetUpLayoutCISortData();

                    //for (int i = 0; i < data3.Count; i++)
                    //{
                    //    data3[i].Data = dataSort[i];
                    //    data3[i].UpdateData();
                    //}

                    for (int i = 0; i < data3.Count; i++)
                    {
                        int index = UnityEngine.Random.Range(i, data3.Count);

                        if (i != index)
                        {
                            CubeItem_Property cubeItem_Property = data3[i].Data;
                            data3[i].Data = data3[index].Data;
                            data3[index].Data = cubeItem_Property;

                            data3[i].UpdateData();
                            data3[index].UpdateData();
                        }
                    }

                    UIMgr.ShowPanel<MaskPanel>();
                    cubeGridTf.GetComponent<CubeGrids>().FlyGridLayout(() =>
                    {

                        UIMgr.HideUI<MaskPanel>();

                    });

                    return true;

                //break;
                default:
                    break;
            }

            return false;
        }

        public void ClearSelected()
        {
            SetSelectFram(selectFrame1, cubeGridTf, false);
            SetSelectFram(selectFrame2, cubeGridTf, false);
        }
    }
}
