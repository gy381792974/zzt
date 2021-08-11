using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EazyGF
{

    [System.Serializable]
    public class BubbleData
    {
        public int type; //0 不开心 1开心
        public string txt;
        public Transform cs;

        public BubbleData(int type, string txt, Transform cs)
        {
            this.type = type;
            this.txt = txt;
            this.cs = cs;
        }
    }

    [System.Serializable]
    public class BubbleSerData
    {
        public Dictionary<int, int> sCusGTStallDic;
    }

    public class CustomerBubbleMgr : SingleClass<CustomerBubbleMgr>
    {
        Dictionary<int, int> cusGTStallDic = new Dictionary<int, int>();

        private bool isUserBubble = true;

        private string savaDataName = "customerBubble.data";
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


        public override void Init()
        {
            if (!ReadData())
            {
                SaveData();
            }
        }
  
        public void TargetBubble(CustomerNor cn)
        {
            cn.ShowBBType = -1;

            if (!isUserBubble)
            {
                return;
            }

            if (cn.Data.MustGoStall == -1)
            {
                return;
            }

            int targetBuildLevel = BuildMgr.GetUserBuildLevelById(cn.Data.MustGoStall);
            int needLevel = cn.BubbleData.ConParam;

            int id = cn.Data.ID;

            if (targetBuildLevel >= needLevel)
            {
                if (GetGTStallNum(id) == 0)
                {
                    AddGTStallNum(id);

                    string txt = LanguageMgr.GetTranstion(cn.BubbleData.HappyTxt);
                    BubbleData bubbleData = new BubbleData(1, txt, cn.transform);
                    EventManager.Instance.TriggerEvent(EventKey.SendBubbleData, bubbleData);

                    cn.ShowBBType = 1;
                }
            }
            else
            {
                AddGTStallNum(id);
                if (GetGTStallNum(id) >= cn.BubbleData.SpeStallNum)
                {
                    string title = LanguageMgr.GetTranstion(cn.Data.Name);
                    bool isShowDialog = false;

                    if (isShowDialog) //是否显示对话框
                    {
                        UIMgr.ShowPanel<ComDialogPanel>(new ComDialogPanelData(0, "", title, () =>
                        {
                            string txt = LanguageMgr.GetTranstion(cn.BubbleData.RantTxt);
                            BubbleData bubbleData = new BubbleData(0, txt, cn.transform);
                            EventManager.Instance.TriggerEvent(EventKey.SendBubbleData, bubbleData);
                            cn.ShowBBType = 0;
                        },
                       () =>
                       {
                           
                       }
                       ));
                    }
                    else
                    {
                        string txt = LanguageMgr.GetTranstion(cn.BubbleData.RantTxt);

                        BubbleData bubbleData = new BubbleData(0, txt, cn.transform);

                        EventManager.Instance.TriggerEvent(EventKey.SendBubbleData, bubbleData);
                        cn.ShowBBType = 0;
                    }

                }
            }
        }

        public void AddGTStallNum(int id)
        {
            if (cusGTStallDic.ContainsKey(id))
            {
                cusGTStallDic[id]++;
            }
            else
            {
                cusGTStallDic.Add(id, 1);
            }

            SaveData();
        }

        public int GetGTStallNum(int id)
        {
            if (cusGTStallDic.TryGetValue(id, out int value))
            {
              return value;
            }

            return 0;
        }

        public bool ReadData()
        {
            BubbleSerData serData = SerializHelp.DeserializeFileToObj<BubbleSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                cusGTStallDic = serData.sCusGTStallDic;
            }

            return loadSuccess;
        }

        public void SaveData()
        {
            BubbleSerData serData = new BubbleSerData();
            serData.sCusGTStallDic = cusGTStallDic;
            SerializHelp.SerializeFile(SavePath, serData);
        }
    }
}