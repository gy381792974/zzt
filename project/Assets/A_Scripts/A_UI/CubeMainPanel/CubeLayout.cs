using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class CubeLayout : MonoBehaviour
    {
        public List<Transform> posTfs;

        Cube_Property cubeData;

        public Transform grid;
        [ContextMenu("InitComponent")]
        private void InitComponent()
        {
            posTfs.Clear();

            for (int i = 0; i < grid.childCount; i++)
            {
                Transform tf = grid.GetChild(i);
                tf.gameObject.name = "cube" + i.ToString();
                posTfs.Add(grid.GetChild(i));
            }
        }

        private void Start()
        {
            
        }

        List<CubeItem> cubeItems = new List<CubeItem>();

        public void BindData(Cube_Property cubeData, int layout, int[] cubeIds)
        {
            int[] indexPos = cubeData.IndexPos;

            indexPos = CubeGameMgr.Instance.SortArray(indexPos);

            cubeItems.Clear();

            CubeLaoutOff mLastData = null;
            //if (lastData != null)
            //{
            //    mLastData = new CubeLaoutOff(lastData.offx - cubeData.PosX, lastData.offy - cubeData.PosY);
            //}

            CubeLaoutOff mNextData = null;
            //if (nextData != null)
            //{
            //    mNextData = new CubeLaoutOff(nextData.offx - cubeData.PosX, nextData.offy - cubeData.PosY);
            //}

            //int index = 0;

            //int[] ids = CubeConfig_DataBase.GetPropertyByID(cubeData.CubeLibId).BoxIds;
            //ids = CubeGameMgr.Instance.SortArray(ids);
            
            //int type = cubeData.Type;
            //int typeNum = cubeData.TypeNum;

            //int[] boxIds = new int[type];
            //for (int i = 0; i < type; i++)
            //{
            //    boxIds[i] = ids[i];
            //}

            for (int i = 0; i < indexPos.Length; i++)
            {
                CubeItem cubeItem = AssetMgr.Instance.LoadGameobjFromPool("CubeGameItem").GetComponent<CubeItem>();

                cubeItem.transform.SetParent(posTfs[indexPos[i]]);
                cubeItem.transform.localScale = Vector3.one;
                cubeItem.transform.localPosition = Vector3.one;

                cubeItem.PosIndex = indexPos[i];
                cubeItem.MLayout = layout;

                //if (index == boxIds.Length * 2)
                //{
                //    index = 0;
                //}

                //int id = 0;
                //if (i < boxIds.Length * 2)
                //{
                //    id = boxIds[i / 2];
                //}
                //else
                //{
                //    id = boxIds[index / (2 * typeNum)];
                //}

                //index++;

                //if (true)
                //{
                //    Debug.LogError($"id: {id}");
                //}
                
                cubeItem.BindData(CubeItem_DataBase.GetPropertyByID(cubeIds[i]), mLastData, mNextData);

                cubeItems.Add(cubeItem);
            }

            CubeGameMgr.Instance.AddCubeItem(layout, cubeItems);
        }

        public void SetCubeLayoutLockInfo()
        {
            for (int i = 0; i < cubeItems.Count; i++)
            {
                cubeItems[i].SetLockInfo();
            }
        }

        public void RemoveItem(CubeItem cubeItem)
        {
            if (cubeItems.Contains(cubeItem))
            {
                cubeItems.Remove(cubeItem);
                
                //CubeGameMgr.Instance.RemoveCubeItem(cubeItem);
            }
        }

        bool isDebug = false;

        public void RemoveAllItem()
        {
            for (int i = cubeItems.Count - 1; i >= 0; i--)
            {
                if (isDebug)
                {
                    Debug.LogError($"RemoveAllItem  : {i}");
                }
                PoolMgr.Instance.DespawnOne(cubeItems[i].transform);

                RemoveItem(cubeItems[i]);
            }
        }

        public void UnLockItem(List<int> canLockIndexs)
        {
            for (int i = 0; i < cubeItems.Count; i++)
            {
                if (canLockIndexs.Contains(cubeItems[i].PosIndex))
                {
                    cubeItems[i].SetLockInfo();
                   
                }
            }
        }

        public bool UnLockItem(int index)
        {
            for (int i = 0; i < cubeItems.Count; i++)
            {
                if (cubeItems[i].PosIndex == index)
                {
                    cubeItems[i].SetLockInfo();
                    return true;
                }
            }

            return false;
        }
    }
}
