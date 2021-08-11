using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EazyGF
{
    [System.Serializable]
    public class BuildAreaSer
    {
        public Dictionary<int, BuildAreadInfo> sBuildAreaUnlockDic;
        public Dictionary<int, bool> sBuildAreaMash;
    }

     [System.Serializable]
    public class BuildAreadInfo
    {
        public int id;
        public bool isUnlock;
        public int maskAreaId;
        public int type; //0区域 1区域遮罩

        public BuildAreadInfo(int id, bool isUnlock, int maskAreaId, int type = 0)
        {
            this.id = id;
            this.isUnlock = isUnlock;
            this.maskAreaId = maskAreaId;
            this.type = type;
        }
    }

    public class BuildAreaMgr : SingleClass<BuildAreaMgr>
    {
        public Dictionary<int, BuildAreadInfo> buildAreaUnlockDic = new Dictionary<int, BuildAreadInfo>();

        public Dictionary<int, bool> buildAreaMash = new Dictionary<int, bool>();

        private string savaDataName = "buildArea.data";
        private string savePath;
        private string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    savePath = UICommonUtil.GetSerSavePath(savaDataName);
                }

                return savePath;
            }
        }

        public override void Init()
        {
            if (!ReadData())
            {
                BuildMaskArea_Property[] maTabls = BuildMaskArea_Data.DataArray;
                for (int i = 0; i < maTabls.Length; i++)
                {
                    buildAreaMash.Add(maTabls[i].ID, false);
                }

                BuildArea_Property[] baTables = BuildArea_Data.DataArray;
                for (int i = 0; i < baTables.Length; i++)
                {
                    BuildArea_Property data = baTables[i];
                    buildAreaUnlockDic.Add(data.ID, new BuildAreadInfo(data.ID, data.IsUnLock == 1, data.MaskAreaId));
                }
                SaveData();
            }
        }

        public bool ReadData()
        {
            BuildAreaSer serData = SerializHelp.DeserializeFileToObj<BuildAreaSer>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                buildAreaUnlockDic = serData.sBuildAreaUnlockDic;
                buildAreaMash = serData.sBuildAreaMash;
            }

            return loadSuccess;
        }

        public void SaveData()
        {
            BuildAreaSer serData = new BuildAreaSer();           

            serData.sBuildAreaMash = buildAreaMash;
            serData.sBuildAreaUnlockDic = buildAreaUnlockDic;

            SerializHelp.SerializeFile(SavePath, serData);
        }

        //得到区域是否解锁
        public bool GetIsUnLockAreaById(int areaId)
        {
            if (buildAreaUnlockDic.TryGetValue(areaId, out BuildAreadInfo value))
            {
                return value.isUnlock && GetBuildMaskIsUnLock(value.maskAreaId);
            }

            return true;
        }

        public void TriggerUnlockArea(int type, int id)
        {
            if (type == 0)
            {
                UpdateAreaData(id);
            }
            else if (type == 1)
            {
                UpdateBuildMaskData(id);
            }
        }

        //更新区域数据
        public void UpdateAreaData(int id, bool isUnLock = true)
        {
            if (buildAreaUnlockDic.TryGetValue(id, out BuildAreadInfo value))
            {
                if (GetBuildMaskIsUnLock(value.maskAreaId))
                {
                    value.isUnlock = isUnLock;

                    SaveData();

                    EventManager.Instance.TriggerEvent(EventKey.BuildAreaDataUpdate, buildAreaUnlockDic[id]);
                }
            }
            else
            {
                Debug.LogError($"不存区域id = {id}的数据");
            }
        }

        //更新区域遮罩数据
        public void UpdateBuildMaskData(int id, bool isUnLock = true)
        {
            if (buildAreaMash.ContainsKey(id))
            {
                buildAreaMash[id] = isUnLock;

                SaveData();

                BuildAreadInfo buildAreadInfo = new BuildAreadInfo(id, isUnLock, -1, 1);
                EventManager.Instance.TriggerEvent(EventKey.BuildAreaDataUpdate, buildAreadInfo);
            }
            else
            {
                Debug.LogError($"不存区域id = {id}的数据");
            }
        }

        //得到建筑
        public bool GetBuildMaskIsUnLock(int id)
        {
            if (id == -1)
            {
                return true;
            }

            if (buildAreaMash.TryGetValue(id, out bool value))
            {
                return value;
            }

            return true;
        }

        public int[] GetBoundAreas(int areaId)
        {
            int[] ids = new int[2];

            List<int> areaIds = new List<int>();
            ids[0] = -1;
            ids[1] = -1;

            int index = -1;

            foreach (var key in buildAreaUnlockDic.Keys)
            {
                bool isUnlock = GetIsUnLockAreaById(key);

                if (isUnlock)
                {
                    areaIds.Add(key);

                    if (key == areaId)
                    {
                        index = areaIds.Count - 1;
                    }
                }
            }

            if (areaIds.Count == 0 || areaIds.Count == 1 || index == -1)
            {
                ids[0] = -1;
                ids[1] = -1;
            }
            else
            {
                if (index - 1 >= 0)
                {
                    ids[0] = areaIds[index - 1];
                }

                if (index + 1 < areaIds.Count && index + 1 >= 1)
                {
                    ids[1] = areaIds[index + 1];
                }
            }

            Debug.LogWarning($"left{ids[0]} right{ids[1]} ids.length = {areaIds.Count} sel = {areaId}");

            return ids;
        }

    }
}