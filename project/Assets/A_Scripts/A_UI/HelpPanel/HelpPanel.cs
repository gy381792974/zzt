using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class HelpPanelData : UIDataBase
    {
        public HelpPanelData()
        {

        }
    }

    public partial class HelpPanel : UIBase
    {
        SkeletonGraphic sg;
        Button lastHighLightBtn = null;
        Coroutine coroutine;
        protected override void OnInit()
        {
            sg = spineAnim_obj.GetComponent<SkeletonGraphic>();
            HelpCloseBtn_btn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<HelpPanel>();
            });
            play_btn.onClick.AddListener(() => { PlayAnim(play_btn, "play"); PlayMusicEff(PlayMusic()); });
            help_btn.onClick.AddListener(() => { PlayAnim(help_btn, "help"); PlayMusicEff(OtherMusic("c_item_magnifier", 0.5f)); });
            magic_btn.onClick.AddListener(() => { PlayAnim(magic_btn, "magic"); PlayMusicEff(OtherMusic("c_item_lodestone", 0.2f)); });
            reset_btn.onClick.AddListener(() => { PlayAnim(reset_btn, "reset"); PlayMusicEff(OtherMusic("c_item_renumber", 0.2f)); });
        }

        protected override void OnShow(UIDataBase helppanelData = null)
        {
            if (helppanelData != null)
            {
                mPanelData = helppanelData as HelpPanelData;
            }
            CubeGameMgr.Instance.isPause = true;
            PlayAnim(play_btn, "play");
            PlayMusicEff(PlayMusic());
        }

        protected override void OnHide()
        {
            sg.AnimationState.SetEmptyAnimation(0, 0);
            lastBtnDark();
            CubeGameMgr.Instance.isPause = false;
        }
        //按钮变亮并播放相应的动画
        private void PlayAnim(Button btn, string animName)
        {
            BtnClickAnimation(btn.transform);
            lastBtnDark();
            ColorHighLight(btn, 1);
            sg.timeScale = 0.8f;
            sg.AnimationState.SetAnimation(0, animName, true);
        }
        private void ColorHighLight(Button btn, float a)
        {
            btn.transform.GetChild(0).gameObject.SetActive(true);
            btn.transform.GetChild(1).gameObject.SetActive(false);
            Image image = btn.GetComponent<Image>();
            Color color = image.color;
            color.a = a;
            image.color = color;
            lastHighLightBtn = btn;
        }
        //上一个高亮的按钮变暗
        private void lastBtnDark()
        {
            if (lastHighLightBtn != null)
            {
                ColorHighLight(lastHighLightBtn, 0);
                lastHighLightBtn.transform.GetChild(0).gameObject.SetActive(false);
                lastHighLightBtn.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        private void PlayMusicEff(IEnumerator routine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            if (coroutine == null)
                coroutine = StartCoroutine(routine);
        }
        //play按钮的音效
        IEnumerator PlayMusic(bool loop = true)
        {
            yield return new WaitForSeconds(0.6f / sg.timeScale);
            MusicMgr.Instance.PlayMusicEff(CubeGameMgr.Instance.GetRandomClearEff());
            yield return new WaitForSeconds(0.9f / sg.timeScale);
            MusicMgr.Instance.PlayMusicEff(CubeGameMgr.Instance.GetRandomClearEff());
            yield return new WaitForSeconds(0.9f / sg.timeScale);
            MusicMgr.Instance.PlayMusicEff(CubeGameMgr.Instance.GetRandomClearEff());
            if (loop)
            {
                sg.AnimationState.Complete += (x) => { PlayMusicEff(PlayMusic(loop)); };
            }
        }
        //其他按钮的音效
        IEnumerator OtherMusic(string name, float interval, bool loop = true)
        {
            yield return new WaitForSeconds(interval / sg.timeScale);
            MusicMgr.Instance.PlayMusicEff(name);
            if (loop)
            {
                sg.AnimationState.Complete += (x) => { PlayMusicEff(OtherMusic(name, interval, loop)); };
            }
        }


    }
}
