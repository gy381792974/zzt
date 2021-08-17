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

            LocalCommonUtil.ShowBB(2, data.tf, data.id, data.num);

            if (data.type == 1)
            {
                BuildCollectMgr.Instance.GetCoin(data.id);
            }
            else if (data.type == 4)
            {
                BuildCollectMgr.Instance.GetEquipBuildCoin(data.id);
            }
        }
    }
}