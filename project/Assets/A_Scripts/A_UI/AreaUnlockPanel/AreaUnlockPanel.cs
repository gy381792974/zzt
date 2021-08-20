using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class AreaUnlockPanelData : UIDataBase
    {
        public int id;
        public int level;
        public int type;
        public int areaIndex;
        public BuildDataModel data;
        public AreaUnlockPanelData(BuildDataModel data)
        {
            this.id = data.Id;
            this.level = data.Level;
            this.type = data.Type;
            this.data = data;
        }
        public AreaUnlockPanelData(BuildAreaItem build)
        {
            this.id = build.id;
            this.type = -1;
            this.areaIndex = build.AreaType;
        }
    }

    public partial class AreaUnlockPanel : UIBase
    {
        int needCoin;
        protected override void OnInit()
        {
            build_btn.onClick.AddListener(UnlockArea);
        }

        protected override void OnShow(UIDataBase buildunlockpanelData = null)
        {
            if (buildunlockpanelData != null)
            {
                mPanelData = buildunlockpanelData as AreaUnlockPanelData;
            }
            GetPropetyByType();
        }

        protected override void OnHide()
        {



        }
        //LanguageMgr.GetTranstion(2, 1, adorn.UnlockCoin)
        private void GetPropetyByType()
        {
            //if (type == 1)
            //{
            //    Stall_Property stall = BuildMgr.GetStall_Property(mPanelData.id, mPanelData.level);
            //    needCoin = stall.UnlockCoin;
            //    UpdateContent(AssetMgr.Instance.LoadTexture("buildtex", stall.IconName),
            //        AssetMgr.Instance.LoadTexture("AreaTitle", stall.title),
            //         LanguageMgr.GetTranstion(stall.BuildIntro), $"{needCoin}");
            //}
            //else
            //{
            //    BuildArea_Property area = BuildArea_Data.GetBuildArea_DataByID(mPanelData.id);
            //    needCoin = area.needCoin;
            //    UpdateContent(AssetMgr.Instance.LoadTexture("AreaTexture", area.icon),
            //        AssetMgr.Instance.LoadTexture("AreaTitle", area.title),
            //         LanguageMgr.GetTranstion(area.desc), $"{needCoin}");
            //}
            AreaUnlock_Property area = AreaUnlock_Data.GetAreaUnlock_DataByID(mPanelData.id);
            needCoin = area.needCoin;
            UpdateContent(AssetMgr.Instance.LoadTexture("AreaTexture", area.icon),
                   AssetMgr.Instance.LoadTexture("AreaTitle", area.title),
                     LanguageMgr.GetTranstion(area.desc), $"{needCoin}");


        }

        public void UpdateContent(Sprite sprite, Sprite spriteTitle, string desc, string buildTxt)
        {
            //build_btn.interactable = isBuild;
            title_img.sprite = spriteTitle;
            title_img.SetNativeSize();
            area_img.sprite = sprite;
            area_img.SetNativeSize();
            area_img.transform.localScale = mPanelData.type == 1 ? Vector3.one * 2.5f : Vector3.one;

            //区域描述文本
            areaDes_text.text = desc;
            //建造{0}
            build_text.text = LanguageMgr.GetTranstion(2, 1);
            coin_text.text = buildTxt;
        }

        private void UnlockArea()
        {
            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, needCoin))
            {
                if (mPanelData.type == 1)
                {
                    mPanelData.level++;
                    BuildMgr.UpgradeBuilding(mPanelData.id, mPanelData.level);
                }
                else
                {
                    BuildAreaMgr.Instance.TriggerUnlockArea(mPanelData.areaIndex, mPanelData.id);
                }
                UIMgr.HideUI<AreaUnlockPanel>();
            }
        }

    }
}
