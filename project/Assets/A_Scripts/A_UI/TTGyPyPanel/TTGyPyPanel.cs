using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class TTGyPyPanelData : UIDataBase
	{
        object obj;

		public TTGyPyPanelData(object obj)
		{
            this.obj = obj;
		}
	}

	public partial class TTGyPyPanel : UIBase
	{

		protected override void OnInit()
		{

		}

        protected override void OnShow(UIDataBase ttgypypanelData = null)
		{
			if (ttgypypanelData != null)
			{
                mPanelData = ttgypypanelData as TTGyPyPanelData;
			}
		}

		protected override void OnHide()
		{

		}
	}
}
