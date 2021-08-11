using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class ComRewardPanelData : UIDataBase
    {
        public List<AwardData> awardData;


        public ComRewardPanelData(List<AwardData> awardDatas)
        {
            this.awardData = awardDatas;
        }
    }

    public partial class ComRewardPanel : UIBase
    {

        protected override void OnInit()
        {
            getBtn.onClick.AddListener(() =>
            {
                MusicMgr.Instance.PlayMusicEff("g_collect_win_push");
             
                UIMgr.HideUI<ComRewardPanel>();
            });
        }

        protected override void OnShow(UIDataBase comrewardpanelData = null)
        {
            if (comrewardpanelData != null)
            {
                mPanelData = comrewardpanelData as ComRewardPanelData;
            }

            for (int i = 0; i < awardGrids.Count; i++)
            {
                if (i < mPanelData.awardData.Count)
                {
                    awardGrids[i].BuildData(mPanelData.awardData[i]);
                }
                else
                {
                    awardGrids[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < mPanelData.awardData.Count; i++)
            {
                ItemPropsManager.Intance.AddItem(mPanelData.awardData[i].id, mPanelData.awardData[i].num);
            }
        }

        protected override void OnHide()
        {

        }
    }
}
