using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class GuidesPanelData : UIDataBase
	{
		public GuidesPanelData()
		{
			
		}
	}

	

	public partial class GuidesPanel : UIBase
	{
		public Carditme cardItem;
		public List<Carditme> RoleCardList = new List<Carditme>();

      

        protected override void OnInit()
        {

            for (int i = 1; i < CARD_Data.DataArray.Length; i++)
            {				
				var cardObj = Instantiate(cardItem, Content_obj.transform);
				cardObj.cardId = CARD_Data.GetCARD_DataByID(i).ID;
				RoleCardList.Add(cardObj);
			}
		}

        protected override void OnShow(UIDataBase guidespanelData = null)
		{
			if (guidespanelData != null)
			{
                mPanelData = guidespanelData as GuidesPanelData;
			}

			GuideClose_btn.onClick.AddListener(() =>
			{
				UIMgr.HideUI<GuidesPanel>();
			});
		}

		protected override void OnHide()
		{

		}

     
	}
}
