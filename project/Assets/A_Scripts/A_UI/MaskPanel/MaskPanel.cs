using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class MaskPanelData : UIDataBase
	{
		public MaskPanelData()
		{

		}
	}

	public partial class MaskPanel : UIBase
	{

		protected override void OnInit()
		{

		}

        protected override void OnShow(UIDataBase maskpanelData = null)
		{
			if (maskpanelData != null)
			{
                mPanelData = maskpanelData as MaskPanelData;
			}
		}

		protected override void OnHide()
		{

		}
	}
}
