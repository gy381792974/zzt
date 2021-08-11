using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class BubblePanelData : UIDataBase
    {
        public BubblePanelData()
        {

        }
    }
    /// <summary>
    /// 气泡管理器
    /// </summary>
    public partial class BubblePanel : UIBase
    {
        public Bubble bubble;
        List<Bubble> bubbles = new List<Bubble>();
        protected override void OnInit()
        {
            EventManager.Instance.RegisterEvent(EventKey.SendBubbleData, ShowBubble);
            EventManager.Instance.RegisterEvent(EventKey.CanceLoopBubble, CustomerLeaveArea);
        }

        protected override void OnShow(UIDataBase bubblepanelData = null)
        {
            if (bubblepanelData != null)
            {
                mPanelData = bubblepanelData as BubblePanelData;
            }
        }

        protected override void OnHide()
        {

        }
        private void ShowBubble(object arg)
        {
            BubbleData data = (BubbleData)arg;
            Bubble bubble = GetBubble();
            bubble.Init(data.cs);
            if (data.type == 0)
            {
                bubble.ShowUnhappyText(data.txt);
            }
            else
            {
                bubble.ShowHappyText(data.txt);
            }
        }

        private void CustomerLeaveArea(object arg)
        {
            Transform cnTrans = (Transform)arg;
            Bubble bubble = BubbleByHashCode(cnTrans.GetHashCode());
            bubble.isUse = false;
            bubble.gameObject.SetActive(false);
        }

        private Bubble BubbleByHashCode(int hashValue)
        {
            for (int i = 0; i < bubbles.Count; i++)
            {
                if (bubbles[i].Trans.GetHashCode() == hashValue)
                {
                    return bubbles[i];
                }
            }
            return null;
        }

        public Bubble GetBubble()
        {
            for (int i = 0; i < bubbles.Count; i++)
            {
                if (!bubbles[i].isUse)
                {
                    bubbles[i].gameObject.SetActive(true);
                    bubbles[i].isUse = true;
                    return bubbles[i];
                }
            }

            Bubble bb = AssetMgr.Instance.LoadGameobj("bubble").GetComponent<Bubble>();
            bb.transform.localScale = Vector3.one;
            bubbles.Add(bb);

            return bb;
        }
      


    }
}
