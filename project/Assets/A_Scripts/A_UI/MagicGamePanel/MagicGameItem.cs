using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class MagicGameItem : MonoBehaviour
    {
        public Button backBtn;

        public Transform frontBg;
        public Image frontIcon;

        public Action<int, int> act;
        int index;
        int value;

        private void Start()
        {
            backBtn.onClick.AddListener(OnClick);
        }

        public void InitData(int index, int value, Sprite sprite)
        {
            this.index = index;

            backBtn.transform.localEulerAngles = Vector3.zero;
            backBtn.gameObject.SetActive(true);

            frontBg.transform.localEulerAngles = new Vector3(0, -90, 0);
            frontIcon.sprite = sprite;

            this.value = value;
        }

        public void BindData(int value, Sprite sprite)
        {
            frontIcon.sprite = sprite;
            this.value = value;
        }

        public void OnClick()
        {
            UIMgr.ShowPanel<MaskPanel>();

            MusicMgr.Instance.PlayMusicEff("d_guests_m_select");
            Sequence sequence = DOTween.Sequence();
            Tween tween1 = backBtn.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.5f).SetEase(Ease.Linear);
            sequence.Append(tween1);

            Tween tween2 = frontBg.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.Linear);
            sequence.Append(tween2);

            sequence.OnComplete(() =>
            {
                backBtn.gameObject.SetActive(false);

                act(index, value);
            });
        }

        internal void SetOnClickHandle(Action<int, int> fun)
        {
            act = fun;
        }
    }
}
