using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class TTGyModel : MonoBehaviour
    {
        private void Awake()
        {
            //ShowPanel<MainPanel>();
        }

        //public void ShowPanel<T>(params Type[] conditonPanelType) where T : UIBase
        //{
        //    Debug.LogError("Type");
        //    //if (UIPanelManager.Instance.IsCanShowUI(typeof(T), uiDataBase, conditonPanelType))
        //    //{
        //    //    UIPanelManager.Instance.ShowUI<T>(uiDataBase, delayTime);
        //    //}
        //}

        void OnMouseUpAsButton()
        {
            //Debug.LogError("OnClick");
            //GuideMgr.Instance.SetHighlight(transform);
        }

        int onClickNum = 0;

        void OnMouseDown()
        {
            onClickNum++;
            if (onClickNum > 4)
            {
                onClickNum = 0;
                GuideMgr.Instance.HideGuide();
            }
            else
            {
                GuideMgr.Instance.SetHighlight(transform, 1);
            }
            
        }
    }
}