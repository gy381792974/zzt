using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class MainPanelData : UIDataBase
    {
        public MainPanelData()
        {

        }
    }

    public partial class MainPanel : UIBase
    {
        SkeletonGraphic sg;
        protected override void OnInit()
        {
            Setting_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<SettingPanel>();
            });

            //图鉴面板待完成
            Staff_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<StaffPanel>();
            });

            customer_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<RoleMapPanel>();
            });

            Customs_btn.onClick.AddListener(() =>
            {
                MusicMgr.Instance.PlayMusicEff("d_btn_start");
                UIMgr.ShowPanel<CubeMainPanel>();
            });


            UpdateData(null);
            RegisterEvent();

            sg = Customs_btn.GetComponentInChildren<SkeletonGraphic>();
        }

        protected override void OnShow(UIDataBase mainpanelData = null)
        {
            if (mainpanelData != null)
            {
                mPanelData = mainpanelData as MainPanelData;
            }
            ShowCurLevel();
        }

        private void RegisterEvent()
        {
            EventManager.Instance.RegisterEvent(EventKey.ItemNumUpdate, UpdateData);
        }

        private void RemoveEvent()
        {
            EventManager.Instance.RemoveListening(EventKey.ItemNumUpdate, UpdateData);
        }

        private void UpdateData(object obj)
        {
            Goldnum_text.text = ItemPropsManager.Intance.GetItemNum(1).ToString();
            Starnum_text.text = ItemPropsManager.Intance.GetItemNum(2).ToString();
            bottle_text.text = ItemPropsManager.Intance.GetItemNum(3).ToString();
        }

        protected override void OnHide()
        {
            RemoveEvent();
        }

        /// <summary>
        /// 使发光按钮Customs_btn 停止发光
        /// </summary>
        public void CloseLevelAnmation()
        {
            sg.AnimationState.SetEmptyAnimation(0, 0);
        }


        /// <summary>
        /// 播放按钮Cutoms_btn动画
        /// </summary>
        /// <param name="loop">是否循环</param>
        public void PlayAnimation(bool loop)
        {
            sg.AnimationState.SetAnimation(0, "animation", loop);

        }

        public void ShowCurLevel()
        {
            int level = CubeGameMgr.Instance.curLevel;
            Custons_text.text = $"{level}";
        }
    }
}
