using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class CubeFailPanelData : UIDataBase
    {
        public CubeFailPanelData()
        {

        }
    }

    public partial class CubeFailPanel : UIBase
    {
        int coinNum = 0;
        int curHaveCoin = 0;
        CubeMainPanel cubeMainPanel;
        protected override void OnInit()
        {
            cubeMainPanel = UIMgr.GetUI<CubeMainPanel>();
            restart_btn.onClick.AddListener(ReStart);
            revival_btn.onClick.AddListener(Revival);
            revival_text.text = LanguageMgr.GetTranstion(1,10);
            close_btn.onClick.AddListener(Close);
        }

        private void Revival_BtnState()
        {
            if (curHaveCoin < coinNum)
            {
                revival_btn.interactable = false;
            }
            else
            {
                revival_btn.interactable = true;
            }
        }

        protected override void OnShow(UIDataBase cubefailpanelData = null)
        {
            if (cubefailpanelData != null)
            {
                mPanelData = cubefailpanelData as CubeFailPanelData;
            }
            MusicMgr.Instance.PlayMusicEff("c_win_lose");
            Revival_BtnState();
            FailText();
        }

        protected override void OnHide()
        {

        }

        private void ReStart()
        {
            //CubeGameMgr.Instance.isPause = false;
            if (cubeMainPanel != null)
            {
                cubeMainPanel.OnReset();
                CubeGameMgr.Instance.ClearSelected();
            }
            BtnClickAnimation(restart_btn.transform);
            UIMgr.HideUI<CubeFailPanel>();
        }

        private void Close()
        {
            UIMgr.HideUI<CubeFailPanel>();
            UIMgr.HideUI<CubeMainPanel>();
        }

        private void Revival()
        {
            BtnClickAnimation(revival_btn.transform);
            ItemPropsManager.Intance.AddItem((int)CurrencyType.Coin, -coinNum);
            cubeMainPanel.Revival();
            UIMgr.HideUI<CubeFailPanel>();
        }

        private void FailText()
        {
            coinNum = 1000;
            Text_text.text = LanguageMgr.GetTranstion(1,9,coinNum);//string.Format("You can pay {0} gold prices to be resurrected", coinNum);
            curHaveCoin = ItemPropsManager.Intance.GetItemNum((int)CurrencyType.Coin);
            coin_text.text = string.Format("Have:{0}", curHaveCoin);
        }
    }
}
