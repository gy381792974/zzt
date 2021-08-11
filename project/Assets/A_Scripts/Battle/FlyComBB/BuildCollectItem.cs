using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class BuildCollectItem : FlyBBBase
    {
        public Button collectBtn;
        public Text text;

        private void Awake()
        {
            collectBtn.onClick.AddListener(OnCollectBtnClick);
        }

        protected override void UpateContent()
        {
            text.text = data.num.ToString();
        }

        private void OnCollectBtnClick()
        {
            EventManager.Instance.TriggerEvent(EventKey.RecycleCBBData, this);

            CBBData cBBData = new CBBData();
            cBBData.type = 2;
            cBBData.num = data.num;
            cBBData.tf = data.tf;
            cBBData.id = data.id;
            EventManager.Instance.TriggerEvent(EventKey.SendCBBData, cBBData);
        }
    }
}