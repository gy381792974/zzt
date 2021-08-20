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
        int level;

        int curBuildLevel = 1; //建筑的等级
        int Upgrade_coin;

        int[] curLevels;
        int[] btns;
        KitchenItem item;
        List<BuildDataModel> builds = new List<BuildDataModel>();
        [SerializeField] ScrollViewInfinity infinity;
        [SerializeField] GameObject maxObj;
        [SerializeField] Animator animator;
        protected override void OnInit()
        {
            build_btn.onClick.AddListener(ClickUpgradeBtn);
            infinity.onItemRender.AddListener(UpdateItem);
            cook_obj.gameObject.SetActive(true);
            maxObj.SetActive(false);
            left_btn.onClick.AddListener(LeftBtn);
            right_btn.onClick.AddListener(RightBtn);
        }

        protected override void OnShow(UIDataBase otherpanelData = null)
        {
            if (otherpanelData != null)
            {
                mPanelData = otherpanelData as CookPanelData;
            }
            InitBuildList();
            KitchenLevel_Property kitchen = GetKitchenByIndex(mPanelData.index);
            UpdateUI(kitchen);
            SwitchTitle();
            LeftAndRightBtn();
        }

        protected override void OnHide()
        {
            EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2, new CameraViewMove(GetPositionById(), false));
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
                            models[j].level, models[j].pos, 1, mPanelData.buildDataModels[i].AreaIndex);
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
            KitchenLevel_Property kitchen = BuildMgr.GetKitchenPropertyByIdAndLevel(builds[index].Id, builds[index].Level);
            int curBuildLevel = curLevels[index];
            int id = builds[index].Id;
            item.SetKitchenData(kitchen, index, curBuildLevel, id);
            ShowFirstImg(item);
        }
        /// <summary>
        /// 得到第一个为此id的Index，意味着如果有相同的id则只return 第一个的index
        /// </summary>
        /// <returns></returns>
        private int GetIndexBy(int id)
        {
            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].Id == id)
                {
                    return i;
                }
            }
            Debug.LogError($"没有找到{id}");
            return -1;
        }

        private int GetLevelById(int id)
        {
            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].Id == id)
                {
                    return builds[i].Level;
                }
            }
            Debug.LogError($"没有找到{id}");
            return -1;
        }

        private void LeftBtn()
        {
            List<BuildDataModel> buildList = BuildMgr.GetBuildDatasByArea(btns[0]);
            if (buildList[0].AreaIndex != 8)
            {
                UIMgr.HideUI<CookPanel>();
                UIMgr.ShowPanel<ChairPanel>(new ChairPanelData(buildList, buildList[0].Id, 0));
            }
        }

        private void RightBtn()
        {
            List<BuildDataModel> buildList = BuildMgr.GetBuildDatasByArea(btns[1]);
            if (buildList[0].AreaIndex != 8)
            {
                UIMgr.HideUI<CookPanel>();
                UIMgr.ShowPanel<ChairPanel>(new ChairPanelData(buildList, buildList[0].Id, 0));
            }
        }

        private void LeftAndRightBtn()
        {
            int index = GetIndexBy(mPanelData.id);
            int areaId = builds[index].AreaIndex;
            btns = BuildAreaMgr.Instance.GetBoundAreas(areaId);
            left_btn.gameObject.SetActive(btns[0] != -1);
            right_btn.gameObject.SetActive(btns[1] != -1);
        }



        private KitchenLevel_Property GetKitchenByIndex(int index)
        {
            if (index == -1)
            {
                id = mPanelData.id;
                level = GetLevelById(id);
                return BuildMgr.GetKitchenPropertyByIdAndLevel(mPanelData.id, level);
            }
            id = builds[index].Id;
            level = builds[index].Level;
            return BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
        }

        //区域内有多少个建筑  读表
        //按钮、文本、图片
        private IEnumerator ShowBuildNameAndDesc(KitchenLevel_Property kitchen)
        {
            name_text.text = LanguageMgr.GetTranstion(kitchen.Name) + ":";
            yield return null;
            float width = name_text.rectTransform.sizeDelta.x;
            RectTransform descRt = desc_text.rectTransform;
            descRt.sizeDelta = new Vector2(615 - width, 0);
            desc_text.text = LanguageMgr.GetTranstion(kitchen.Intro);
        }

        private void CookLockText(KitchenLevel_Property kitchen)
        {
            cookLock_text.text = $"{kitchen.reward} plates for store";
            curLevel_text.text = "";
        }

        private void CookUnlockText(KitchenLevel_Property kitchen)
        {
            cookFill_img.fillAmount = (float)curBuildLevel / kitchen.maxLevel;
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
                this.item = item;
                Icon_img.sprite = item.Img.sprite;
                curBuildLevel = item.CurLevel;
                KitchenLevel_Property kitchen = GetKitchenByIndex(item.Index);
                UpdateUI(kitchen);
                EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2, new CameraViewMove(GetPositionById(), true));
            }
        }

        private Vector3 GetPositionById()
        {
            Vector3 position = Vector3.zero;
            int i;
            BuildDataModel bdm = builds[item.Index];
            int pos = bdm.Pos;
            if (bdm.Type == 2)
            {
                i = GetEquipIndexById(id);
                position = MainSpace.Instance.equipList[i].GetShowBuildPos(pos, bdm.Level);
                ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.equipList[i].GetShowBuildMR(pos, bdm.Level));
            }
            return position;
        }

        private int GetEquipIndexById(int id)
        {
            Equip_Property[] equips = Equip_Data.DataArray;
            for (int i = 0; i < equips.Length / 4; i++)
            {
                if (equips[i * 4].ID == id)
                {
                    return i;
                }
            }
            Debug.LogError("在建筑表\" Equip \"中未找到id：" + id);
            return -1;
        }

        private void ShowFirstImg(KitchenItem item)
        {
            if (item.Id == mPanelData.id)
            {
                item.SetSwitch(true);
                curBuildLevel = curLevels[item.Index];
                Icon_img.sprite = item.Img.sprite;
            }
        }

        private void UpdateUI(KitchenLevel_Property kitchen)
        {
            //区分解锁的情况
            //未解锁显示lock 已解锁显示unlock
            cookLock_text.gameObject.SetActive(level < 1);
            unlockCook_obj.SetActive(level >= 1);
            if (unlockCook_obj.activeSelf)
            {
                CookUnlockText(kitchen);
            }
            else    //未解锁
            {
                CookLockText(kitchen);
            }
            StartCoroutine(ShowBuildNameAndDesc(kitchen));
            ShowCoin(kitchen);
            BtnInteractable(kitchen);
        }

        private void SwitchTitle()
        {
            int areaIndex = builds[0].AreaIndex;
            AreaUnlock_Property area = AreaUnlock_Data.GetAreaUnlock_DataByID(areaIndex);
            title_img.sprite = AssetMgr.Instance.LoadAsset<Sprite>("AreaTitle", area.title);
            title_img.SetNativeSize();
        }

        /// <summary>
        /// 显示 button Text 金币
        /// </summary>
        /// <param name="property"></param>
        private void ShowCoin(KitchenLevel_Property property)
        {
            if (curBuildLevel == property.maxLevel || level == 0)
            {
                Upgrade_coin = property.upgradePrice[0] * (int)Mathf.Pow(property.upgradePrice[1], level);
                build_text.text = LanguageMgr.GetTranstion(2, 1);
            }
            else
            {
                Upgrade_coin = property.buildPrice[0] * (int)Mathf.Pow(property.buildPrice[1], curBuildLevel - 1);
                build_text.text = LanguageMgr.GetTranstion(1, 3);
            }
            Coin_text.text = Upgrade_coin.ToString();
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        private void ClickUpgradeBtn()
        {
            KitchenLevel_Property property = BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
            if (curBuildLevel == property.maxLevel || level == 0)
            {
                if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, Upgrade_coin))
                {
                    curBuildLevel = 1;
                    level++;
                    builds[item.Index].Level = level;
                    BuildMgr.UpdateSelectBuild(id, level);
                    property = BuildMgr.GetKitchenPropertyByIdAndLevel(id, level);
                    item.SetKitchenData(property, item.Index, curBuildLevel, id);
                    UpdateUI(property);
                    animator.Play("UIup");
                    return;
                }
            }
            else if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, Upgrade_coin))
            {
                curBuildLevel++;
                item.UpdateData(curBuildLevel);
                ItemPropsManager.Intance.AddItem(3, property.reward);
            }
            SaveData();
            ShowCoin(property);
            cookFill_img.fillAmount = (float)curBuildLevel / property.maxLevel;
            BtnInteractable(property);
        }

        private void BtnInteractable(KitchenLevel_Property property)
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
