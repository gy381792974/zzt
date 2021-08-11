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
        public BuildDataModel data;
        public AreaUnlockPanelData(BuildDataModel data)
        {
            this.id = data.Id;
            this.level = data.Level;
            this.type = data.Type;
            this.data = data;
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
            GetPropetyByType(mPanelData.type);
        }

        protected override void OnHide()
        {

        }
        //LanguageMgr.GetTranstion(2, 1, adorn.UnlockCoin)
        private void GetPropetyByType(int type)
        {
            //int coin = ItemPropsManager.Intance.GetItemNum((int)CurrencyType.Coin);
            switch (type)
            {
                case 1:
                    Stall_Property stall = BuildMgr.GetStall_Property(mPanelData.id, mPanelData.level);
                    needCoin = stall.UnlockCoin;
                    UpdateContent(AssetMgr.Instance.LoadTexture("buildtex", stall.IconName),
                         LanguageMgr.GetTranstion(stall.BuildIntro), $"{stall.UnlockCoin}");
                    break;
                case 2:
                    Equip_Property equip = BuildMgr.GetEquip_Property(mPanelData.id, mPanelData.level);
                    needCoin = equip.UnlockCoin;
                    UpdateContent(AssetMgr.Instance.LoadTexture("buildtex", equip.IconName),
                         LanguageMgr.GetTranstion(equip.BuildIntro), $"{equip.UnlockCoin}");
                    break;
                case 3:
                    Adorn_Property adorn = BuildMgr.GetAdorn_Property(mPanelData.id, mPanelData.level);
                    needCoin = adorn.UnlockCoin;
                    UpdateContent(AssetMgr.Instance.LoadTexture("buildtex", adorn.IconName),
                         LanguageMgr.GetTranstion(adorn.BuildIntro), $"{adorn.UnlockCoin}");
                    break;
            }
        }

        public void UpdateContent(Sprite sprite, string desc, string buildTxt)
        {
            //build_btn.interactable = isBuild;
            area_img.sprite = sprite;
            //area_img.SetNativeSize();
            //区域描述文本
            areaDes_text.text = desc;
            //建造{0}
            coin_text.text = buildTxt;
        }

        private void UnlockArea()
        {
            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, needCoin))
            {
                mPanelData.level++;
                BuildMgr.UpgradeBuilding(mPanelData.id, mPanelData.level);
                UIMgr.HideUI<AreaUnlockPanel>();
            }
        }

    }
}
