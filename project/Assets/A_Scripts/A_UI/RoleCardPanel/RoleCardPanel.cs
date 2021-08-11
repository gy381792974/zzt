using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class RoleCardPanelData : UIDataBase
	{
		public RoleCardPanelData()
		{

		}
	}

	public partial class RoleCardPanel : UIBase
	{

		protected override void OnInit()
		{
	
		}

        protected override void OnShow(UIDataBase rolecardpanelData = null)
		{
			if (rolecardpanelData != null)
			{
                mPanelData = rolecardpanelData as RoleCardPanelData;
			}

			ClientClose_btn.onClick.AddListener(() =>
			{
				UIMgr.HideUI<RoleCardPanel>();
			});
		}

		protected override void OnHide()
		{
			
		}

		/// <summary>
		/// 人物特殊效果信息读取
		/// </summary>
		/// <param name="id"></param>
		public void RoleCardFunctionLanguage(int id)
        {
			
			switch (CARD_Data.GetCARD_DataByID(id).ID)
            {
				case 1:
					Effect1.text = LanguageMgr.GetTranstion(7, 1);
					Effect2.text = LanguageMgr.GetTranstion(7, 2);
					GreenEffect1.text = LanguageMgr.GetTranstion(7, 3);					
					Effect3.text = LanguageMgr.GetTranstion(7, 4);
					break;
				case 2:
					Effect1.text = LanguageMgr.GetTranstion(8, 1);
					Effect2.text = LanguageMgr.GetTranstion(8, 2);
					GreenEffect1.text = LanguageMgr.GetTranstion(8, 3);
					Effect3.text = LanguageMgr.GetTranstion(8, 4);
					break;
				case 3:
					Effect1.text = LanguageMgr.GetTranstion(9, 1);
					Effect2.text = LanguageMgr.GetTranstion(9, 2);
					GreenEffect1.text = LanguageMgr.GetTranstion(9, 3);
					Effect3.text = LanguageMgr.GetTranstion(9, 4);
					break;
				case 4:
					Effect1.text = LanguageMgr.GetTranstion(10, 1);
					Effect2.text = LanguageMgr.GetTranstion(10, 2);
					GreenEffect1.text = LanguageMgr.GetTranstion(10, 3);
					Effect3.text = LanguageMgr.GetTranstion(10, 4);
					break;
				default:
                    break;
            }
        }
	}
}
