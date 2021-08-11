using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class StartPanelData : UIDataBase
	{
		public StartPanelData()
		{
			
		}
	}

	public partial class StartPanel : UIBase
	{

		protected override void OnInit()
		{
		
		}

        protected override void OnShow(UIDataBase startpanelData = null)
		{
			if (startpanelData != null)
			{
                mPanelData = startpanelData as StartPanelData;
			}
		}

		protected override void OnHide()
		{
			
		}

	
	}
}
