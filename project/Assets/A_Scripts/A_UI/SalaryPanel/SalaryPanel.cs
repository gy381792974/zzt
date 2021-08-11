using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class SalaryPanelData : UIDataBase
	{
		public SalaryPanelData()
		{

		}
	}

	public partial class SalaryPanel : UIBase
	{
        [SerializeField] private ScrollViewInfinity scrollViewInfinity;

        List<object> dataList = new List<object>();

        protected override void OnInit()
		{
            scrollViewInfinity.onItemRender.AddListener(OnUpdateItem);
        }

        protected override void OnShow(UIDataBase salarypanelData = null)
		{
			if (salarypanelData != null)
			{
                mPanelData = salarypanelData as SalaryPanelData;
			}
		}

		protected override void OnHide()
		{

		}

        private void OnUpdateItem(int arg0, Transform arg1)
        {
            
        }
    }
}
