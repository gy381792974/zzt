using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class ShopNewPanelData : UIDataBase
    {
        public ShopNewPanelData()
        {

        }
    }

    public partial class ShopNewPanel : UIBase
    {
        private int curSelectType;
        Toggle lastToggle = null;
        List<int> dataList = new List<int>();
        [SerializeField] ScrollViewInfinity scrollViewInfinity;
        protected override void OnInit()
        {
            curSelectType = 1;

            Stall_toggle.onValueChanged.AddListener((isSelect) =>
            {
                if (isSelect)
                {
                    SetPage(1);
                    SetToggleScale(Stall_toggle);
                    MusicMgr.Instance.PlayMusicEff("c_shop_tab");
                }
            });

            Facility_toggle.onValueChanged.AddListener((isSelect) =>
            {
                if (isSelect)
                {
                    SetPage(2);
                    SetToggleScale(Facility_toggle);
                    MusicMgr.Instance.PlayMusicEff("c_shop_tab");
                }

            });

            Decora_toggle.onValueChanged.AddListener((isSelect) =>
            {
                if (isSelect)
                {
                    SetPage(3);
                    SetToggleScale(Decora_toggle);
                    MusicMgr.Instance.PlayMusicEff("c_shop_tab");
                }
            });

            Close_btn.onClick.AddListener(() =>
            {
                //HideUI();
                UIMgr.HideUI<ShopNewPanel>();
            });

            //scrollViewInfinity.InitScrollView(0);
            scrollViewInfinity.onItemRender.AddListener(OnUpdateItem);

            SetToggleScale(Stall_toggle);
            SetPage(curSelectType);
        }

        public void UpdateUIInfo(object obj)
        {
            SetPage(curSelectType);
        }

        public void OnUpdateItem(int index, Transform itemTf)
        {
            //Debug.LogWarning("OnUpdateItem" + index);
            NewShopItem item = itemTf.GetComponent<NewShopItem>();

            item.BindData(curSelectType, dataList[index]);
        }

        public void SetPage(int index)
        {
            curSelectType = index;

            dataList.Clear();

            dataList = BuildMgr.GetAllBuildIdByType(curSelectType);
            scrollViewInfinity.InitScrollView(dataList.Count);
        }

        private void SetToggleScale(Toggle toggle)
        {
            if (lastToggle != null)
            {
                lastToggle.transform.localScale = Vector3.one;
            }

            toggle.isOn = true;
            Vector3 scale = toggle.transform.localScale;
            scale.y = 1.1f;
            toggle.transform.localScale = scale;
            lastToggle = toggle;
        }
        private void TextShow()
        {
            stall_text.text = LanguageMgr.GetTranstion(1, 30);
            Facility_text.text = LanguageMgr.GetTranstion(1, 31);
            decora_text.text = LanguageMgr.GetTranstion(1, 32);
        }


        protected override void OnShow(UIDataBase shopnewpanelData = null)
        {
            if (shopnewpanelData != null)
            {
                mPanelData = shopnewpanelData as ShopNewPanelData;
            }
            TextShow();
            UpdateItemText(null);
            EventManager.Instance.RegisterEvent(EventKey.ItemNumUpdate, UpdateItemText);
            EventManager.Instance.RegisterEvent(EventKey.UnLockBuildEvent, UpdateUIInfo);
            EventManager.Instance.RegisterEvent(EventKey.GetNewBuild, UpdateUIInfo);
        }

        private void UpdateItemText(object arg)
        {
            coinNum_text.text = $"{ItemPropsManager.Intance.GetItemNum(1)}";
            starNum_text.text = $"{ItemPropsManager.Intance.GetItemNum(2)}";
        }


        protected override void OnHide()
        {
            EventManager.Instance.RegisterEvent(EventKey.ItemNumUpdate, UpdateItemText);
            EventManager.Instance.RemoveListening(EventKey.GetNewBuild, UpdateUIInfo);
            EventManager.Instance.RemoveListening(EventKey.UnLockBuildEvent, UpdateUIInfo);
        }
    }
}
