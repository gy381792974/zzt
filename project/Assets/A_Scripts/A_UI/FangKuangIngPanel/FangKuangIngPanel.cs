using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class FangKuangIngPanelData : UIDataBase
	{
		public FangKuangIngPanelData()
		{

		}
	}

	public partial class FangKuangIngPanel : UIBase
	{

		protected override void OnInit()
		{

		}

        protected override void OnShow(UIDataBase fangkuangingpanelData = null)
		{
			if (fangkuangingpanelData != null)
			{
                mPanelData = fangkuangingpanelData as FangKuangIngPanelData;
			}
		}

		protected override void OnHide()
		{

		}
	}
}
