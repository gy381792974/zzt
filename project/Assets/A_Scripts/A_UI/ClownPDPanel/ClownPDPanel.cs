using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class ClownPDPanelData : UIDataBase
    {
        public ClownPDPanelData()
        {

        }
    }

    public partial class ClownPDPanel : UIBase
    {
        Spine.Unity.SkeletonGraphic sg;
        protected override void OnInit()
        {
            sg = spineAnim_Obj.GetComponent<Spine.Unity.SkeletonGraphic>();
        }

        protected override void OnShow(UIDataBase clownpdpanelData = null)
        {
            if (clownpdpanelData != null)
            {
                mPanelData = clownpdpanelData as ClownPDPanelData;
            }
            PlayAnim();
        }

        protected override void OnHide()
        {

        }
        private void PlayAnim()
        {
            sg.AnimationState.SetAnimation(0, "pd", false);
            sg.AnimationState.Complete += (x) => { UIMgr.HideUI<ClownPDPanel>(); };
        }
    }
}
