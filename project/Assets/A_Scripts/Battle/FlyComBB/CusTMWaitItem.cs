using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class CusTMWaitItem : FlyBBBase
    {
        public Image waitProImg;

        int waitTime = 2;

        bool isPlayAni = false;
        float timer = 0;


        protected override void UpateContent()
        {
            waitTime = data.num;

            waitProImg.fillAmount = 0;

            isPlayAni = true;

            timer = 0;
        }

        public override void FlyBBUpdate()
        {
            base.FlyBBUpdate();

            if (isPlayAni)
            {
                timer += Time.deltaTime;

                waitProImg.fillAmount = timer / waitTime;

                if (timer >= waitTime)
                {
                    isPlayAni = false;

                    EventManager.Instance.TriggerEvent(EventKey.RecycleCBBData, this);
                }
            }
        }
    }

}