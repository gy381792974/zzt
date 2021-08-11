using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    class SalayEle : MonoBehaviour
    {
        public Image iconImg;
        public Text nameTxt;
    
        public List<Text> tests;

        public GameObject lockObj;
        public Button unLockBtn;
        public Text unlockPriceTxt;
        public Text unLockCondition;

        public GameObject unlockObj;
        public Button upgradeBtn;

        private void Start()
        {
            unLockBtn.onClick.AddListener(unLockBtnOnClick);
            upgradeBtn.onClick.AddListener(upgradeBtnClick);
        }

        private void unLockBtnOnClick()
        {
           
        }

        private void upgradeBtnClick()
        {

        }

        public void BindData(int id)
        {
           
        }
    }
}
