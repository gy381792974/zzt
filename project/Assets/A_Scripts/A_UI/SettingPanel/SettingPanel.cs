using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    [System.Serializable]
    public class SettingVoiceData
    {
        public bool isPauseBGM;
        public bool isPauseEff;
    }

    public class SettingPanelData : UIDataBase
    {

        public SettingPanelData()
        {

        }
    }

    public partial class SettingPanel : UIBase
    {
        protected override void OnInit()
        {
            MusicOFF_btn.Hide();
            SoundON_btn.Hide();

            SettingClose_btn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<SettingPanel>();
            });

            #region 音量控制
            MusicON_btn.onClick.AddListener(() =>
            {
                MusicON_btn.Hide();
                MusicOFF_btn.Show();
                BGM_On(true);
            });

            MusicOFF_btn.onClick.AddListener(() =>
            {
                MusicOFF_btn.Hide();
                MusicON_btn.Show();
                BGM_On(false);
            });
            #endregion


            #region 音效控制
            SoundON_btn.onClick.AddListener(() =>
            {
                SoundON_btn.Hide();
                SoundOFF_btn.Show();
                ME_On(true);
            });

            SoundOFF_btn.onClick.AddListener(() =>
            {
                SoundON_btn.Show();
                SoundOFF_btn.Hide();
                ME_On(false);
            });
            #endregion
        }

        protected override void OnShow(UIDataBase settingpanelData = null)
        {
            if (settingpanelData != null)
            {
                mPanelData = settingpanelData as SettingPanelData;
            }

            if (MusicMgr.Instance.IsCloseBG)
            {
                MusicON_btn.Hide();
                MusicOFF_btn.Show();
            }
            else
            {
                MusicON_btn.Show();
                MusicOFF_btn.Hide();
            }
            if (MusicMgr.Instance.IsCloseEff)
            {
                SoundON_btn.Hide();
                SoundOFF_btn.Show();
            }
            else
            {
                SoundON_btn.Show();
                SoundOFF_btn.Hide();
            }
        }

        protected override void OnHide()
        {
            MusicMgr.Instance.SavePlayerSetting();
        }
        //背景音乐
        private void BGM_On(bool isPause)
        {
            MusicMgr.Instance.IsCloseBG = isPause;
        }
        //音效
        private void ME_On(bool isPause)
        {
            MusicMgr.Instance.IsCloseEff = isPause;
        }
    }
}
