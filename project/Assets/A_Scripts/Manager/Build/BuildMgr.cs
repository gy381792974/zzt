using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{

    [System.Serializable]
    public class BuildDataModel
    {
        int id;
        int type; // 类型 1 摊位 2 设备 3 装饰
        int level; // 等级为0的时候锁住的
        int pos;
        int areaIndex; //所属区域
        int commboType; //就是一个id包含多个建筑的类型 1是椅子 0常规
        public int index;

        public BuildDataModel(int type, int id, int level, int pos, int commboType = 0, int areaIndex = 0)
        {
            this.id = id;
            this.type = type;
            this.level = level;
            this.pos = pos;
            this.areaIndex = areaIndex;
            this.commboType = commboType;
        }

        public int Id { get => id; set => id = value; }
        public int Type { get => type; set => type = value; }
        public int Level { get => level; set => level = value; }

        public int Pos { get => pos; set => pos = value; }
        public int AreaIndex { get => areaIndex; set => areaIndex = value; }
        public int CommboType { get => commboType; set => commboType = value; }
    }

    [System.Serializable]
    public class BuildComModel
    {
        public int pos;
        public int level;

        public BuildComModel(int pos, int level)
        {
            
            this.pos = pos;
            this.level = level;
        }
    }


    [System.Serializable]
    public class BuildSerData
    {
        //public Dictionary<int, BuildDataModel> sUnLockBuildDir;

        //public Dictionary<int, List<int>> sHavaBuildDir;

        public Dictionary<int, BuildDataModel> sUserBuildDir;

        public Dictionary<int, List<BuildComModel>> sBuildCommbo;
    }

    class BuildMgr
    {
        private static string savaDataName = "buildData.data";
        private static string savePath;

        public static string SavePath
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

        public static Dictionary<int, BuildDataModel> unLockBuildDir = new Dictionary<int, BuildDataModel>();

        public static Dictionary<int, List<int>> havaBuildDir = new Dictionary<int, List<int>>();

        public static Dictionary<int, BuildDataModel> userBuildDir = new Dictionary<int, BuildDataModel>(); //使用的建筑

        
        public static Dictionary<int, List<BuildComModel>> buildCommbo = new Dictionary<int, List<BuildComModel>>(); 

        //0收银台 1厨房 2厕所
        private static int[] speBuildIds = new int[] { 22007, 22005, 22008 };

        public static void InitData()
        {
            if (!ReadBuildData())
            {
                InitBuildData();

                GuideMgr.Instance.isFirstEnterGame = true;
            }
            else
            {
                GuideMgr.Instance.isFirstEnterGame = false;
            }

            CustomerLogic.UpdateUnlockData();
        }

        public static void UnLockAllBuild(int level)
        {
            InitBuildData(level);

            CustomerLogic.UpdateUnlockData();
        }

        private static void InitBuildData(int level = 0)
        {
            unLockBuildDir.Clear();
            userBuildDir.Clear();
            havaBuildDir.Clear();

            BuildInital_Property[] datas = BuildInital_Data.DataArray;
            for (int i = 0; i < datas.Length; i++)
            {
                BuildInital_Property data = datas[i];

                BuildDataModel buildDataModel = new BuildDataModel(datas[i].Type, datas[i].ID, datas[i].InitLevel, 
                    datas[i].BuildPos, datas[i].CommboType, datas[i].AreaIndex);

                if (level != 0 && buildDataModel.Level != -1)
                {
                    buildDataModel.Level = level;
                }

                if (buildDataModel.Level != -1 && buildDataModel.CommboType >= 1)
                {
                    buildDataModel.Level = 1;

                    AddCommboBuildById(buildDataModel.Id);
                }

                userBuildDir.Add(datas[i].ID, buildDataModel);

                //unLockBuildDir.Add(datas[i].ID, buildDataModel);
                //for (int j = 1; j <= data.InitLevel; j++)
                //{
                //    AddHaveBuild(data.ID, j, false, false);
                //}
            }

            SaveBuildData();
        }

        private static void SaveBuildData()
        {
            BuildSerData buildSerData = new BuildSerData();

            //buildSerData.sUnLockBuildDir = unLockBuildDir;
            //buildSerData.sHavaBuildDir = havaBuildDir;

            buildSerData.sUserBuildDir = userBuildDir;
            buildSerData.sBuildCommbo = buildCommbo;

            SerializHelp.SerializeFile(SavePath, buildSerData);
        }

        private static bool ReadBuildData()
        {
            BuildSerData buildSerData = SerializHelp.DeserializeFileToObj<BuildSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                //unLockBuildDir = buildSerData.sUnLockBuildDir;
                //havaBuildDir = buildSerData.sHavaBuildDir;

                userBuildDir = buildSerData.sUserBuildDir;
                buildCommbo = buildSerData.sBuildCommbo;
            }

            return loadSuccess;
        }

        public static int GetAllUnlockBuildFoodPriceSum()
        {
            int num = 0;
            foreach (var item in userBuildDir)
            {
                if (item.Value.Type == 1 && item.Value.Level > 0)
                {
                    int foodPrice = GetStall_Property(item.Key, item.Value.Level).FoodPrice;

                    num += foodPrice;
                }
            }

            return num;
        }

        public static int GetStillFoodPrice(int id)
        {
            int level = GetUserBuildLevelById(id);

            return GetStall_Property(id, level).FoodPrice;
        }

        public static bool IsHaveBuildByIdAndLevel(int id, int level)
        {
            if (havaBuildDir.TryGetValue(id, out List<int> data))
            {
                return data.Contains(level);
            }

            return false;
        }

        public static bool IsUseBuild(int id, int level)
        {
            return userBuildDir[id].Level == level;
        }

        //取餐数
        public static int GetStllCapacityNumById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel unlockBuild))
            {
                Stall_PropertyBase stall_PropertyBase = GetStall_Property(id, unlockBuild.Level);

                return stall_PropertyBase.CapacityNum;
            }

            return 0;
        }

        public static int GetAllStallAndBornPosNum()
        {
            int num = 0;

            foreach (var item in userBuildDir)
            {
                if (item.Value.Level > 0 && (item.Value.Type == 1 || item.Value.Type == 3))
                {
                    if (item.Value.Type == 1)
                    {
                        Stall_PropertyBase data = GetStall_Property(item.Key, item.Value.Level);
                        num += data.CapacityNum;
                    }
                    else if (item.Value.Type == 3)
                    {
                        Adorn_Property data = GetAdorn_Data(item.Key, item.Value.Level);
                        num += data.CapacityNum;
                    }
                }
            }

            return num;
        }


        public static List<int> GetAllBornLevels()
        {
            List<int> list = new List<int>();

            return list;
        }

        public static Stall_PropertyBase GetUserStllById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel unlockBuild))
            {
                Stall_PropertyBase stall_PropertyBase = GetStall_Property(id, unlockBuild.Level);

                return stall_PropertyBase;
            }

            return null;
        }

        public static int GetStllQueueNumById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel unlockBuild))
            {
                if (unlockBuild.Level <= 0)
                {
                    return 0;
                }

                Stall_PropertyBase stall_PropertyBase = GetStall_Property(id, unlockBuild.Level);

                return stall_PropertyBase.QueueNum;
            }

            return 0;
        }

        public static int GetQueueNumById(int id, int type)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel unlockBuild))
            {
                if (unlockBuild.Level <= 0)
                {
                    return 0;
                }

                if (type == 1)
                {
                    Stall_PropertyBase tableEle = GetStall_Property(id, unlockBuild.Level);

                    return tableEle.QueueNum;
                }
                else if (type == 3)
                {
                    Adorn_Property tableEle = GetAdorn_Data(id, unlockBuild.Level);

                    return tableEle.CapacityNum;
                }
            }

            return 0;
        }


        public static int CalBuildKey(int type, int id)
        {
            return id;
        }

        public static List<int> GetRangUnlockStall()
        {
            List<int> ids = new List<int>();

            foreach (var item in userBuildDir)
            {
                if (item.Value.Type == 1 && item.Value.Level > 0)
                {
                    ids.Add(item.Key);
                }
            }


            return UICommonUtil.Instance.UpsetData(ids);
        }

        public static List<int> GetRangUnlockBorn()
        {
            List<int> ids = new List<int>();

            foreach (var item in userBuildDir)
            {
                if (item.Value.Type == 3 && item.Value.Level > 0)
                {
                    ids.Add(item.Key);
                }
            }

            return UICommonUtil.Instance.UpsetData(ids);
        }


        public static BuildDataModel GetBuildInfo(int id)
        {

            if (userBuildDir.TryGetValue(id, out BuildDataModel build))
            {
                return userBuildDir[id];
            }

            Debug.LogError(string.Format("id = {0} 的数据", id));
            return null;
        }

        public static int GetMaxUnlockLevelById(int id)
        {
            if (unLockBuildDir.TryGetValue(id, out BuildDataModel build))
            {
                return build.Level;
            }

            Debug.LogError(string.Format("没有找到id = {0}  的数据", id));
            return 0;
        }

        public static int GetUserBuildLevelById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel build))
            {
                return build.Level;
            }

            Debug.LogError(string.Format("没有找到id = {0}  的数据", id));
            return 0;
        }

        public static BuildDataModel GetUserBuildDataById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel build))
            {
                return build;
            }

            Debug.LogError(string.Format("没有找到id = {0}  的数据", id));
            return null;
        }

        public static int GetUserBuildStayTimeById(int id)
        {
            if (userBuildDir.TryGetValue(id, out BuildDataModel build))
            {
                return GetStall_Property(id, build.Level).StayTime;
            }

            Debug.LogError(string.Format("没有找到id = {0}  的数据", id));
            return 0;
        }


        public static Stall_Property GetStall_Property(int id, int level)
        {
            Stall_Property[] stalls = Stall_Data.DataArray;

            for (int i = 0; i < stalls.Length; i++)
            {
                if (stalls[i].ID == id)
                {
                    if (level == 0)
                    {
                        return stalls[i];
                    }
                    else if (level == stalls[i].Level)
                    {
                        return stalls[i];
                    }
                }
            }

            Debug.LogError("没有找到摊位表的数据" + $"{id} + {level}");
            return null;
        }

        public static Adorn_Property GetAdorn_Data(int id, int level)
        {
            Adorn_Property[] data = Adorn_Data.DataArray;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].ID == id)
                {
                    if (level == 0)
                    {
                        return data[i];
                    }
                    else if (level == data[i].Level)
                    {
                        return data[i];
                    }
                }
            }

            Debug.LogError("没有找到摊位表的数据" + $"{id} + {level}");
            return null;
        }

        public static void AddHaveBuild(int id, int level, bool isNote = true, bool isSave = true)
        {
            if (havaBuildDir.ContainsKey(id))
            {
                if (!havaBuildDir[id].Contains(level))
                {
                    havaBuildDir[id].Add(level);
                }
            }
            else
            {
                havaBuildDir.Add(id, new List<int>() { level });
            }

            if (isNote)
            {
                EventManager.Instance.TriggerEvent(EventKey.GetNewBuild, null);
            }

            if (isSave)
            {
                SaveBuildData();
            }
        }

        public static void UpdateUnlockBuild(int id, int level, bool isNote = true, bool isSave = true)
        {
            if (unLockBuildDir.ContainsKey(id))
            {
                unLockBuildDir[id].Level = level;
            }
            else
            {
                Debug.LogError(string.Format("BuildInital 表中没有id ={0}的数据", id));
            }

            if (isNote)
            {
                EventManager.Instance.TriggerEvent(EventKey.UnLockBuildEvent, null);
            }

            if (isSave)
            {
                SaveBuildData();
            }
        }

        public static void UpgradeBuilding(int id, int level)
        {
            AddHaveBuild(id, level, false);
            UpdateSelectBuild(id, level);
        }

        public static void UpdateSelectBuild(int id, int level, bool isNote = true)
        {
            if (userBuildDir.ContainsKey(id))
            {
                userBuildDir[id].Level = level;
            }
            else
            {
                Debug.LogError(string.Format("BuildInital 表中没有id ={0}的数据", id));
            }

            UIMgr.HideUI<ShopNewPanel>();

            if (isNote)
            {
                EventManager.Instance.TriggerEvent(EventKey.SelectBuildEvent, userBuildDir[id]);
            }

            if (id == speBuildIds[0])
            {
                EventManager.Instance.TriggerEvent(EventKey.CheckoutCounterLevelUpdate, userBuildDir[id].Level);
            }

            if (id == speBuildIds[1])
            {
                EventManager.Instance.TriggerEvent(EventKey.KitChenLevelChange, userBuildDir[id].Level);
            }

            CustomerLogic.UpdateReadyData();

            SaveBuildData();
        }

        public static Equip_Property GetEquip_Property(int id, int level)
        {
            Equip_Property[] datas = Equip_Data.DataArray;

            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].ID == id)
                {
                    if (level == 0)
                    {
                        return datas[i];
                    }
                    else if (level == datas[i].Level)
                    {
                        return datas[i];
                    }
                }
            }

            Debug.LogError("没有找到设备表的数据");
            return null;
        }

        public static Adorn_Property GetAdorn_Property(int id, int level)
        {
            Adorn_Property[] datas = Adorn_Data.DataArray;

            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].ID == id)
                {
                    if (level == 0)
                    {
                        return datas[i];
                    }
                    else if (level == datas[i].Level)
                    {
                        return datas[i];
                    }
                }
            }

            Debug.LogError("没有找到设备表的数据");
            return null;
        }

        public static List<int> GetAllBuildIdByType(int type)
        {
            BuildInital_Property[] builds = BuildInital_Data.DataArray;

            List<BuildInital_Property> list = new List<BuildInital_Property>();

            for (int i = 0; i < builds.Length; i++)
            {
                if (builds[i].Type == type)
                {
                    list.Add(builds[i]);
                }
            }

            list.Sort((a, b) =>
            {
                return a.Shop_Index.CompareTo(b.Shop_Index);
            });

            List<int> ids = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                ids.Add(list[i].ID);
            }

            return ids;
        }

        public static int GetStallPosById(int id)
        {
            foreach (var item in userBuildDir)
            {
                if (item.Key == id)
                {
                    return item.Value.Pos;
                }
            }

            Debug.LogError($"ID = {id} 数据没有");

            return -1;
        }

        public static int[] GetEmptyStallIdAndLevel(Dictionary<int, int> quequeDic)
        {
            foreach (var item in userBuildDir)
            {
                if (item.Value.Type == 1 && item.Value.Level > 0)
                {
                    if (quequeDic.TryGetValue(item.Key, out int value))
                    {
                        if (value < GetStllQueueNumById(item.Key))
                        {
                            return new int[] { item.Key, item.Value.Level };
                        }
                    }
                }
            }

            return null;
        }
        public static StallLevel_Property GetStallLevelPropertyByIdAndLevel(int id, int level)
        {
            StallLevel_Property[] stalls = StallLevel_Data.DataArray;
            foreach (var stall in stalls)
            {
                if (stall.ID == id)
                {
                    if (level == 0)
                    {
                        return stall;
                    }
                    else if (stall.Level == level)
                    {
                        return stall;
                    }
                }
            }
            Debug.LogError("没有找到 摊位表 的数据");
            return null;
        }

        public static Chair_Property GetChairByIdAndLevel(int id, int level)
        {
            Chair_Property[] chairs = Chair_Data.DataArray;
            foreach (var chair in chairs)
            {
                if (chair.ID == id)
                {
                    if (level == 0)
                    {
                        return chair;
                    }
                    else if (level == chair.level)
                    {
                        return chair;
                    }
                }
            }

            Debug.LogError("没有找到 厨房表 的数据");
            return null;
        }
        public static KitchenLevel_Property GetKitchenPropertyByIdAndLevel(int id, int level)
        {
            KitchenLevel_Property[] kitchens = KitchenLevel_Data.DataArray;
            foreach (var kitchen in kitchens)
            {
                if (kitchen.ID == id)
                {
                    if (level == 0)
                    {
                        return kitchen;
                    }
                    else if (level == kitchen.level)
                    {
                        return kitchen;
                    }
                }
            }
            Debug.LogError("没有找到 厨房表 的数据");
            return null;
        }
        //得到收银台等级
        public static int GetCheckoutCounterLevel()
        {
            return GetUserBuildLevelById(speBuildIds[0]);
        }

        //得到厨房等级
        public static int GetKitchenLevel()
        {
            return GetUserBuildLevelById(speBuildIds[1]);
        }

        //是否是厨房
        public static bool IsKitChen(int id)
        {
            return id == speBuildIds[1];
        }

        //点餐位
        public static int GetCurTeamNumByStallId(int id)
        {
            return BuildUpgradeMgr.Instance.GetCurTakeMealNum(id);
        }

        //排队数
        public static int GetCurCanQueueNum(int id)
        {
            return BuildUpgradeMgr.Instance.GetCurQueueNum(id);
        }

        //得到食物价格
        public static int GetCurFoodPriceById(int id)
        {
            int level = GetUserBuildLevelById(id);

            return BuildUpgradeMgr.Instance.GetCurLevelFoodSell(id, level);
        }

        public static List<BuildDataModel> GetBuildDatasByArea(int areaIndex)
        {
            List<BuildDataModel> list = new List<BuildDataModel>();
            foreach (var item in userBuildDir)
            {
                if (item.Value.AreaIndex == areaIndex)
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }

        public static void InitCommboData()
        {
            if (buildCommbo.ContainsKey(0))
            {

            }
        }

        public static void AddCommboBuildById(int id)
        {
            if (!buildCommbo.ContainsKey(id))
            {
                BuildCommboInital_Property data = BuildCommboInital_Data.GetBuildCommboInital_DataByID(id);

                int[] levels = data.InitLevel;

                List<BuildComModel> list = new List<BuildComModel>();

                for (int i = 0; i < levels.Length; i++)
                {
                    BuildComModel comData = new BuildComModel(i, levels[i]);
                    list.Add(comData);
                }

                buildCommbo.Add(id, list);
            }
            else
            {
                Debug.LogError($"已近添加过id={id}对应的数据");
            }
        }

        static Dictionary<int, int[]> nBuildDic = new Dictionary<int, int[]>();

        public static List<BuildComModel> GetCommboById(int id)
        {
            if (buildCommbo.TryGetValue(id, out List<BuildComModel> buildComModel))
            {
                return buildComModel;
            }

            Debug.LogError($"没有id={id}对应的数据");
            return null;
        }

        public static int GetCommboBuildLevelById(int id, int pos)
        {
            if (buildCommbo.TryGetValue(id, out List<BuildComModel> buildComModel))
            {
                return buildComModel[pos].level;
            }

            Debug.LogError($"没有id={id}对应的数据");
            return -1;
        }

        public static void UpgradeComBuild(int id, int pos)
        {
            if (buildCommbo.TryGetValue(id, out List<BuildComModel> buildComModel))
            {
                int level = buildComModel[pos].level;

                if (level >= 4)
                {
                    Debug.LogError("等级达到满级 无法在升级");
                    return;
                }

                level++;

                buildComModel[pos].level = level;

                BuildDataModel data = new BuildDataModel(2, id, level, pos, 1, 0);

                EventManager.Instance.TriggerEvent(EventKey.BuildComDataUdpate, data);

                SaveBuildData();
            }
        }

        public static List<int> GetCanUseCom(int id)
        {
            List<int> poss = new List<int>();

            if (buildCommbo.TryGetValue(id, out List<BuildComModel> buildComModels))
            {
                for (int i = 0; i < buildComModels.Count; i++)
                {
                    if (buildComModels[i].level > 0)
                    {
                        poss.Add(buildComModels[i].level);
                    }
                }
            }

            return poss;
        }

        public static List<BuildDataModel> GetAllBMDByAreaId(int areaId)
        {
            List<BuildDataModel> list = new List<BuildDataModel>();

            foreach (var item in userBuildDir)
            {
                if (item.Value.AreaIndex == areaId)
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }
    }
}
