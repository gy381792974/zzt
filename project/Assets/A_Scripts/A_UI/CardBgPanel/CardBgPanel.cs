using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class CardBgPanelData : UIDataBase
	{
		
		public CardBgPanelData()
		{

		}
	}

	public partial class CardBgPanel : UIBase
	{
		public int Card_id;
		public Sprite[] StarUpNum;//设施图集
		public Sprite[] TanWeiSprites;//摊位图集
		

		private void Start()
        {

            var num = TANWEI_Data.GetTANWEI_DataByID(Card_id).star;
            UpstarXnum_img.sprite = StarUpNum[num];

            GoldBuy_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<FacilityPanel>();
                UIMgr.GetUI<FacilityPanel>().IconImageChange(Card_id, IsBuyGoods);

            });

            Upstar_btn.onClick.AddListener(() =>
            {

                UpStarOnClick();
            });

            NoUse_btn.onClick.AddListener(() =>
            {
                NoUse_btn.Hide();
                InUse_btn.Show();
                Debug.Log("使用中！！！");
            });
        }

	

        protected override void OnInit()
		{
			
			
		}
		
        protected override void OnShow(UIDataBase cardbgpanelData = null)
		{
			if (cardbgpanelData != null)
			{
                mPanelData = cardbgpanelData as CardBgPanelData;
			}
			IsBuyGoods();
		}


        protected override void OnHide()
		{

		}

		/// <summary>
		/// 星星点击购买
		/// </summary>
		public void UpStarOnClick()
		{
			//设施进入
			if (GlobeFunction.isOpenStar==true)
            {
				//	Upstar_btn.onClick.RemoveAllListeners();
				if (PlayerDataMgr.g_playerData.starNum >= SHESHI_Data.GetSHESHI_DataByID(Card_id).star)
				{
					PlayerDataMgr.g_playerData.starNum -= SHESHI_Data.GetSHESHI_DataByID(Card_id).star;

					
					Bg_img.sprite = Bg2_img.sprite;//bg为红色
					photo_img.sprite = StarUpNum[Card_id];
					Upstar_btn.Hide();
					GoldBtnTextShowNum();

				
					//更新

					//UIMgr.GetUI<ShopPanel>().EachCardList();

					if (PlayerDataMgr.g_playerData.starNum <= 0)
					{
						PlayerDataMgr.g_playerData.starNum = 0;
					}
					
				}
				//金币不足处理
				else
                {
					return;
                }
			}

            else if(GlobeFunction.isOpenStar == false)

			{
                //摊位进入
                if (PlayerDataMgr.g_playerData.starNum >= TANWEI_Data.GetTANWEI_DataByID(Card_id).star)
                {
                    PlayerDataMgr.g_playerData.starNum -= TANWEI_Data.GetTANWEI_DataByID(Card_id).star;


                    Bg_img.sprite = Bg2_img.sprite;//bg为红色
                    photo_img.sprite = TanWeiSprites[Card_id];
                    Upstar_btn.Hide();
                    GoldBtnTextShowNum();


                    //更新

                    //UIMgr.GetUI<ShopPanel>().EachCardList();

                    if (PlayerDataMgr.g_playerData.starNum <= 0)
                    {
                        PlayerDataMgr.g_playerData.starNum = 0;
                    }

                }
                else
                {
                    //金币不足处理
                    return;
                }
            }



        }

		/// <summary>
		/// 判断星星到达数量显示UI
		/// </summary>
		public void UpStar()
		{
			if (PlayerDataMgr.g_playerData.starNum >= SHESHI_Data.GetSHESHI_DataByID(Card_id).star)
			{

				Upstar_btn.Show();
	
			}
			else
			{

				Upstar_btn.Hide();
			}
		}


		/// <summary>
		/// 读取金币显示数量UI
		/// </summary>
		public void GoldBtnTextShowNum()
        {
			GoldBug_text.text = SHESHI_Data.GetSHESHI_DataByID(Card_id).GOLD.ToString();
			GoldBug_text.text = TANWEI_Data.GetTANWEI_DataByID(Card_id).GOLD.ToString();
			GoldBuy_btn.Show();		
		}


		public void IsBuyGoods()
        {
			if (GlobeFunction.isBuyGoods == true)
			{
				//购买了才
				//GoldBuy_btn.gameObject.SetActive(false);
				Bg_img.sprite = Bg1_img.sprite;//bg为紫色
				GoldBuy_btn.Hide();
				NoUse_btn.Show();			
			}
			else
			{
				return;
			}
	
		}

	}
}
