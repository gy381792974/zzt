using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class CookPanelData : UIDataBase
    {
        public int id;
        public int index;
        public List<BuildDataModel> buildDataModels;
        public CookPanelData(List<BuildDataModel> buildDataModels, int id, int index)
        {
            this.buildDataModels = buildDataModels;
            this.id = id;
            this.index = index;
        }
    }

    public partial class CookPanel : UIBase
    {
        int id;
        int level; //阶段等级 最大4  通过id和level读表获取表中的数据

        int curBuildLevel = 1; //建筑的等级
        int Upgrade_coin;

        // int index; //建筑在集合里的位置  同时  也是toggle在group中位置
        int[] curLevels;
        KitchenItem item;
        List<BuildDataModel> builds = new List<BuildDataModel>();
        [SerializeField] ScrollViewInfinity infinity;
        [SerializeField] GameObject maxObj;
        protected override void OnInit()
        {
            build_btn.onClick.AddListener(ClickUpgradeBtn);
            infinity.onItemRender.AddListener(UpdateItem);
            cook_obj.gameObject.SetActive(true);
        }
        protected override void OnShow(UIDataBase otherpanelData = null)
        {
            if (otherpanelData != null)
            {
                mPanelData = otherpanelData as CookPanelData;
            }
            InitBuildList();
            Kitchen_Property kitchen = GetKitchenByIndex(mPanelData.index);
            UpdateUI(kitchen);
        }

        protected override void OnHide()
        {


        }

        private void InitBuildList()
        {
            builds.Clear();
            for (int i = 0; i < mPanelData.buildDataModels.Count; i++)
            {
                if (mPanelData.buildDataModels[i].CommboType == 0)
                {
                    builds.Add(mPanelData.buildDataModels[i]);
                }
                else
                {
                    List<BuildComModel> models = BuildMgr.GetCommboById(mPanelData.buildDataModels[i].Id);
                    for (int j = 0; j < models.Count; j++)
                    {
                        BuildDataModel build = new BuildDataModel(mPanelData.buildDataModels[i].Type, mPanelData.buildDataModels[i].Id,
                            models[j].level, models[j].pos, 0, mPanelData.buildDataModels[i].AreaIndex);
                        builds.Add(build);
                    }
                }
            }
            curLevels = BuildUpgradeMgr.Instance.GetCurAreaItem(0, builds.Count);
            infinity.InitScrollView(builds.Count, 4);
        }

        /// <summary>
        /// 无限滚动更新 Item
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void UpdateItem(int arg0, Transform arg1)
        {
            KitchenItem item = arg1.GetComponent<KitchenItem>();
            SetItem(item, arg0);
        }

        private void SetItem(KitchenItem item, int index)
        {
            Kitchen_Property kitchen = BuildMgr.GetKitchenPropertyByIdAndLevel(builds[index].Id, builds[index].Level);
            curBuildLevel = curLevels[index];
            item.SetKitchenData(kitchen, index, curBuildLevel);
            ShowFirstImg(item);
        }
        /// <summary>
        /// 得到第一个为此id的Index，意味着如果有相同的id则只return 第一个的index
        /// </summary>
        /// <returns></returns>
        private int GetBuildDataModelIndex()
        {
            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].Id == id)
                {
                    return i + mPanelData.index;
                }
            }
            Debug.LogError("此建筑不存在");
            return -1;
        }

        private Kitchen_Property GetKitchenByIndex(int index)
        {
            id = builds[index].Id;
            level = builds[index].Level;
            return BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
        }

        //区域内有多少个建筑  读表
        //按钮、文本、图片
        private void ShowCommonText(Kitchen_Property kitchen)
        {
            desc_text.text = LanguageMgr.GetTranstion(kitchen.Intro);
        }
        private IEnumerator ShowBuildNameAndDesc(Kitchen_Property kitchen)
        {
            name_text.text = LanguageMgr.GetTranstion(kitchen.Name) + ":";
            yield return null;
            float width = name_text.rectTransform.sizeDelta.x;
            RectTransform descRt = desc_text.rectTransform;
            descRt.sizeDelta = new Vector2(615 - width, 0);
            desc_text.text = LanguageMgr.GetTranstion(kitchen.Intro);
        }

        private void CookLockText(Kitchen_Property kitchen)
        {
            cookLock_text.text = $"{kitchen.reward} plates for store";
            curLevel_text.text = "";
        }

        private void CookUnlockText(Kitchen_Property kitchen)
        {
            cookFill_img.fillAmount = curBuildLevel / kitchen.maxLevel;
            curLevel_text.text = $"LV.{level}";
            cookMaxLevel_text.text = $"lv.{kitchen.maxLevel}";
            unlockCook_text.text = $"Gain per level {kitchen.reward}";
        }

        /// <summary>
        /// 点击 Switch后的处理
        /// </summary>
        /// <param name="thisSwitch"></param>
        /// <param name="isOn"></param>
        public void ClickSwitch(KitchenItem item, bool isOn)
        {
            if (isOn)
            {
                Icon_img.sprite = item.Img.sprite;
                curBuildLevel = item.CurLevel;
                Kitchen_Property kitchen = GetKitchenByIndex(item.Index);
                UpdateUI(kitchen);
            }
        }

        private void ShowFirstImg(KitchenItem item)
        {
            if (this.item == null && item.Index == mPanelData.index)
            {
                item.SetSwitch(true);
                curBuildLevel = curLevels[item.Index];
                Icon_img.sprite = item.Img.sprite;
            }
        }

        private void UpdateUI(Kitchen_Property kitchen)
        {
            //区分解锁的情况
            //未解锁显示lock 已解锁显示unlock
            cookLock_text.gameObject.SetActive(level < 1);
            unlockCook_obj.SetActive(level >= 1);
            if (unlockCook_obj.activeInHierarchy)
            {
                CookUnlockText(kitchen);
            }
            else    //未解锁
            {
                CookLockText(kitchen);
            }
            //ShowCommonText(kitchen);
            StartCoroutine(ShowBuildNameAndDesc(kitchen));
            ShowCoin(kitchen);
        }

        /// <summary>
        /// 显示 button Text 金币
        /// </summary>
        /// <param name="property"></param>
        private void ShowCoin(Kitchen_Property property)
        {
            if (curBuildLevel == property.maxLevel || level == 0)
            {
                Upgrade_coin = property.upgradePrice[0] * (int)Mathf.Pow(property.upgradePrice[1], level);
                build_text.text = $"建造";
            }
            else
            {
                Upgrade_coin = property.buildPrice[0] * (int)Mathf.Pow(property.buildPrice[1], curBuildLevel - 1);
                build_text.text = $"升级";
            }
            Coin_text.text = Upgrade_coin.ToString();
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        private void ClickUpgradeBtn()
        {
            Kitchen_Property property = BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
            if (curBuildLevel == property.maxLevel || level == 0)
            {
                if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, Upgrade_coin))
                {
                    curBuildLevel = 1;
                    level++;
                    builds[item.Index].Level = level;
                    BuildMgr.UpdateSelectBuild(id, level);
                    property = BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
                    item.SetKitchenData(property, item.Index, curBuildLevel);
                    UpdateUI(property);
                    return;
                }
            }
            else if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, Upgrade_coin))
            {
                curBuildLevel++;
                item.UpdateData(curBuildLevel);
            }
            SaveData();
            ShowCoin(property);
            cookFill_img.fillAmount = curBuildLevel / property.maxLevel;
            BtnInteractable(property);
        }

        private void BtnInteractable(Kitchen_Property property)
        {
            build_btn.interactable = level < 4 || curBuildLevel < property.maxLevel;
            maxObj.SetActive(!build_btn.interactable);
        }

        /// <summary>
        /// 将图片显示在ViewPort中
        /// </summary>
        private void ShowInView()
        {
            Vector2 pos = infinity.content.anchoredPosition;
            float y = infinity.itemSize.y;
            float y2 = infinity.space.y;
            int top = infinity.margin.top;
            pos.y = (y + y2 + top) * (item.Index / 4);
            infinity.content.anchoredPosition = pos;
        }

        private void SaveData()
        {
            curLevels[item.Index] = curBuildLevel;
            BuildUpgradeMgr.Instance.SaveAreaData(0, curLevels);
        }
    }
}
