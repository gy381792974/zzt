using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class BuildFlyTxtItem : FlyBBBase
    {
        public Text txt;
        public CanvasGroup canvasGroup;
        public Transform flyTf;

        Vector3 startPos = Vector3.zero;
        bool isInitPos = false;

        Sequence se;

        protected override void UpateContent()
        {
            txt.text = "+" + data.num.ToString();

            BuildCollectMgr.Instance.GetCoin(data.id);

            flyTf.DOPause();
            canvasGroup.DOPause();

            canvasGroup.alpha = 1;

            if (!isInitPos)
            {
                isInitPos = true;
                startPos = flyTf.localPosition;
            }
            else
            {
                flyTf.localPosition = startPos;
            }

            flyTf.DOLocalMoveY(20, 1f);
            canvasGroup.DOFade(0, 1).OnComplete(AniPlayCom);
        }

        public void AniPlayCom()
        {
            //Debug.LogError("OnComplete");

            //EventManager.Instance.TriggerEvent(EventKey.RecycleCBBData, this);
        }

    }

  
}