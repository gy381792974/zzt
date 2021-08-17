using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace EazyGF
{
    [System.Serializable]
    public class BuildStatus
    {
        //建筑id
        //public int id;
        ////建筑等级
        //public int level;
        //当前食物等级
        public int foodLevel;
        //队列数量
        public int queueNum;
        //取餐数量
        public int mealNum;

        public BuildStatus(int foodLevel, int queueNum, int mealNum)
        {
            //this.level = level;
            this.foodLevel = foodLevel;
            this.queueNum = queueNum;
            this.mealNum = mealNum;
        }
    }

    public class BuildUpgradePanelData : UIDataBase
    {
        public int id;
        public int level;
        public int type;
        public int pos;
        public BuildDataModel data;
        public BuildUpgradePanelData(BuildDataModel data)
        {
            id = data.Id;
            level = data.Level;
            type = data.Type;
            pos = data.Pos;
            this.data = data;
        }
    }

    public partial class BuildUpgradePanel : UIBase
    {
        //升级食物需要的金币
        int food_coin;
        //升级建筑物需要的金币
        int build_coin;
        //升级取餐位置需要的金币
        int meal_coin;
        //升级排队位置需要的金币
        int queue_coin;

        int bottleNum;
        int curQueue = 1;
        //int queueNum;
        int curMeal = 1;
        //int mealNum;
        int curFoodLevel = 1;

        BuildStatus bs;
        [SerializeField] GameObject FX;
        [SerializeField] GameObject[] objects;
        protected override void OnInit()
        {
            InitAllButton();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }

        protected override void OnShow(UIDataBase buildupgradepanelData = null)
        {
            if (buildupgradepanelData != null)
            {
                mPanelData = buildupgradepanelData as BuildUpgradePanelData;
            }
            bs = BuildUpgradeMgr.Instance.GetBuildStatusById(mPanelData.id);
            curMeal = bs.mealNum;
            curQueue = bs.queueNum;
            curFoodLevel = bs.foodLevel;
            Stall_Property stall = BuildMgr.GetStall_Property(mPanelData.id, mPanelData.level);
            StallLevel_Property stallLevel = BuildMgr.GetStallLevelPropertyByIdAndLevel(mPanelData.id, mPanelData.level);
            title_img.sprite = AssetMgr.Instance.LoadTexture("AreaTitle", stall.title);
            title_img.SetNativeSize();
            icon_img.sprite = AssetMgr.Instance.LoadTexture("buildTex", stall.IconName);
            //食物图片
            food_img.sprite = AssetMgr.Instance.LoadTexture("FoodIcon", stall.foodIcon);
            food_img.SetNativeSize();
            icon_img.SetNativeSize();
            bottleNum = ItemPropsManager.Intance.GetItemNum((int)CurrencyType.Bottle);
            ShowAllText(stall, stallLevel);
            ShowAllBtnInteractable(stallLevel);
            ShowTakeMealPos(stallLevel);
            ShowQueuePos(stallLevel);
        }

        protected override void OnHide()
        {
            Vector3 point = MainSpace.Instance.stallList[mPanelData.pos - 1].GetShowBuildBoxTf().position;
            EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2,
                new CameraViewMove(point, false));
            SaveData();
        }

        private void InitCoin(StallLevel_Property stall)
        {
            //初始价格(为等级1时的价格)*成长值^（等级-1）;
            food_coin = stall.FoodPrice[0] * (int)Mathf.Pow(stall.FoodPrice[1], curFoodLevel - 1);
            build_coin = stall.BuildPrice[0] * (int)Mathf.Pow(stall.BuildPrice[1], stall.Level - 1);
            meal_coin = stall.TakeMealPrice[0] * (int)Mathf.Pow(stall.TakeMealPrice[1], curMeal - 1);
            queue_coin = stall.QueuePrice[0] * (int)Mathf.Pow(stall.QueuePrice[1], curQueue - 1);
        }

        private void InitAllButton()
        {
            upFood_btn.onClick.AddListener(UpgradeFoodLevel);
            remould_btn.onClick.AddListener(Remould);
            upgradeMeal_btn.onClick.AddListener(UpgradeMealPos);
            Upgrade_btn.onClick.AddListener(UpgradeQueuePos);
        }

        private void ShowAllBtnInteractable(StallLevel_Property stallLevel)
        {
            //食物升级按钮
            upFood_btn.interactable = curFoodLevel < stallLevel.FoodMaxLevel && bottleNum >= stallLevel.BotNeedNum;
            objects[0].SetActive(stallLevel.Level >= 4 && curFoodLevel >= stallLevel.FoodMaxLevel);
            //建筑升级按钮
            remould_btn.interactable = bottleNum >= stallLevel.BotNeedNum && stallLevel.Level < 4 && curFoodLevel >= stallLevel.FoodMaxLevel;
            objects[1].SetActive(stallLevel.Level >= 4);
            //取餐位升级按钮
            upgradeMeal_btn.interactable = curMeal < stallLevel.TakeMealMax;
            objects[2].SetActive(stallLevel.Level >= 4 && curMeal >= stallLevel.TakeMealMax);
            //排队位升级按钮
            Upgrade_btn.interactable = curQueue < stallLevel.MaxQueueNum;
            objects[3].SetActive(stallLevel.Level >= 4 && curQueue >= stallLevel.MaxQueueNum);
        }

        private void ShowAllText(Stall_Property stall, StallLevel_Property stallLevel)
        {
            StartCoroutine(TextController(stall));
            time_text.text = $"{stall.StayTime}s";
            foodMaxLevel_text.text = $"Lv.{stallLevel.FoodMaxLevel}";
            fill_img.fillAmount = (float)curFoodLevel / stallLevel.FoodMaxLevel;
            ShowFoodText(stallLevel);
            ShowTipText(stallLevel);
            MealText(stallLevel);
            QueueText(stallLevel);
            ShowBtnText(stallLevel);
        }

        private void QueueText(StallLevel_Property stallLevel)
        {
            string queue = $" {curQueue}/ {stallLevel.MaxQueueNum}";
            queueNum_text.text = LanguageMgr.GetTranstion(11, 7, queue);
        }

        private void MealText(StallLevel_Property stallLevel)
        {
            string meal = $" {curMeal}/ {stallLevel.TakeMealMax}";
            mealNum_text.text = LanguageMgr.GetTranstion(11, 6, meal);
        }

        private IEnumerator TextController(Stall_Property stall)
        {
            stall_text.text = LanguageMgr.GetTranstion(stall.BuildName) + ":";
            yield return null;
            float width = stall_text.rectTransform.sizeDelta.x;
            RectTransform desRt = stallDes_text.rectTransform;
            desRt.sizeDelta = new Vector2(615 - width, 0);
            stallDes_text.text = LanguageMgr.GetTranstion(stall.BuildIntro);
        }

        private void ShowTipText(StallLevel_Property stallLevel)
        {
            //tip 1  需要的最大调料瓶数量20;
            tip1_text.gameObject.SetActive(bottleNum < stallLevel.BotNeedNum);
            if (tip1_text.gameObject.activeSelf)
            {
                tip1_text.text = LanguageMgr.GetTranstion(11, 18, stallLevel.BotNeedNum);
            }
            //tip 2  最大食物等级10;
            tip2_text.gameObject.SetActive(curFoodLevel < stallLevel.FoodMaxLevel);
            if (tip2_text.gameObject.activeSelf)
            {
                tip2_text.text = LanguageMgr.GetTranstion(11, 17, stallLevel.FoodMaxLevel);
            }
        }

        /// <summary>
        /// 4个按钮的文本
        /// </summary>
        /// <param name="stall"></param>
        private void ShowBtnText(StallLevel_Property stall)
        {
            //升级食物的按钮
            food_coin = stall.FoodPrice[0] * (int)Mathf.Pow(stall.FoodPrice[1], curFoodLevel - 1);
            upFood_text.text = food_coin.ToString();
            //升级建筑物的按钮
            build_coin = stall.BuildPrice[0] * (int)Mathf.Pow(stall.BuildPrice[1], stall.Level - 1);
            remould_text.text = build_coin.ToString();
            //升级取餐位的按钮
            meal_coin = stall.TakeMealPrice[0] * (int)Mathf.Pow(stall.TakeMealPrice[1], curMeal - 1);
            upgradeMeal_text.text = meal_coin.ToString();
            //升级排队位的按钮
            queue_coin = stall.QueuePrice[0] * (int)Mathf.Pow(stall.QueuePrice[1], curQueue - 1);
            Upgrade_text.text = queue_coin.ToString();
        }

        /// <summary>
        /// 改造  //刷新该页面
        /// </summary>
        private void Remould()
        {
            if (mPanelData.level >= 4)
            {
                return;
            }

            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, build_coin))
            {
                BuildStatus bs = BuildUpgradeMgr.Instance.GetBuildStatusById(mPanelData.id);
                bs.foodLevel = 1;
                mPanelData.data.Level++;
                BuildMgr.UpgradeBuilding(mPanelData.id, mPanelData.data.Level);
                OnShow(new BuildUpgradePanelData(mPanelData.data));
                SaveData();
            }

        }

        /// <summary>
        /// 升级食物等级
        /// </summary>
        private void UpgradeFoodLevel()
        {
            StallLevel_Property stall = BuildMgr.GetStallLevelPropertyByIdAndLevel(mPanelData.id, mPanelData.level);
            if (curFoodLevel >= stall.FoodMaxLevel)
            {
                return;
            }

            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, food_coin))
            {
                curFoodLevel++;
                fill_img.fillAmount = (float)curFoodLevel / stall.FoodMaxLevel;
                food_coin = stall.FoodPrice[0] * (int)Mathf.Pow(stall.FoodPrice[1], curFoodLevel - 1);
                upFood_text.text = food_coin.ToString();
                ShowFoodText(stall);
                PlayFoodUpgradeFX();
                PlayUpgradeAnim();
                SaveData();
            }
            upFood_btn.interactable = curFoodLevel < stall.FoodMaxLevel && bottleNum >= stall.BotNeedNum;
            objects[0].SetActive(stall.Level >= 4 && curFoodLevel >= stall.FoodMaxLevel);
        }

        /// <summary>
        /// 升级取餐位置
        /// </summary>
        private void UpgradeMealPos()
        {
            StallLevel_Property stall = BuildMgr.GetStallLevelPropertyByIdAndLevel(mPanelData.id, mPanelData.level);
            if (curMeal >= stall.TakeMealMax)
            {
                return;
            }
            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, meal_coin))
            {
                for (int i = curMeal; i < curMeal + 1; i++)
                {
                    Transform meal = GridMeal_trans.GetChild(i);
                    meal.GetChild(0).gameObject.SetActive(false);
                    meal.GetChild(1).gameObject.SetActive(true);
                }
                curMeal++;
                string takeMealText = $" {curMeal}/ {stall.TakeMealMax}";
                mealNum_text.text = LanguageMgr.GetTranstion(11, 6, takeMealText);
                meal_coin = stall.TakeMealPrice[0] * (int)Mathf.Pow(stall.TakeMealPrice[1], curMeal - 1);
                upgradeMeal_text.text = meal_coin.ToString();
                SaveData();
            }
            upgradeMeal_btn.interactable = curMeal < stall.TakeMealMax;
            objects[2].SetActive(stall.Level >= 4 && curMeal >= stall.TakeMealMax);
        }

        /// <summary>
        /// 升级排队位置
        /// </summary>
        private void UpgradeQueuePos()
        {
            StallLevel_Property stall = BuildMgr.GetStallLevelPropertyByIdAndLevel(mPanelData.id, mPanelData.level);
            if (curQueue >= stall.MaxQueueNum)
            {
                return;
            }

            if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, queue_coin))
            {
                for (int i = curQueue; i < curQueue + 1; i++)
                {
                    Transform queue = GridQueue_trans.GetChild(i);
                    queue.GetChild(0).gameObject.SetActive(false);
                    queue.GetChild(1).gameObject.SetActive(true);
                }
                curQueue++;
                string queueText = $" {curQueue}/ {stall.MaxQueueNum}";
                queueNum_text.text = LanguageMgr.GetTranstion(11, 7, queueText);
                queue_coin = stall.QueuePrice[0] * (int)Mathf.Pow(stall.QueuePrice[1], curQueue - 1);
                Upgrade_text.text = queue_coin.ToString();
                SaveData();
            }
            Upgrade_btn.interactable = curQueue < stall.MaxQueueNum;
            objects[3].SetActive(stall.Level >= 4 && curQueue >= stall.MaxQueueNum);
        }

        private void ShowFoodText(StallLevel_Property stall)
        {
            if (curFoodLevel < stall.FoodMaxLevel)
            {
                food_text.text = $"{stall.FoodSell[0] * (int)Mathf.Pow(stall.FoodSell[1], curFoodLevel - 1)}";
                next_text.text = $"{stall.FoodSell[0] * (int)Mathf.Pow(stall.FoodSell[1], curFoodLevel)}";
            }
            else
            {
                food_text.text = $"{stall.FoodSell[0] * (int)Mathf.Pow(stall.FoodSell[1], curFoodLevel - 1)}";
                UpdateRemouldBtn(stall);
            }
            next_text.gameObject.SetActive(curFoodLevel < stall.FoodMaxLevel);
        }

        private void ShowQueuePos(StallLevel_Property stall)
        {
            for (int i = 0; i < GridQueue_trans.childCount; i++)
            {
                Transform queue = GridQueue_trans.GetChild(i);
                if (i < stall.MaxQueueNum)
                {
                    queue.gameObject.SetActive(true);
                    if (i < curQueue)
                    {
                        queue.GetChild(0).gameObject.SetActive(false);
                        queue.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        queue.GetChild(0).gameObject.SetActive(true);
                        queue.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    queue.gameObject.SetActive(false);
                }
            }
        }
        private void ShowTakeMealPos(StallLevel_Property stall)
        {
            for (int i = 0; i < GridMeal_trans.childCount; i++)
            {
                Transform meal = GridMeal_trans.GetChild(i);
                if (i < stall.TakeMealMax)
                {
                    meal.gameObject.SetActive(true);
                    if (i < curMeal)
                    {
                        meal.GetChild(0).gameObject.SetActive(false);
                        meal.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        meal.GetChild(0).gameObject.SetActive(true);
                        meal.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    meal.gameObject.SetActive(false);
                }
            }
        }
        //更新 升级建筑按钮
        private void UpdateRemouldBtn(StallLevel_Property stall)
        {
            //建筑升级按钮
            remould_btn.interactable = bottleNum >= stall.BotNeedNum && stall.Level < 4 && curFoodLevel >= stall.FoodMaxLevel;
            tip2_text.gameObject.SetActive(curFoodLevel < stall.FoodMaxLevel);
            if (tip2_text.gameObject.activeSelf)
            {
                tip2_text.text = LanguageMgr.GetTranstion(11, 18, stall.FoodMaxLevel);
            }

        }
        private void PlayUpgradeAnim()
        {
            Sequence sqe = DOTween.Sequence();
            sqe.Join(food_text.rectTransform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack));
            sqe.Append(food_text.rectTransform.DOScale(1, 0.4f).SetEase(Ease.OutBack));
        }
        private void PlayFoodUpgradeFX()
        {
            if (FX != null)
            {
                //FX.SetActive(false);
                //FX.SetActive(true);
                ParticleSystem particle = FX.GetComponent<ParticleSystem>();
                particle.Play();
                Debug.Log("播放特效");
            }
        }





        private void SaveData()
        {
            bs.foodLevel = curFoodLevel;
            bs.queueNum = curQueue;
            bs.mealNum = curMeal;
            BuildUpgradeMgr.Instance.SaveData();
        }
    }
}
