using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    class ShopBuildItem : MonoBehaviour
    {
        public GameObject unlockObj;
        public GameObject nextLevelTip;

        public GameObject haveObj;
        public Button selectBtn;

        public GameObject noHaveObj;
        public Button buyBtn;
        public Text costCoinTxt;
        public Transform selectTf;

        //没有解锁

        public GameObject lockObj;
        public GameObject canUnlock;
        public Button unlockBtn;
        public Text starNumTxt;

        [SerializeField] Button icon_btn;
        [SerializeField] Image icon;
        public delegate void GoToTargetPosHandle(Transform tf);
        public event GoToTargetPosHandle onHandle;
        public string itemName;
        public SkeletonGraphic sg;
        public SkeletonGraphic star_sg;
        private void Start()
        {
            selectBtn.onClick.AddListener(SelectBtnClick);
            buyBtn.onClick.AddListener(ClickGoldBuy);
            unlockBtn.onClick.AddListener(UnLockBtnClick);
            icon_btn.onClick.AddListener(BuyBtnClick);
        }

        object obj;

        public void BindData(int type, int id, int level)
        {
            if (type == 1)
            {
                Stall_Property data = BuildMgr.GetStall_Property(id, level);
                SetData(data.ID, data.Level, data.UnlockCoin, data.NeedStar, data.IconName);

                obj = data;
                itemName = LanguageMgr.GetTranstion(data.BuildName);
            }
            else if (type == 2)
            {
                Equip_Property data = BuildMgr.GetEquip_Property(id, level);
                SetData(data.ID, data.Level, data.UnlockCoin, data.NeedStar, data.IconName);

                obj = data;
                itemName = LanguageMgr.GetTranstion(data.BuildName);
            }
            else if (type == 3)
            {
                Adorn_Property data = BuildMgr.GetAdorn_Property(id, level);
                SetData(data.ID, data.Level, data.UnlockCoin, data.NeedStar, data.IconName);

                obj = data;
                itemName = LanguageMgr.GetTranstion(data.BuildName);
            }

            this.type = type;
        }

        int needStar;
        int id;
        int level;
        int type;
        int needCoin;
        public void SetData(int id, int level, int unlockCoin, int needStar, string iconName)
        {
            this.needStar = needStar;
            this.id = id;
            this.level = level;
            needCoin = unlockCoin;
            int maxUnlockLevel = BuildMgr.GetMaxUnlockLevelById(id);

            bool isUnlock = level <= maxUnlockLevel;

            unlockObj.SetActive(isUnlock);
            lockObj.SetActive(!isUnlock);

            if (isUnlock)
            {
                bool isHava = BuildMgr.IsHaveBuildByIdAndLevel(id, level);
                nextLevelTip.SetActive(isHava && level < 4);

                haveObj.SetActive(isHava);
                noHaveObj.SetActive(!isHava);

                costCoinTxt.text = unlockCoin.ToString();

                icon.sprite = AssetMgr.Instance.LoadTexture("BuildTex", iconName);

                if (isHava && BuildMgr.IsUseBuild(id, level))
                {
                    if (onHandle != null)
                    {
                        onHandle(selectTf);
                    }
                }
            }
            else
            {
                long starNum = ItemPropsManager.Intance.GetItemNum(2);

                starNumTxt.text = "x" + needStar.ToString();

                if (level - maxUnlockLevel == 1)
                {
                    canUnlock.SetActive(true);
                }
                else
                {
                    canUnlock.SetActive(false);
                }
            }
        }

        bool isCanBuy = true;

        private void UnLockBtnClick()
        {
            if (!isCanBuy)
            {
                return;
            }

            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Star, needStar, true, false))
            {
                isCanBuy = false;
                sg.gameObject.SetActive(false);
                sg.gameObject.SetActive(true);

                sg.AnimationState.TimeScale = 0.4f;
                sg.AnimationState.SetAnimation(0, "unlock", false);
                MusicMgr.Instance.PlayMusicEff("d_shop_unlock");
                PlayStarUnlockAnim();
                sg.AnimationState.Complete += UnLockBuild;
            }
            else
            {
                UIMgr.ShowPanel<StarNotEnough>();
            }
        }

        private void UnLockBuild(Spine.TrackEntry trackEntry)
        {
            sg.gameObject.SetActive(false); BuildMgr.UpdateUnlockBuild(id, level);
            star_sg.AnimationState.SetAnimation(0, "animation", true);
            sg.AnimationState.Complete -= UnLockBuild;
            isCanBuy = true;
        }

        private void PlayStarUnlockAnim()
        {
            //star_sg.AnimationState.SetAnimation(0, "animation", false);
            star_sg.timeScale = 0.7f;
            star_sg.AnimationState.SetAnimation(0, "unlock", false);
        }


        private void BuyBtnClick()
        {
            BuildDetailsPanelData panelData = new BuildDetailsPanelData();
            panelData.data = obj;
            panelData.type = type;

            UIMgr.ShowPanel<BuildDetailsPanel>(panelData);
        }

        private void ClickGoldBuy()
        {
            if (ItemPropsManager.Intance.CoseItem(1, needCoin))
            {
                BuildMgr.AddHaveBuild(id, level);
            }
        }

        private void SelectBtnClick()
        {
            BuildMgr.UpdateSelectBuild(id, level);

            if (onHandle != null)
            {
                onHandle(selectTf);
            }
        }
    }
}
