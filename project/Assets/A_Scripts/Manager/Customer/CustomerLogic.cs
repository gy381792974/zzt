using Spine.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EazyGF
{
    public enum CusType
    {
        Normal = 1,
        Special = 2,
        Npc = 3,
    }
    public struct RoleMapData
    {
        public string iconPath;
        public string name_text;
        public int type;
        public int id;

        public RoleMapData(string iconPath, string name_text, int type, int id)
        {
            this.iconPath = iconPath;
            this.name_text = name_text;
            this.type = type;
            this.id = id;
        }
    }

    [System.Serializable]
    public class CustomerSerData
    {
        public List<CustomerNormal_Property> sUnlockCNs;
        public List<CustomerNormal_Property> sReadyUnLockCNs;
        public List<CustomerSpecial_Property> sUnlockCSs;
    }

    class CustomerLogic
    {
        public static List<CustomerNormal_Property> unlockCNs = new List<CustomerNormal_Property>();
        public static List<CustomerNormal_Property> ReadyUnLockCNs = new List<CustomerNormal_Property>();

        public static List<CustomerSpecial_Property> unlockCSs = new List<CustomerSpecial_Property>();

        private static string savaDataName = "Customer.data";
        private static string customerDataSavePath;
        public static string CustomerDataSavePath
        {
            get
            {
                if (string.IsNullOrEmpty(customerDataSavePath))
                {
#if !UNITY_EDITOR
                customerDataSavePath = Path.Combine(Application.persistentDataPath, savaDataName);
#else
                    customerDataSavePath = Path.Combine(Directory.GetCurrentDirectory(), $"OtherAssets/PlayerData/{savaDataName}");
#endif
                }
                return customerDataSavePath;
            }
        }

        public static void InitData()
        {
            //ReadData();
        }

        public static void SaveData()
        {
            //CustomerSerData serData = new CustomerSerData();
            //serData.sUnlockCNs = unlockCNs;
            //serData.sReadyUnLockCNs = ReadyUnLockCNs;
            //serData.sUnlockCSs = unlockCSs;

            //SerializHelp.SerializeFile(CustomerDataSavePath, serData);
        }

        public static bool ReadData()
        {
            CustomerSerData serData = SerializHelp.DeserializeFileToObj<CustomerSerData>(CustomerDataSavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                unlockCNs = serData.sUnlockCNs;
                ReadyUnLockCNs = serData.sReadyUnLockCNs;
                unlockCSs = serData.sUnlockCSs;
            }

            return loadSuccess;
        }

        public int GetDataNum()
        {
            return 0;
        }

        public static int GetUnlockCusNum()
        {
            return unlockCNs.Count + CustomerSpecial_Data.DataArray.Length;
        }

        public static List<RoleMapData> GetAllLockData()
        {
            List<RoleMapData> list = new List<RoleMapData>();

            CustomerNormal_Property[] cnps = CustomerNormal_Data.DataArray;

            CustomerSpecial_Property[] csps = CustomerSpecial_Data.DataArray;

            //Npc_Property[] npc = Npc_Data.DataArray;

            for (int i = 0; i < cnps.Length; i++)
            {
                CustomerNormal_Property data = cnps[i];

                string name = LanguageMgr.GetTranstion(data.Name);

                list.Add(new RoleMapData(data.IconName, name, 1, data.ID));
            }

            for (int i = 0; i < csps.Length; i++)
            {
                CustomerSpecial_Property data = csps[i];

                string name = LanguageMgr.GetTranstion(data.Name);

                list.Add(new RoleMapData(data.IconName, name, 2, data.ID));
            }

            //for (int i = 0; i < npc.Length; i++)
            //{
            //    Npc_Property data = npc[i];

            //    string name = LanguageMgr.GetTranstion(data.Name);

            //    list.Add(new RoleMapData(data.IconName, name, 3, data.ID));
            //}

            return list;
        }

        public static bool IsUnlock(CusType cusType, int id)
        {
            switch (cusType)
            {
                case CusType.Normal:

                    for (int i = 0; i < unlockCNs.Count; i++)
                    {
                        if (unlockCNs[i].ID ==id)
                        {
                            return true;
                        }
                    }

                    break;
                case CusType.Special:

                    //for (int i = 0; i < unlockCSs.Count; i++)
                    //{
                    //    if (unlockCSs[i].ID == id)
                    //    {
                    //        return true;
                    //    }
                    //}

                    return true;
                   

                case CusType.Npc:

                    return true;

                    
                default:
                    break;
            }

            return false;
        }

        public static void UpdateUnlockData()
        {
            CustomerNormal_Property[] cns = CustomerNormal_Data.DataArray;

            int num = 0;
            for (int i = 0; i < cns.Length; i++)
            {
                CustomerNormal_Property table = cns[i];

                if (AddUnLockCN(table, false))
                {
                    num++;
                }
            }

            if (num > 0)
            {
                SaveData();
            }
        }

        public static bool AddUnLockCN(CustomerNormal_Property table, bool isSave = true)
        {
            if (!unlockCNs.Contains(table) && CNIsUnlock(table))
            {
                unlockCNs.Add(table);

                if (isSave)
                {
                    SaveData();
                }

                return true;
            }

            return false;
        }


        public static bool AddUnLockCS(CustomerSpecial_Property table)
        {
            return false;

            //if (!unlockCSs.Contains(table))
            //{
            //    unlockCSs.Add(table);

            //    SaveData();

            //    return true;
            //}

            //return false;
        }


        public static void UpdateReadyData()
        {
            CustomerNormal_Property[] cns = CustomerNormal_Data.DataArray;

            int addNum = 0;

            for (int i = 0; i < cns.Length; i++)
            {
                CustomerNormal_Property table = cns[i];

                if (!unlockCNs.Contains(table) && !ReadyUnLockCNs.Contains(table) && CNIsUnlock(table))
                {
                    ReadyUnLockCNs.Add(cns[i]);

                    //EventManager.Instance.TriggerEvent(EventKey.Null, null);

                    addNum++;
                }
            }

            if (addNum > 0)
            {
                SaveData();
            }
        }

        public static void RemoveReadyData(CustomerNormal_Property table)
        {
            if (unlockCNs.Contains(table))
            {
                unlockCNs.Remove(table);
            }

            if (!unlockCNs.Contains(table))
            {
                unlockCNs.Add(table);

                UIMgr.ShowPanel<RoleMapInfoPanel>();
            }
        }

        public static int GetNCCreateIndex()
        {
            List<CustomerNormal_Property> cns = CustomerLogic.unlockCNs;

            int totalWeight = 0;

            for (int i = 0; i < cns.Count; i++)
            {
                totalWeight += cns[i].CreatWeight;
            }

            int size = UnityEngine.Random.Range(1, totalWeight + 1);

            int index = 0;

            //Debug.LogWarning(string.Format("size = {0}", size));

            for (int i = 0; i < cns.Count; i++)
            {
                size = size - cns[i].CreatWeight;

                if (size <= 0)
                {
                    index = i;
                    break;
                }
            }

            //Debug.LogWarning(string.Format("index = {0}", index));

            return index;
        }


        //public static int GetStallIndexById(int stallId)
        //{
        //    Stall_Property stall_Property; Stall_DataBase.GetPropertyByID(stallId);

        //    return stall_Property.BuildPos;
        //}

        public static bool CNIsUnlock(CustomerNormal_Property table)
        {
            Dictionary<int, BuildDataModel> userBuildDir = BuildMgr.userBuildDir;

            if (table.StallCondit[0] != -1)
            {
                for (int i = 0; i < table.StallCondit.Length; i++)
                {
                    if (userBuildDir[table.StallCondit[i]].Level <= 0)
                    {
                        return false;
                    }
                }
            }

            if (table.EquipCondit[0] != -1)
            {
                for (int i = 0; i < table.EquipCondit.Length; i++)
                {
                    if (userBuildDir[table.EquipCondit[i]].Level <= 0)
                    {
                        return false;
                    }
                }
            }

            if (table.AdornCondit[0] != -1)
            {
                for (int i = 0; i < table.AdornCondit.Length; i++)
                {
                    if (userBuildDir[table.AdornCondit[i]].Level <= 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static int GetCanBuyFoodNum()
        {
            int rang = UnityEngine.Random.Range(1, 101);

            if (rang <= 30)
            {
                return 1;
            }
            else if (rang > 30 && rang <= 90)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        public static int GetStallIndexById(int id)
        {
           return BuildInital_Data.GetBuildInital_DataByID(id).BuildPos - 1;
        }

        public static int GetAdornPosIndexById(int id)
        {
            return BuildInital_Data.GetBuildInital_DataByID(id).BuildPos - 1;
        }

        static Dictionary<string, SkeletonDataAsset> SkeleDir = new Dictionary<string, SkeletonDataAsset>();

        public static SkeletonDataAsset GetSkeletonDataAssetByName(string bundleName, string spineAssetName)
        {
            if (!SkeleDir.ContainsKey(bundleName))
            {
                SkeletonDataAsset sda = LoadSkeletonDataAsset(bundleName, spineAssetName);

                SkeleDir.Add(bundleName, sda);

                return sda;
            }

            return SkeleDir[bundleName];
        }

        public static SkeletonDataAsset LoadSkeletonDataAsset(string spineAssetBundleName, string spineAssetName)
        {
#if UNITY_EDITOR
            return LoadSkeletonDataAssetInEditor(spineAssetBundleName, spineAssetName);
            //return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
#else
           return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
#endif

        }

        private static SkeletonDataAsset LoadSkeletonDataAssetInAssetBundle(string spineAssetBundleName, string spineAssetName)
        {
            return AssetMgr.Instance.LoadAsset<SkeletonDataAsset>(spineAssetBundleName, $"{spineAssetName}_SkeletonData.asset");
        }

        // 加载本地资源赋值
        private static SkeletonDataAsset LoadSkeletonDataAssetInEditor(string spineAssetBundleName, string spineAssetName)
        {
#if UNITY_EDITOR

            string basePath = $"{AB_ResFilePath.abAllSpinesRootDir}/{spineAssetBundleName}/{spineAssetName}";
            return UnityEditor.AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>($"{basePath}_SkeletonData.asset");
#endif

            return LoadSkeletonDataAssetInAssetBundle(spineAssetBundleName, spineAssetName);
        }

    }
}
