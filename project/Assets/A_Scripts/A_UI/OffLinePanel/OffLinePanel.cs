using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class OffLinePanelData : UIDataBase
    {
        public int offline;
        public OffLinePanelData(int offline)
        {
            this.offline = offline;
        }
    }

    public partial class OffLinePanel : UIBase
    {
        [SerializeField] int second = 5;//多少秒获得的离线收益
        int coinCount;//可以获得的金币数量
        protected override void OnInit()
        {
            obtain_Btn.onClick.AddListener(GetCurrentry);
        }

        protected override void OnShow(UIDataBase offlinepanelData = null)
        {
            if (offlinepanelData != null)
            {
                mPanelData = offlinepanelData as OffLinePanelData;
            }
            GetOfflineAward(mPanelData.offline);
            MusicMgr.Instance.PlayMusicEff("g_win_push");
        }

        protected override void OnHide()
        {

        }

        private void GetCurrentry()
        {
            MusicMgr.Instance.PlayMusicEff("d_money_collect");
            BtnClickAnimation(obtain_Btn.transform);
            ItemPropsManager.Intance.AddItem(1, coinCount);
            UIMgr.HideUI<OffLinePanel>();
            PlayerDataMgr.SaveData(true);
        }

        /// <summary>
        /// 获取离线奖励/奖励的规则
        /// </summary>
        /// <param name="seconds">离线的时间（秒）</param>
        public void GetOfflineAward(int seconds)
        {
            coinCount = seconds / second * SumAllAdornExtraGlod();
            coinCount_text.text = coinCount.ToString();
        }

        private int SumAllAdornExtraGlod()
        {
            int sum = 0;
            List<int> StallIds = BuildMgr.GetAllBuildIdByType(3);
            for (int i = 0; i < StallIds.Count; i++)
            {
                int level = BuildMgr.GetUserBuildLevelById(StallIds[i]);
                Adorn_Property adorn = BuildMgr.GetAdorn_Property(Adorn_Data.DataArray[i].ID, level);
                sum += adorn.ExtraGold;
            }
            return sum;
        }

    }
}
