using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
namespace EazyGF
{

    public class StartPanel2Data : UIDataBase
    {
        public StartPanel2Data()
        {

        }
    }

    public partial class StartPanel2 : UIBase
    {
        Tween tw;
        private void Start()
        {
            OnInit();
        }
        protected override void OnInit()
        {
            InitStartPanel();
            StartProgress();
        }

        protected override void OnShow(UIDataBase startpanel2Data = null)
        {
            if (startpanel2Data != null)
            {
                mPanelData = startpanel2Data as StartPanel2Data;
            }
        }

        protected override void OnHide()
        {

        }

        /// <summary>
        /// 初始化StartPanel
        /// </summary>
        private void InitStartPanel()
        {
            progress_img.fillAmount = 0;
            Start_btn.onClick.AddListener(() => { tw.Kill(); StartCoroutine(LoadScene()); });
        }

        private void StartProgress()
        {
            StartCoroutine(Progress());
        }

        /// <summary>
        /// 假 进度条
        /// </summary>
        /// <returns></returns>
        IEnumerator Progress()
        {
            float timer = 0;
            while (progress_img.fillAmount < 1)
            {
                timer += 0.005f;
                progress_img.fillAmount = Mathf.Lerp(progress_img.fillAmount, 1, timer);
                progress_text.text = string.Format("{0:N2}%", (progress_img.fillAmount / 1) * 100);
                yield return new WaitForSeconds(0.1f);
            }
            progressBG_trans.gameObject.AddComponent<CanvasGroup>().DOFade(0, 0.6f);
            yield return new WaitForSeconds(1f);
            CanvasGroup cg = Start_btn.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0.5f;
            Start_btn.gameObject.SetActive(true);
            tw = cg.DOFade(1, 3f).SetLoops(-1, LoopType.Yoyo);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadScene()
        {
            yield return SceneManager.LoadSceneAsync("Main");
        }
    }
}
