using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class VictoryPanelData : UIDataBase
    {
        public int startNum;
        public int level;

        public VictoryPanelData(int startNum, int level)
        {
            this.startNum = startNum;
            this.level = level;
        }
    }

    public partial class VictoryPanel : UIBase
    {
        SkeletonGraphic sg;
        CubeMainPanel cubeMainPanel;
        protected override void OnInit()
        {
            enter_btn.onClick.AddListener(Enter_btnClick);
            sg = GetComponentInChildren<SkeletonGraphic>();
        }

        private void Enter_btnClick()
        {
            MusicMgr.Instance.PlayMusicEff("c_star_collect");
            BtnClickAnimation(enter_btn.transform);
            ItemPropsManager.Intance.AddItem((int)CurrencyType.Star, mPanelData.startNum);
            UIMgr.HideUI<VictoryPanel>();
            cubeMainPanel = UIMgr.GetUI<CubeMainPanel>();
            cubeMainPanel.OnReset();

            CubeGameMgr.Instance.ClearSelected();
        }

        protected override void OnShow(UIDataBase victorypanelData = null)
        {
            if (victorypanelData != null)
            {
                mPanelData = victorypanelData as VictoryPanelData;
            }

            ItemNum_text.text = "X" + mPanelData.startNum;

            Level_text.text = "LEVEL:" + mPanelData.level;
            MusicMgr.Instance.PlayMusicEff("c_win_win");
            PlayAnimation();
        }

        protected override void OnHide()
        {

        }

        private void PlayAnimation()
        {
            if (sg != null)
            {
                sg.AnimationState.SetEmptyAnimation(0, 0);  //清空animation trackIndex=0;
                sg.AnimationState.SetAnimation(0, "animation", false);
                sg.AnimationState.AddAnimation(0, "again", true, 0);
            }
        }
    }

}
