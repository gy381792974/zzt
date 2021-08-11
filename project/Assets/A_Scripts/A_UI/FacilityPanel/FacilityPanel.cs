using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class FacilityPanelData : UIDataBase
	{
		public FacilityPanelData()
		{

		}
	}

	public partial class FacilityPanel : UIBase
	{
		public Sprite[] SheshiSprites;
	
	//	int goldid;
        protected override void OnInit()
		{
			//        foreach (var item in TANWEI_Data.DataArray)
			//        {
			//goldid = item.ID;
			//        }

		}

        protected override void OnShow(UIDataBase facilitypanelData = null)
		{
			if (facilitypanelData != null)
			{
                mPanelData = facilitypanelData as FacilityPanelData;
			}

			FacilityClose_btn.onClick.AddListener(() =>
			{
				UIMgr.HideUI<FacilityPanel>();
			});
		}

		protected override void OnHide()
		{
		//	LanguageMgr.GetTranstion(3,2);
		}

		/// <summary>
		/// 读取外部图标置换
		/// </summary>
		/// <param name="card"></param>
		public void IconImageChange(int card, Action isBuyGoods)
        {
			this.isBuyGoods = null;
			this.isBuyGoods = isBuyGoods;
			//var num = TANWEI_Data.GetTANWEI_DataByID(bgPanel.Card_id).ID;
			Facilityphoto_img.sprite = SheshiSprites[card];
			SortCard(card);
		}
		Action isBuyGoods;

		/// <summary>
		/// 读取设施详情配置表信息
		/// </summary>
		/// <param name="type"></param>
		public void SortCard(int type)
        {
			GoldIconShowOnHide(type);
			GoldBuyClick(type);
			switch (SHESHI_Data.GetSHESHI_DataByID(type).type)
            {

				case 1:
					FacilityTitle_text.text = LanguageMgr.GetTranstion(3, 1);
					FacilityJieshao_text.text = LanguageMgr.GetTranstion(3, 2);
					Effect_text.text = LanguageMgr.GetTranstion(3, 3);
					Effect1_text.text = LanguageMgr.GetTranstion(3, 4);
					Effect2_text.text = LanguageMgr.GetTranstion(3, 5);
					GoldBuy_text.text = SHESHI_Data.GetSHESHI_DataByID(type).GOLD.ToString();
					break;
				case 2:
					FacilityTitle_text.text = LanguageMgr.GetTranstion(4, 1);
					FacilityJieshao_text.text = LanguageMgr.GetTranstion(4, 2);
					Effect_text.text = LanguageMgr.GetTranstion(4, 3);
					Effect1_text.text = LanguageMgr.GetTranstion(4, 4);
					Effect2_text.text = LanguageMgr.GetTranstion(4, 5);
					GoldBuy_text.text = SHESHI_Data.GetSHESHI_DataByID(type).GOLD.ToString();
					break;
				case 3:
					FacilityTitle_text.text = LanguageMgr.GetTranstion(5, 1);
					FacilityJieshao_text.text = LanguageMgr.GetTranstion(5, 2);
					Effect_text.text = LanguageMgr.GetTranstion(5, 3);
					Effect1_text.text = LanguageMgr.GetTranstion(5, 4);
					Effect2_text.text = LanguageMgr.GetTranstion(5, 5);
					GoldBuy_text.text = SHESHI_Data.GetSHESHI_DataByID(type).GOLD.ToString();
					break;
				
				default:
                    break;
            }
        }


		/// <summary>
		/// 金币到达数量时激活
		/// </summary>
		public void GoldIconShowOnHide(int type)
        {
			if(PlayerDataMgr.g_playerData.goldNum>= SHESHI_Data.GetSHESHI_DataByID(type).GOLD)
            {
				GoldBuy_btn.interactable = true;
            }
			else
            {
				GoldBuy_btn.interactable = false;
			}
        }

		/// <summary>
		/// 蓝色金币购买按钮点击
		/// </summary>
		/// <param name="type"></param>
		public void GoldBuyClick(int type)
        {
			GoldBuy_btn.onClick.RemoveAllListeners();
			GoldBuy_btn.onClick.AddListener(() =>
			{
				
				Debug.Log(SHESHI_Data.GetSHESHI_DataByID(type).GOLD);
				Debug.Log(PlayerDataMgr.g_playerData.goldNum);
				PlayerDataMgr.g_playerData.goldNum -= SHESHI_Data.GetSHESHI_DataByID(type).GOLD;
				GoldBuy_btn.interactable = false;
				GlobeFunction.isBuyGoods = true;
				isBuyGoods();
				if (PlayerDataMgr.g_playerData.goldNum<=0)
                {
					PlayerDataMgr.g_playerData.goldNum = 0;
				}
			
			});
        }

  
    }
}
