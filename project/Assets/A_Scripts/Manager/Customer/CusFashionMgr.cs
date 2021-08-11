using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EazyGF
{
    [System.Serializable]
   public class FashSerData
    {
       public Dictionary<int, Dictionary<int, int>> sFashData;
       public Dictionary<int, int> sUseFashDic;
    }

    public class CusFashionMgr : SingleSD<CusFashionMgr>
    {
        // 顾客id 摊位id 次数
        Dictionary<int, Dictionary<int, int>> fashData = new Dictionary<int, Dictionary<int, int>>();
        Dictionary<int, int> useFashDic = new Dictionary<int, int>();

        public override void Init()
        {
            base.Init();
        }

        protected override void MInitData()
        {
            CustomerNormal_Property[] dataArray = CustomerNormal_Data.DataArray;
            useFashDic.Clear();

            for (int i = 0; i < dataArray.Length; i++)
            {
                useFashDic.Add(dataArray[i].ID, 0);
            }
        }

        protected override string GetSavaDataName()
        {
            return "cusFash.Data";
        }

        protected override void SaveData()
        {
            FashSerData fashSerData = new FashSerData();
            fashSerData.sFashData = fashData;
            fashSerData.sUseFashDic = useFashDic;
            SerializHelp.SerializeFile(SavePath, fashSerData);
        }

        protected override bool ReadData()
        {
            FashSerData itemSerData = SerializHelp.DeserializeFileToObj<FashSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                fashData = itemSerData.sFashData;
                useFashDic = itemSerData.sUseFashDic;
            }

            return loadSuccess;
        }

        public void AddGTStallNum(int cusId, int stallId)
        {
            if (fashData.TryGetValue(cusId, out Dictionary<int, int> value))
            {
                if (value.TryGetValue(stallId, out int v))
                {
                    value[stallId]++;
                }
                else
                {
                    value.Add(stallId, 1);
                }
            }
            else
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();

                dic.Add(stallId, 1);

                fashData.Add(cusId, dic);
            }

            SaveData();
        }

        public int GetGTStallNum(int cusId, int stallId)
        {
            if (fashData.TryGetValue(cusId, out Dictionary<int, int> value))
            {
                if (value.TryGetValue(stallId, out int v))
                {
                    return v;
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        public int CalFashionIndex(int cusId, int statllId, int curFashId = -1)
        {
            CusFashion_Property[] cus = CusFashion_Data.DataArray;

            for (int i = 0; i < cus.Length; i++)
            {
                if (cus[i].ID == cusId && cus[i].StallId == statllId)
                {
                    if (cus[i].FashionId == curFashId)
                    {
                        return -1;
                    }

                    //cus[i].GTNum = 1;

                    if (GetGTStallNum(cusId, statllId) >= cus[i].GTNum)
                    {
                        return cus[i].FashionId;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }

            return -1;
        }

        public int GetFashionByCusId(int cusId)
        {
            if (useFashDic.TryGetValue(cusId, out int value))
            {
                return value;
            }

            return 0;
        }

        public void UpdateFashionIndex(int cusId, int value)
        {
            if (useFashDic.ContainsKey(cusId))
            {
                useFashDic[cusId] = value;

                SaveData();
            }
            else
            {
                Debug.LogError($"没有 cusId = {cusId} 的顾客");
            }
        }
    }
}
