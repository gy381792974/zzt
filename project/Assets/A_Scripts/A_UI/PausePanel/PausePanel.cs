using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
namespace EazyGF
{
    public class PausePanelData : UIDataBase
    {
        public PausePanelData()
        {

        }
    }

    public partial class PausePanel : UIBase
    {
        protected override void OnInit()
        {
            GoOn_btn.onClick.AddListener(GoOn_btnClick);
            Reset_btn.onClick.AddListener(Reset_btnClick);
            ZhuyeBtn_btn.onClick.AddListener(ZhuyeBtn_btnClick);

            MusicBtn_btn.onClick.AddListener(MusicBtn_btnClick);
            CloseMusic_btn.onClick.AddListener(CloseMusic_btnClick);

            SoundBtn_btn.onClick.AddListener(SoundBtn_btnClick);
            CloseSound_btn.onClick.AddListener(CloseSound_btnClick);
            
        }

        bool isOpenMusic = false;

        bool isOpenSound = true;

        private void MusicBtn_btnClick()
        {
            isOpenMusic = true;

            UpdateMusic();
        }

        private void CloseMusic_btnClick()
        {
            isOpenMusic = false;

            UpdateMusic();
        }

        private void SoundBtn_btnClick()
        {
            isOpenSound = true;

            UpdateSound();
        }

        private void CloseSound_btnClick()
        {
            isOpenSound = false;

            UpdateSound();
        }

        private void UpdateSound()
        {
            MusicMgr.Instance.IsCloseEff = isOpenSound;
            SoundBtn_btn.gameObject.SetActive(!isOpenSound);
            CloseSound_btn.gameObject.SetActive(isOpenSound);
            Transform trans = SoundBtn_btn.gameObject.activeSelf ? SoundBtn_btn.transform : CloseSound_btn.transform;
            BtnClickAnimation(trans);
        }

        private void UpdateMusic()
        {
            MusicMgr.Instance.IsCloseBG = isOpenMusic;
            MusicBtn_btn.gameObject.SetActive(!isOpenMusic);
            CloseMusic_btn.gameObject.SetActive(isOpenMusic);
            Transform trans = MusicBtn_btn.gameObject.activeSelf ? MusicBtn_btn.transform : CloseMusic_btn.transform;
            BtnClickAnimation(trans);
        }

        private void ZhuyeBtn_btnClick()
        {
            BtnClickAnimation(ZhuyeBtn_btn.transform);

            UIMgr.HideUI<PausePanel>();
            UIMgr.HideUI<CubeMainPanel>();
        }

        private void Reset_btnClick()
        {
            //SceneManager.LoadScene("Main");
            BtnClickAnimation(Reset_btn.transform);

            CubeGameMgr.Instance.isPause = false;
            UIMgr.HideUI<PausePanel>();
            CubeMainPanel cubeMainPanel = UIMgr.GetUI<CubeMainPanel>();
            cubeMainPanel.OnReset();

            CubeGameMgr.Instance.ClearSelected();
        }

        private void GoOn_btnClick()
        {
            BtnClickAnimation(GoOn_btn.transform);

            CubeGameMgr.Instance.isPause = false;
            UIMgr.HideUI<PausePanel>();
        }

        protected override void OnShow(UIDataBase pausepanelData = null)
        {
            if (pausepanelData != null)
            {
                mPanelData = pausepanelData as PausePanelData;
            }
            CubeGameMgr.Instance.isPause = true;
            isOpenMusic = MusicMgr.Instance.IsCloseBG;
            isOpenSound = MusicMgr.Instance.IsCloseEff;
            UpdateMusic();
            UpdateSound();
        }

        protected override void OnHide()
        {
            MusicMgr.Instance.SavePlayerSetting();
        }
    }
}
