using UnityEditor;
using System.Collections.Generic;
using UnityEngine.U2D;
using System.IO;
using UnityEngine;

namespace EazyGF
{
    public enum CurrencyType
    {
        Coin = 1,  //金币
        Star = 2,  //星星
        Bottle = 3   //调料
    }

    [System.Serializable]
    public class ItemSerData
    {
       public Dictionary<int, int> itemProps;
    }

    public class ItemPropsManager
    {

        private static ItemPropsManager intance;

        public static ItemPropsManager Intance
        {
            get
            {
                if (intance == null)
                {
                    intance = new ItemPropsManager();
                    intance.Init();
                }

                return intance;
            }
        }

        private string savaDataName = "PlayerItemPropsData.data";
        private string savePath;

        public string SavePath
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

        //id 1金币 2星星
        private Dictionary<int, int> itemProps = new Dictionary<int, int>();

        public SpriteAtlas atlas;

        public void Init()
        {
            if (!ReadData())
            {
                Item_Property[] datas = Item_Data.DataArray;

                for (int i = 0; i < datas.Length; i++)
                {
                    AddItem(datas[i].ID, datas[i].InitNum);   
                }

                SaveData();
            }
        }

        public void AddAllItem(int num)
        {
            Item_Property[] datas = Item_Data.DataArray;

            for (int i = 0; i < datas.Length; i++)
            {
                AddItem(datas[i].ID, num);
            }

            SaveData();
        }

        public UnityEngine.Sprite GetSpriteByName(string name)
        {
            return atlas.GetSprite(name);
        }

        public int GetItemNum(int id)
        {
            if (itemProps.ContainsKey(id))
            {
                return itemProps[id];
            }

           return 0;
        }

        /// <summary>
        ///    1 金币   Star 2    
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num"></param>
        public void AddItem(int id, int num)
        {
            if (itemProps.ContainsKey(id))
            {
                itemProps[id] = itemProps[id] + num;
            }
            else
            {
                itemProps.Add(id, num);
            }

            if (itemProps[id] < 0)
            {
                itemProps[id] = 0;
            }

            SaveData();
            EventManager.Instance.TriggerEvent(EventKey.ItemNumUpdate, null);
        }

        public bool CoseItem(int id , int num, bool isNote = true, bool isCommonTip = true)
        {
            Item_Property ItemP = Item_DataBase.GetPropertyByID(id);

            if (itemProps[id] - num >= 0)
            {
                itemProps[id] -= num;


                switch (ItemP.UserType)
                {
                    case 1:

                        MusicMgr.Instance.PlayMusicEff("d_shop_buybuilding");
                        break;
                    
                    case 2:

                        //MusicMgr.Instance.PlayMusicEff("d_shop_unlock");
                        break;
                    
                    default:
                        break;
                }

                if (isNote)
                {
                    EventManager.Instance.TriggerEvent(EventKey.ItemNumUpdate, ItemP.UserType);
                }

                SaveData();
                return true;
            }
            else
            {
                MusicMgr.Instance.PlayMusicEff("g_btn_err");

                switch (ItemP.UserType)
                {
                    case 1 :

                        TipCommonTool.Instance.ShowTip(new int[] { 1, 16 });

                        break;
                
                    default:
                        break;
                }

            }

            return false;
        }

        public bool CostCurrency(int coinNum, CurrencyType coinType)
        {
            return CoseItem((int)coinType, coinNum);
        }

        public void AddAward(List<AwardData> awardData)
        {
            for (int i = 0; i < awardData.Count; i++)
            {
                AddItem(awardData[i].id, awardData[i].num);
            }
        }

        //public bool CostCurrency(int coinNum, CurrencyType coinType)
        //{
        //    if (coinType == CurrencyType.Coin)
        //    {
        //        if (PlayerDataMgr.g_playerData.goldNum - coinNum >= 0)
        //        {
        //            PlayerDataMgr.g_playerData.goldNum -= coinNum;

        //            PlayerDataMgr.SaveData();

        //            EventManager.Instance.TriggerEvent(EventKey.ItemNumUpdate, null);

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else if (coinType == CurrencyType.Star)
        //    {
        //        if (PlayerDataMgr.g_playerData.starNum - coinNum >= 0)
        //        {
        //            PlayerDataMgr.g_playerData.starNum -= coinNum;

        //            PlayerDataMgr.SaveData();

        //            EventManager.Instance.TriggerEvent(EventKey.ItemNumUpdate, null);
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    return false;
        //}

        public void SaveData()
        {
            ItemSerData itemSerData = new ItemSerData();
            itemSerData.itemProps = itemProps;
            SerializHelp.SerializeFile(SavePath, itemSerData);
        }

        public bool ReadData()
        {
     
            ItemSerData itemSerData = SerializHelp.DeserializeFileToObj<ItemSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                itemProps = itemSerData.itemProps;
            }

            return loadSuccess;
        }
    }
}
