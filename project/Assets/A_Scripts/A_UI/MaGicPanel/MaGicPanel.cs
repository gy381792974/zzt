using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class MaGicPanelData : UIDataBase
	{
		public MaGicPanelData()
		{

		}
	}

	public partial class MaGicPanel : UIBase
	{

		protected override void OnInit()
		{

		}

        protected override void OnShow(UIDataBase magicpanelData = null)
		{
			if (magicpanelData != null)
			{
                mPanelData = magicpanelData as MaGicPanelData;
			}
		}

		protected override void OnHide()
		{

		}
	}
}
