using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace EazyGF
{

    public class TipPanelData : UIDataBase
    {
        public TipPanelData()
        {

        }
    }

    public partial class TipPanel : UIBase
    {
        CanvasGroup canvas;
        Coroutine coroutine;
        Tween tween;
        Vector2 initPos;
        protected override void OnInit()
        {
            canvas = this.gameObject.AddComponent<CanvasGroup>();
            initPos = transform.position;
        }

        protected override void OnShow(UIDataBase tippanelData = null)
        {
            if (tippanelData != null)
            {
                mPanelData = tippanelData as TipPanelData;
            }
        }

        protected override void OnHide()
        {

        }

        public void PlayAnimation(int[] arr, params object[] objs)
        {
            InitState();
            string txt = LanguageMgr.GetTranstion(arr, objs);
            coroutine = StartCoroutine(PlayAnim(txt));
        }

        public void PlayAnimation(string str)
        {
            InitState();
            coroutine = StartCoroutine(PlayAnim(str));
        }

        IEnumerator PlayAnim(string str)
        {
            tip_text.text = str;
            yield return new WaitForSeconds(2f);
            canvas.DOFade(0, 0.8f);
            tween = transform.DOMoveY(transform.position.y + 2f, 0.8f);
            tween.onComplete = () => { UIMgr.HideUI<TipPanel>(); };
        }

        private void InitState()
        {
            gameObject.SetActive(true);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            tween.Kill();
            canvas.alpha = 0;
            transform.position = initPos;
            tween = canvas.DOFade(1, 0.8f);
        }
    }
}
