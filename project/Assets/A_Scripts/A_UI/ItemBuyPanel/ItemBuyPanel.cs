using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class ItemBuyPanelData : UIDataBase
    {
        public int itemId;


        public ItemBuyPanelData(int itemId)
        {
            this.itemId = itemId;
        }
    }

    public partial class ItemBuyPanel : UIBase
    {

        protected override void OnInit()
        {
            Close_btn.onClick.AddListener(OnClose_btnClick);
            left_btn.onClick.AddListener(OnLeft_btnClick);
            right_btn.onClick.AddListener(OnRight_btnClick);
            Buy_btn.onClick.AddListener(OnBuy_btnClick);
        }

        protected override void OnShow(UIDataBase itembuypanelData = null)
        {
            if (itembuypanelData != null)
            {
                mPanelData = itembuypanelData as ItemBuyPanelData;
            }

            Item_Property ip = Item_DataBase.GetPropertyByID(mPanelData.itemId);

            Icon_img.sprite = AssetMgr.Instance.LoadTexture(ip.IconDir, ip.IconName);
            Icon_img.SetNativeSize();

            ItemDesc_text.text = LanguageMgr.GetTranstion(ip.Desc);

            num = 1;
            price = ip.Price;
            ItemNum_text.text = "X" + ip.BuyNum.ToString();

            EventManager.Instance.RegisterEvent(EventKey.ItemNumUpdate, ItemNumUpdateEvent);

            SetNumCoin();
        }

        int num = 1;
        int price = 0;

        private void ItemNumUpdateEvent(object arg0)
        {
            SetNumCoin();
        }

        protected override void OnHide()
        {
            EventManager.Instance.RemoveListening(EventKey.ItemNumUpdate, ItemNumUpdateEvent);
        }

        private void SetNumCoin()
        {
            int priceNum = num * price;

            BuyItemNum_text.text = num.ToString();

            int havaMoney = ItemPropsManager.Intance.GetItemNum(1);

            Buy_btn.interactable = havaMoney >= priceNum;

            if (havaMoney >= priceNum)
            {
                Buy_btn.interactable = true;

                left_btn.interactable = true;
                right_btn.interactable = true;
            }
            else
            {
                Buy_btn.interactable = false;

                left_btn.interactable = true;
                right_btn.interactable = false;
            }

            if (num == 1)
            {
                left_btn.interactable = false;
            }

            ItemCostNum_text.text = (num * price).ToString();
        }


        private void OnBuy_btnClick()
        {
            //UIMgr.ShowPanel<ItemBuyPanel>();

            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, num * price, false))
            {
                ItemPropsManager.Intance.AddItem(mPanelData.itemId, num);
            }
            BtnClickAnimation(Buy_btn.transform);
        }

        private void OnRight_btnClick()
        {
            num++;

            SetNumCoin();
        }

        private void OnLeft_btnClick()
        {
            if (num >= 2)
            {
                num--;

                SetNumCoin();
            }
        }

        private void OnClose_btnClick()
        {
            UIMgr.HideUI<ItemBuyPanel>();
        }
    }
}
