using System.Collections.Generic;

namespace EazyGF
{
    [System.Serializable]
    public class BuildCollcetSerData
    {
        public Dictionary<int, int> SBCollctCoinDic;
        public Dictionary<int[], int> SEquipCollectDic;
    }


    public class BuildCollectMgr : SingleClass<BuildCollectMgr>
    {
        private static string savaDataName = "buildCollect.data";
        private static string savePath;

        public static int cCoinMinShowNum = 10;

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

        //id，coin
        public Dictionary<int, int> bCollctCoinDic = new Dictionary<int, int>();

        //设备的
        public Dictionary<int[], int> equipCollectDic = new Dictionary<int[], int>();

        public override void Init()
        {
            if (!ReadBuildData())
            {
                SaveData();
            }
        }

        //存到建筑中
        public int AddCoin(int id, int coin)
        {
            int total = coin;
            if (bCollctCoinDic.TryGetValue(id, out int value))
            {
                total = value + coin;
                bCollctCoinDic[id] = total;
            }
            else
            {
                bCollctCoinDic.Add(id, coin);
            }

            SaveData();

            return total;
        }

        public void GetCoin(int id)
        {
            if (bCollctCoinDic.TryGetValue(id, out int value))
            {
                if (value > 0)
                {
                    ItemPropsManager.Intance.AddItem(1, value);

                    bCollctCoinDic[id] = 0;

                    SaveData();
                }
            }
        }

        //存到建筑中
        public int AddEquipCoin(int[] id, int coin)
        {
            int total = coin;
            if (equipCollectDic.TryGetValue(id, out int value))
            {
                total = value + coin;
                equipCollectDic[id] = total;
            }
            else
            {
                equipCollectDic.Add(id, coin);
            }

            SaveData();

            return total;
        }

        public void GetEquipCoin(int[] id)
        {
            if (equipCollectDic.TryGetValue(id, out int value))
            {
                if (value > 0)
                {
                    ItemPropsManager.Intance.AddItem(1, value);

                    equipCollectDic[id] = 0;

                    SaveData();
                }
            }
        }

        public bool ReadBuildData()
        {
            BuildCollcetSerData serData = SerializHelp.DeserializeFileToObj<BuildCollcetSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                bCollctCoinDic = serData.SBCollctCoinDic;
                equipCollectDic = serData.SEquipCollectDic;
            }

            return loadSuccess;
        }

        public void SaveData()
        {
            BuildCollcetSerData buildSerData = new BuildCollcetSerData();
            buildSerData.SBCollctCoinDic = bCollctCoinDic;
            buildSerData.SEquipCollectDic = equipCollectDic;
            SerializHelp.SerializeFile(SavePath, buildSerData);
        }
    }
}