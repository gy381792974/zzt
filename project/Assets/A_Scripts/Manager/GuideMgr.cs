
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public enum GuideType
    {
        unLockBuild
    }

    public class GuideModel
    {
       public int id;
       public int stepNum;
    }

    public class GuideMgr : SingleClass<GuideMgr>
    {
        GuideCtrl guideCtrl;

        GuidePanel guidePanel;

        UIPanelManager uIPanelManager;

        //目标锚点
        public Transform targetTf;

        bool isdebug = false;

        public bool isGuide = true;

        public List<GuideStep_Property> curGuides;

        int stepCounter = 0;

        public bool isFirstEnterGame = false; 

        
        public void FirstEnterGameGuide()
        {
            if (isFirstEnterGame)
            {
                TargetGuide(0);
            }
        }

        public void TargetGuide(int guideId)
        {
            if (isGuide)
            {
                curGuides = GetGuideData(guideId);

                stepCounter = 0;

                SetGuideData();
            }
        }

        //下一步 也包括了上一步处理结束
        public void NextStep()
        {
            switch (curGuides[stepCounter].ShowStep)
            {
                case 1:

                    if (targetTf != null)
                    {
                        targetTf.GetComponent<Button>().onClick.RemoveListener(NextStep);
                    }

                    break;
                default:
                    break;
            }

            stepCounter++;

            SetGuideData();
        }


        public void SetGuideData()
        {
            if (stepCounter < curGuides.Count)
            {
                GuideStep_Property data = curGuides[stepCounter];

                GameObject la = GameObject.Find(data.LightAnchor);

                SetHighlight(la.transform, data.AnchorType);
            }
            else
            {
                targetTf = null;
                HideGuide();
            }
        }

        // type 0 2d  type1 3d
        public void SetHighlight(Transform tf, int type = 0)
        {

            Vector3 pos = tf.position;

            isGuide = true;
            Vector2 vector2 = new Vector2(pos.x, pos.y);

            if (type == 1)
            {
                vector2 = UICommonUtil.Instance.GetUIPosByWorldPos(pos);
            }

            if (isdebug)
            {
                Debug.LogError($"{vector2.x} {vector2.y}");
            }

            UIMgr.ShowPanel<GuidePanel>();

            if (guidePanel == null)
            {
                guidePanel = UIMgr.GetUI<GuidePanel>();
                guideCtrl = guidePanel.transform.GetChild(0).GetComponent<GuideCtrl>();
            }

            if (type == 0)
            {
                guidePanel.maskImg.transform.position = vector2;
            }
            else if (type == 1)
            {
                guidePanel.maskImg.transform.localPosition = vector2;
            }

            guideCtrl.Play(guidePanel.maskImg);

            if (curGuides[stepCounter].ShowStep == 1)
            {
                targetTf = tf;
                targetTf.GetComponent<Button>().onClick.AddListener(NextStep);
            }
        }

        public void HideGuide()
        {
            isGuide = false;
            UIMgr.HideUI<GuidePanel>();
        }

        private static List<GuideStep_Property> GetGuideData(int id)
        {
            GuideStep_Property[] dataArray = GuideStep_Data.DataArray;

            List<GuideStep_Property> datas = new List<GuideStep_Property>();

            for (int i = 0; i < dataArray.Length; i++)
            {
                if (dataArray[i].ID == id)
                {
                    datas.Add(dataArray[i]);
                }
            }

            return datas;
        }
    }
}