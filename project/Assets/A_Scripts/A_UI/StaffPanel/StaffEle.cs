using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    class StaffEle : MonoBehaviour
    {
        public Text nameTxt;
        public Image iconImg;

        public List<Text> tests;

        public GameObject lockObj;
        public Button unLockBtn;
        public Text unlockPriceTxt;
        public Text unLockCondition;

        public GameObject unlockObj;
        public Button upgradeBtn;
        public Text costTxt;
        public GameObject maxLevelObj;
        public GameObject unMaxLevelObj;


        public Image imgSlider;
        public Text needTime;

        int totalTime = 0;
        int growthTime = 0;
        bool isUpdateQuaProgress = false;

        [SerializeField] Animator animator;

        private void Start()
        {
            unLockBtn.onClick.AddListener(UnLockBtnOnClick);
            upgradeBtn.onClick.AddListener(UgradeBtnOnClick);
        }


        private void UnLockBtnOnClick()
        {
            if (ItemPropsManager.Intance.CoseItem(1, unLockCost))
            {
                StaffMgr.Instance.UpgradeStaff(stallModel.id);
            }
        }

        private void UgradeBtnOnClick()
        {
            if (ItemPropsManager.Intance.CoseItem(1, upgradeCost))
            {
                StaffMgr.Instance.UpgradeStaff(stallModel.id);
                PlayAnim();
            }
        }

        private void PlayAnim()
        {
            animator.Play("StaffUpgrade");
        }

        StaffModel stallModel;
        int unLockCost = 0;
        int upgradeCost = 0;

        public void BindData(StaffModel stallModel)
        {
            this.stallModel = stallModel;

            int id = stallModel.id;

            lockObj.gameObject.SetActive(stallModel.level == 0);

            Staff_Property dataBase = StaffMgr.Instance.GetBaseStaffDataById(id);
            nameTxt.text = LanguageMgr.GetTranstion(dataBase.Name);

            iconImg.sprite = AssetMgr.Instance.LoadTexture("CusIcon", dataBase.IconName);

            int level = stallModel.level;
            if (level == 0)
            {
                level = 1;
            }

            isUpdateQuaProgress = false;

            unlockObj.SetActive(stallModel.level != 0);
            lockObj.SetActive(stallModel.level == 0);

            Staff_Level_Property data = StaffMgr.Instance.GetStaffLevelDataByIdAndLevel(id, level);

            tests[0].text = LanguageMgr.GetTranstion(dataBase.ObjName);

            if (stallModel.skillType != 2)
            {
                tests[1].text = UICommonUtil.GetTranstion(dataBase.SpecialSkill, data.SkllParam);
            }
            else
            {
                tests[1].text = LanguageMgr.GetTranstion(new int[2] { 12, 17 });
            }


            if (stallModel.level == 0)  //没有解锁
            {
                tests[2].gameObject.SetActive(false);

                unlockPriceTxt.text = data.EmpPrice.ToString();

                unLockCost = data.EmpPrice;

                bool isCoinEnough = ItemPropsManager.Intance.GetItemNum(0) >= data.EmpPrice;
                bool isUnlock = BuildMgr.GetUserBuildLevelById(dataBase.UnlockParam) >= 1;

                unLockBtn.interactable = isUnlock;

                unLockCondition.text = "";
                if (!isUnlock)
                {
                    unLockCondition.text = LanguageMgr.GetTranstion(dataBase.UnlockDes);
                }
            }
            else if (data.QuaTime != -1) //可以涨薪
            {
                Staff_Level_Property nextData = StaffMgr.Instance.GetStaffLevelDataByIdAndLevel(id, stallModel.level + 1);
                tests[2].gameObject.SetActive(true);
                tests[2].text = UICommonUtil.GetTranstion(dataBase.SpecialSkill, nextData.SkllParam);

                costTxt.text = nextData.EmpPrice.ToString();
                upgradeCost = nextData.EmpPrice;

                totalTime = data.QuaTime;

                maxLevelObj.SetActive(false);
                unMaxLevelObj.SetActive(true);

                UpdateProgress();
            }
            else  //最大等级
            {
                tests[2].gameObject.SetActive(false);

                maxLevelObj.SetActive(true);
                unMaxLevelObj.SetActive(false);
            }
        }

        private void UpdateProgress()
        {
            isUpdateQuaProgress = false;
            growthTime = (int)(GameMgr.Instance.CurrentDateTime - stallModel.createTime);
            if (growthTime >= totalTime)
            {
                imgSlider.fillAmount = 1;

                needTime.gameObject.SetActive(false);
            }
            else
            {
                imgSlider.fillAmount = (float)growthTime / totalTime;

                needTime.gameObject.SetActive(true);

                needTime.text = TimeHelp.CoverNumberToTimer(totalTime - growthTime, true);

                isUpdateQuaProgress = true;
            }

            upgradeBtn.interactable = growthTime >= totalTime;
        }

        private long lastTime = 0;

        private void Update()
        {
            if (isUpdateQuaProgress)
            {
                if (GameMgr.Instance.CurrentDateTime != lastTime)
                {
                    lastTime = GameMgr.Instance.CurrentDateTime;

                    UpdateProgress();
                }
            }

        }
    }
}
