using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class StaffPanelData : UIDataBase
    {
        public StaffPanelData()
        {

        }
    }

    public partial class StaffPanel : UIBase
    {
        [SerializeField] private ScrollViewInfinity scrollViewInfinity;

        List<StaffModel> dataList = new List<StaffModel>();

        protected override void OnInit()
        {
            scrollViewInfinity.onItemRender.AddListener(OnUpdateItem);
        }

        protected override void OnShow(UIDataBase salarypanelData = null)
        {
            if (mPanelData != null)
            {
                mPanelData = salarypanelData as StaffPanelData;
            }

            dataList = StaffMgr.Instance.staffs;

            scrollViewInfinity.InitScrollView(dataList.Count);

            EventManager.Instance.RegisterEvent(EventKey.StaffDataUpdate, UpdateData);
        }

        protected override void OnHide()
        {
            EventManager.Instance.RemoveListening(EventKey.StaffDataUpdate, UpdateData);
        }

        private void UpdateData(object obj)
        {
            scrollViewInfinity.InitScrollView(dataList.Count);
        }

        private void OnUpdateItem(int arg0, Transform arg1)
        {
            StaffEle staffEle = arg1.GetComponent<StaffEle>();

            staffEle.BindData(dataList[arg0]);
        }
    }
}
