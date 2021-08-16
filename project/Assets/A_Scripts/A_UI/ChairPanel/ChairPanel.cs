using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class ChairPanelData : UIDataBase
    {
        public int index;
        public int id;
        public List<BuildDataModel> builds;
        public ChairPanelData(List<BuildDataModel> builds, int id, int index)
        {
            this.id = id;
            this.builds = builds;
            this.index = index;
        }
    }

    public partial class ChairPanel : UIBase
    {
        int id;
        int level;
        int curLevel = 1;
        int upgrade_coin;
        int index;
        //Transform content;
        int[] btns;
        int[] curLevels;
        Image img;
        Toggle[] toggles;
        List<BuildDataModel> builds;
        ChairItem item;
        [SerializeField] ScrollViewInfinity InfinityScroll;
        [SerializeField] GameObject maxObj;
        protected override void OnInit()
        {
            //BuildUpgradeMgr.Instance.Init();
            builds = new List<BuildDataModel>();
            InfinityScroll.onItemRender.AddListener(UpdateItem);
            build_btn.onClick.AddListener(ClickUpgradeBtn);
            chair_obj.SetActive(true);
            left_btn.onClick.AddListener(LeftBtn);
            right_btn.onClick.AddListener(RightBtn);
            maxObj.SetActive(false);
        }

        protected override void OnShow(UIDataBase chairpanelData = null)
        {
            if (chairpanelData != null)
            {
                mPanelData = chairpanelData as ChairPanelData;
            }

            InitBuildsList();
            Chair_Property chair = GetChairByIndex(index);
            SetCurLevel(chair);
            ShowChairStatus(chair);
            ShowImg();
            ShowInView();
            LeftAndRightBtn();
        }

        protected override void OnHide()
        {
            EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2,
      new CameraViewMove(GetPositionById(), false));
        }

        private void UpdateItem(int arg0, Transform arg1)
        {
            ChairItem item = arg1.GetComponent<ChairItem>();
            SetItem(item, arg0);
        }

        //将外部的List转化为内部的List
        private void InitBuildsList()
        {
            builds.Clear();
            for (int i = 0; i < mPanelData.builds.Count; i++)
            {
                if (mPanelData.builds[i].CommboType == 0)
                {
                    builds.Add(mPanelData.builds[i]);
                }
                else
                {
                    List<BuildComModel> buildList = BuildMgr.GetCommboById(mPanelData.builds[i].Id);
                    for (int j = 0; j < buildList.Count; j++)
                    {
                        BuildDataModel build = new BuildDataModel(mPanelData.builds[i].Type, mPanelData.builds[i].Id,
                            buildList[j].level, buildList[j].pos, 1, mPanelData.builds[i].AreaIndex);
                        build.index = j;

                        builds.Add(build);
                    }
                }
            }
            toggles = new Toggle[builds.Count];
            id = mPanelData.id;
            Debug.Log("id:" + id);
            index = GetIndexById();
            InfinityScroll.InitScrollView(builds.Count, 4);

        }

        private void SetItem(ChairItem item, int index)
        {
            Chair_Property chair = GetChairByIndex(index);
            item.SetChairData(chair, index);
            toggles[index] = item.GetComponent<Toggle>();
            Image img = item.Img;
            //item.BindData(builds[index]);
            ShowImage(index, img.sprite);
        }

        private int GetIndexById()
        {
            if (mPanelData.index == -1)
            {
                for (int i = 0; i < builds.Count; i++)
                {
                    if (builds[i].Id == id)
                    {
                        return i;
                    }
                }
            }


            for (int i = 0; i < builds.Count; i++)
            {
                if (builds[i].Id == id)
                {
                    return i + mPanelData.index;
                }
            }
            return -1;
        }

        private int GetPosById()
        {
            for (int i = 0; i < builds.Count; i++)
            {
                if (id == builds[i].Id)
                {
                    return index - i;
                }
            }

            return -1;
        }

        private Chair_Property GetChairByIndex(int index)
        {
            if (index >= 0 && index < builds.Count)
            {
                id = builds[index].Id;
                level = builds[index].Level;
            }
            return BuildMgr.GetChairByIdAndLevel(id, level);
        }

        private void LeftBtn()
        {
            List<BuildDataModel> buildList = BuildMgr.GetBuildDatasByArea(btns[0]);
            if (buildList[0].AreaIndex != 8)
            {
                OnShow(new ChairPanelData(buildList, buildList[0].Id, 0));
            }
            else
            {
                UIMgr.HideUI<ChairPanel>();
                UIMgr.ShowPanel<CookPanel>(new CookPanelData(buildList, buildList[0].Id, -1));
            }
        }

        private void RightBtn()
        {
            List<BuildDataModel> buildList = BuildMgr.GetBuildDatasByArea(btns[1]);
            if (buildList[0].AreaIndex != 8)
            {
                OnShow(new ChairPanelData(buildList, buildList[0].Id, 0));
            }
            else
            {
                UIMgr.HideUI<ChairPanel>();
                UIMgr.ShowPanel<CookPanel>(new CookPanelData(buildList, buildList[0].Id, -1));
            }
        }

        private void LeftAndRightBtn()
        {
            int areaId = builds[index].AreaIndex;
            btns = BuildAreaMgr.Instance.GetBoundAreas(areaId);
            left_btn.gameObject.SetActive(btns[0] != -1);
            right_btn.gameObject.SetActive(btns[1] != -1);
        }

        private int GetIndexByAreaId(int id)
        {
            BuildArea_Property[] area = BuildArea_Data.DataArray;
            for (int i = 0; i < area.Length; i++)
            {
                if (area[i].ID == id)
                {
                    return i;
                }
            }

            Debug.LogError($"此 {id} 不在区域表中");
            return -1;
        }

        public void ClickToggleChangeImage(ChairItem item, bool isOn)
        {
            if (isOn)
            {
                this.item = item;
                img = item.Img;
                Icon_img.sprite = img.sprite;
                Icon_img.SetNativeSize();
                index = item.Index;
                Chair_Property chair = GetChairByIndex(index);
                item.SetItemLevelState(chair);
                SetCurLevel(chair);
                UpdateUI(chair);
                BtnInteraction(chair);
                EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2, new CameraViewMove(GetPositionById(), true));
            }
        }

        private Vector3 GetPositionById()
        {
            Vector3 position = Vector3.zero;
            int i;
            BuildDataModel bdm = builds[index];
            int pos = bdm.Pos;
            if (bdm.Type == 2)
            {
                i = GetEquipIndexById(bdm.Id);
                position = MainSpace.Instance.equipList[i].GetShowBuildPos(pos, bdm.Level);
                ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.equipList[i].GetShowBuildMR(pos, bdm.Level));
            }
            else if (bdm.Type == 3)
            {
                i = GetAdornIndexBy(bdm.Id);
                position = MainSpace.Instance.adornList[i].GetShowBuildPos(pos, bdm.Level);
                ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.adornList[i].GetShowBuildMR(pos, bdm.Level));
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

        private int GetAdornIndexBy(int id)
        {
            Adorn_Property[] adorns = Adorn_Data.DataArray;
            for (int i = 0; i < adorns.Length / 4; i++)
            {
                if (adorns[i * 4].ID == id)
                {
                    return i;
                }
            }
            Debug.LogError("在建筑表\" Adorn \"中未找到id：" + id);
            return -1;
        }
        private void UpdateUI(Chair_Property chair)
        {
            ShowChairStatus(chair);
        }
        private IEnumerator ShowText(Chair_Property chair)
        {
            name_text.text = LanguageMgr.GetTranstion(chair.name) + ":";
            yield return null;
            float width = name_text.rectTransform.sizeDelta.x;
            RectTransform descRt = desc_text.rectTransform;
            descRt.sizeDelta = new Vector2(615 - width, 0);
            desc_text.text = LanguageMgr.GetTranstion(chair.desc);
        }

        private void ShowChairStatus(Chair_Property chair)
        {
            chairUnLock_obj.SetActive(level >= 1);
            chairLock_text.gameObject.SetActive(level < 1);
            StartCoroutine(ShowText(chair));
            if (chairUnLock_obj.activeSelf)
            {
                ShowUnlockText(chair);
            }
            else
            {
                ShowLockText(chair);
            }
            ShowCoin(chair);
        }

        private void ShowLockText(Chair_Property chair)
        {
            chairLock_text.text = $"+{2}";
        }
        private void ShowUnlockText(Chair_Property chair)
        {
            curLevel_text.text = $"LV.{level}";
            //通过字典 赋值 curLevel
            chairFill_img.fillAmount = (float)curLevel / chair.maxLevel;
            chairMaxLevel_text.text = $"lv.{chair.maxLevel}";
            curStage_text.text = $"Current stage: <color=#519f87>Pick up time {2}s</color>";
            nextStage_text.text = $"Next:               <color=#519f87>Pick up time {1.85}s</color>";
        }

        private void ShowCoin(Chair_Property chair)
        {
            if (curLevel == chair.maxLevel || level == 0)
            {
                upgrade_coin = chair.buildPrice[0] * (int)Mathf.Pow(chair.buildPrice[1], level);
                build_text.text = $"建造";
            }
            else
            {
                upgrade_coin = chair.upgradePrice[0] * (int)Mathf.Pow(chair.upgradePrice[1], curLevel - 1);
                build_text.text = $"升级";
            }
            Coin_text.text = upgrade_coin.ToString();
        }

        private void ClickUpgradeBtn()
        {
            Chair_Property chair = BuildMgr.GetChairByIdAndLevel(id, level);
            if (curLevel == chair.maxLevel || level == 0)
            {
                if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, upgrade_coin))
                {
                    curLevel = 1;
                    level++;
                    mPanelData.index = GetPosById();
                    builds[index].Level = level;
                    BuildMgr.UpgradeComBuild(id, mPanelData.index);
                    chair = BuildMgr.GetChairByIdAndLevel(id, level);
                    item.SetItemLevelState(chair);
                    UpdateImage(chair);
                    UpdateUI(chair);
                    return;
                }
            }
            else if (ItemPropsManager.Intance.CoseItem((int)CurrencyType.Coin, upgrade_coin))
            {
                curLevel++;
            }
            SaveData(chair);
            ShowCoin(chair);
            ShowUnlockText(chair);
            BtnInteraction(chair);
        }

        private void BtnInteraction(Chair_Property chair)
        {
            build_btn.interactable = curLevel < chair.maxLevel || chair.level < 4;
            maxObj.SetActive(!build_btn.interactable);

        }

        private void ShowImage(int index, Sprite sp)
        {
            if (index == this.index)
            {
                img = toggles[index].targetGraphic.GetComponent<Image>();
                toggles[index].isOn = false;
                toggles[index].isOn = true;
                Icon_img.sprite = sp;
            }
        }

        private void ShowImg()
        {
            if (toggles[index] == null) return;
            toggles[index].isOn = true;
            img = toggles[index].targetGraphic.GetComponent<Image>();
            Icon_img.sprite = img.sprite;
        }

        private void ShowInView()
        {
            Vector2 pos = InfinityScroll.content.anchoredPosition;
            float y = InfinityScroll.itemSize.y;
            float y2 = InfinityScroll.space.y;
            int top = InfinityScroll.margin.top;
            pos.y = (y + y2 + top) * (index / 4);
            InfinityScroll.content.anchoredPosition = pos;
        }

        private void UpdateImage(Chair_Property chair)
        {
            img.sprite = AssetMgr.Instance.LoadTexture("BuildTex", chair.icon);
            Icon_img.sprite = img.sprite;
        }

        private void SetCurLevel(Chair_Property chair)
        {
            curLevels = BuildUpgradeMgr.Instance.GetCurAreaItem(chair.type, builds.Count);
            curLevel = curLevels[index];
        }

        private void SaveData(Chair_Property chair)
        {
            curLevels[index] = curLevel;
            BuildUpgradeMgr.Instance.SaveAreaData(chair.type, curLevels);
        }

    }
}
