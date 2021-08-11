using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class StarNotEnoughData : UIDataBase
    {
        public StarNotEnoughData()
        {

        }
    }

    public partial class StarNotEnough : UIBase
    {
        protected override void OnInit()
        {
            Close_Btn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<StarNotEnough>();
            });

            Play_btn.onClick.AddListener(() =>
            {
                BtnClickAnimation(Play_btn.transform);
                UIMgr.HideUI<StarNotEnough>();
                UIMgr.HideUI<ShopNewPanel>();
                UIMgr.GetUI<MainPanel>().PlayAnimation(false);

            });
        }
        protected override void OnShow(UIDataBase starnotenoughData = null)
        {
            if (starnotenoughData != null)
            {
                mPanelData = starnotenoughData as StarNotEnoughData;
            }
            NotEnough_text.text = LanguageMgr.GetTranstion(1, 14);
            //MusicMgr.Instance.PlayMusicEff("g_win_push");
        }

        protected override void OnHide()
        {

        }
    }
}
