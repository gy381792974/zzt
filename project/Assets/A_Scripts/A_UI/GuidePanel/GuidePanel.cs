using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
	public class GuidePanelData : UIDataBase
	{
		public GuidePanelData()
		{

		}
	}

	public partial class GuidePanel : UIBase
	{
		protected override void OnInit()
		{
            targetAreaBtn.onClick.AddListener(()=> {

                Debug.LogError("override OnInit");
            });

        }

        protected override void OnShow(UIDataBase guidepanelData = null)
		{
			if (guidepanelData != null)
			{
                mPanelData = guidepanelData as GuidePanelData;
			}
		}

		protected override void OnHide()
		{

		}
	}
}
