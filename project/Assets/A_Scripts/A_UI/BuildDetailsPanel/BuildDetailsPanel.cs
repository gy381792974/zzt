using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class BuildDetailsPanelData : UIDataBase
    {
        public object data;
        public int type;
    }

    public partial class BuildDetailsPanel : UIBase
    {
        protected override void OnInit()
        {
            GoldBuy_btn.onClick.AddListener(GoldBuy_btnClick);

            FacilityClose_btn.onClick.AddListener(FacilityClose_btnClick);
        }

        protected override void OnShow(UIDataBase builddetailspanelData = null)
        {
            if (builddetailspanelData != null)
            {
                mPanelData = builddetailspanelData as BuildDetailsPanelData;
            }

            type = mPanelData.type;

            if (type == 1)
            {
                Stall_Property data = mPanelData.data as Stall_Property;
                SetData(data.IconName, data.BuildName, data.BuildIntro, data.UnlockCoin, 1, data.CapacityNum, data.Desc);

                id = data.ID;
                level = data.Level;
            }
            else if (type == 2)
            {
                Equip_Property data = mPanelData.data as Equip_Property;
                SetData(data.IconName, data.BuildName, data.BuildIntro, data.UnlockCoin, 1, 0, data.Desc);

                id = data.ID;
                level = data.Level;
            }
            else if (type == 3)
            {
                Adorn_Property data = mPanelData.data as Adorn_Property;
                SetData(data.IconName, data.BuildName, data.BuildIntro, data.UnlockCoin, 1, 0, data.Desc);

                id = data.ID;
                level = data.Level;
            }
            IsShowGoldBtn();
        }

        int needCoin;
        int id;
        int level;
        int type;

        public void SetData(string iconName, int[] buildName, int[] desc, int needCoin, int param1, int param2, int[] descs)
        {
            this.needCoin = needCoin;

            Facilityphoto_img.sprite = AssetMgr.Instance.LoadTexture("BuildTex", iconName);
            FacilityTitle_text.text = LanguageMgr.GetTranstion(buildName);
            FacilityJieshao_text.text = LanguageMgr.GetTranstion(desc);

            int length = descs.Length / 2;

            for (int i = 0; i < infoTexts.Count; i++)
            {
                if (i < length)
                {
                    infoTexts[i].gameObject.SetActive(true);
                    infoTexts[i].text = LanguageMgr.GetTranstion(new int[] { descs[i * 2], descs[(i * 2) + 1] });
                }
                else
                {
                    infoTexts[i].gameObject.SetActive(false);
                }
            }

            GoldBuy_text.text = " " + needCoin.ToString();

            int goldNum = ItemPropsManager.Intance.GetItemNum((int)CurrencyType.Coin);

            GoldBuy_btn.interactable = goldNum >= needCoin;
        }

        protected override void OnHide()
        {

        }
        private void FacilityClose_btnClick()
        {
            UIMgr.HideUI<BuildDetailsPanel>();
        }

        private void GoldBuy_btnClick()
        {
            BtnClickAnimation(GoldBuy_btn.transform);
            if (ItemPropsManager.Intance.CoseItem((int)(CurrencyType.Coin), needCoin))
            {
                BuildMgr.AddHaveBuild(id, level);
                UIMgr.HideUI<BuildDetailsPanel>();
            }
        }
        private void IsShowGoldBtn()
        {
            if (BuildMgr.IsHaveBuildByIdAndLevel(id, level))
            {
                GoldBuy_btn.gameObject.SetActive(false);
            }
            else
            {
                GoldBuy_btn.gameObject.SetActive(true);
            }
        }


    }
}
