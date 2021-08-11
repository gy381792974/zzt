using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class RoleMapInfoPanelData : UIDataBase
    {
        public int id;
        public int type;
        public bool isUnLock;

        public RoleMapInfoPanelData(int id, int type, bool isUnLock)
        {
            this.id = id;
            this.type = type;
            this.isUnLock = isUnLock;
        }
    }

    public partial class RoleMapInfoPanel : UIBase
    {
        [SerializeField] private Text[] descTxts;
        int index;
        protected override void OnInit()
        {
            ClientClose_btn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<RoleMapInfoPanel>();
            });
            left_btn.onClick.AddListener(Left_Btn);
            right_btn.onClick.AddListener(Right_Btn);
        }

        private void Right_Btn()
        {
            if (index + 1 >= cus.Count)
            {
                return;
            }
            RoleMapData role = cus[index + 1];
            bool isLock = CustomerLogic.IsUnlock((CusType)role.type, role.id);
            OnShow(new RoleMapInfoPanelData(role.id, role.type, isLock));
        }

        private void Left_Btn()
        {
            if (index - 1 < 0)
            {
                return;
            }
            RoleMapData role = cus[index - 1];
            bool isLock = CustomerLogic.IsUnlock((CusType)role.type, role.id);
            OnShow(new RoleMapInfoPanelData(role.id, role.type, isLock));
        }
        List<RoleMapData> cus;
        private int GetIndexByIdAndType(int id, int type)
        {
            cus = CustomerLogic.GetAllLockData();
            for (int i = 0; i < cus.Count; i++)
            {
                if (cus[i].id == id)
                {
                    if (cus[i].type == type)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        protected override void OnShow(UIDataBase rolemapinfopanelData = null)
        {
            if (rolemapinfopanelData != null)
            {
                mPanelData = rolemapinfopanelData as RoleMapInfoPanelData;
            }
            index = GetIndexByIdAndType(mPanelData.id, mPanelData.type);
            List<int[]> datas = new List<int[]>();

            if ((CusType)mPanelData.type == CusType.Normal)
            {
                CustomerNormal_Property cnp = CustomerNormal_DataBase.GetPropertyByID(mPanelData.id);

                string iconName = cnp.IconName;

                if (mPanelData.isUnLock)
                {
                    datas = GetInfos(cnp.Intro);
                }
                else
                {
                    datas = GetInfos(cnp.LockIntro);

                    iconName = iconName + "_shadow";
                }

                SetData(iconName, datas);
            }
            else if ((CusType)mPanelData.type == CusType.Special)
            {
                CustomerSpecial_Property csp = CustomerSpecial_DataBase.GetPropertyByID(mPanelData.id);

                string iconName = csp.IconName;

                if (mPanelData.isUnLock)
                {
                    datas = GetInfos(csp.Intro);
                }
                else
                {
                    datas = GetInfos(csp.LockIntro);

                    iconName = iconName + "_shadow";
                }

                SetData(iconName, datas);
            }
            else if ((CusType)mPanelData.type == CusType.Npc)
            {
                //Npc_Property csp = Npc_DataBase.GetPropertyByID(mPanelData.id);

                //string iconName = csp.IconName;

                //if (mPanelData.isUnLock)
                //{
                //    datas = GetInfos(csp.Intro);
                //}
                //else
                //{
                //    datas = GetInfos(csp.LockIntro);

                //    iconName = iconName + "_shadow";
                //}

                //SetData(iconName, datas);
            }

        }

        private List<int[]> GetInfos(int[] ints)
        {
            List<int[]> datas = new List<int[]>();

            int length = ints.Length / 2;

            for (int i = 0; i < length; i++)
            {
                datas.Add(new int[] { ints[i * 2], ints[i * 2 + 1] });
            }

            return datas;
        }

        private void SetData(string iconPath, List<int[]> datas)
        {
            RoleIcon_img.sprite = AssetMgr.Instance.LoadTexture("CusIcon", iconPath);

            for (int i = 0; i < descTxts.Length; i++)
            {
                if (i < datas.Count)
                {
                    descTxts[i].gameObject.SetActive(true);
                    descTxts[i].text = LanguageMgr.GetTranstion(datas[i]);
                }
                else
                {
                    descTxts[i].gameObject.SetActive(false);
                }
            }
        }

        protected override void OnHide()
        {

        }
    }
}
