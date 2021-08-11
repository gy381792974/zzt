using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class RoleMapPanelData : UIDataBase
    {
        public RoleMapPanelData()
        {

        }
    }

    public partial class RoleMapPanel : UIBase
    {
        [SerializeField] private ScrollViewInfinity scrollViewInfinity;
        [SerializeField] Sprite[] sprites;
        Image[] Imgs;
        List<RoleMapData> dataList = new List<RoleMapData>();

        private List<RoleMapEle> eles = new List<RoleMapEle>();

        protected override void OnInit()
        {
            scrollViewInfinity.onItemRender.AddListener(OnUpdateItem);

            Close_btn.onClick.AddListener(() => { UIMgr.HideUI<RoleMapPanel>(); });
            Imgs = ImgGrid_trans.GetComponentsInChildren<Image>();
        }

        private void OnUpdateItem(int arg0, Transform arg1)
        {
            RoleMapEle ele = arg1.GetComponent<RoleMapEle>();

            int index = arg0 * 3;
            int count = dataList.Count - index;

            if (count > 3)
            {
                count = 3;
            }

            List<RoleMapData> roleMapDatas = new List<RoleMapData>();

            for (int i = index; i < (index + count); i++)
            {
                roleMapDatas.Add(dataList[i]);
            }

            ele.BindData(roleMapDatas);
        }

        protected override void OnShow(UIDataBase rolemappanelData = null)
        {
            if (rolemappanelData != null)
            {
                mPanelData = rolemappanelData as RoleMapPanelData;
            }

            dataList = CustomerLogic.GetAllLockData();

            int unCusNum = CustomerLogic.GetUnlockCusNum();
            ShowImgs(unCusNum, dataList.Count);

            scrollViewInfinity.InitScrollView(Mathf.CeilToInt((float)dataList.Count / 3));
            //roleNum_text.text = $"{unCusNum} / {dataList.Count}";
        }
        private void ShowImgs(int unlockNum, int allNum)
        {
            char[] showImg = $"({unlockNum}/{allNum})".ToCharArray();
            Sprite sprite = null;
            for (int i = 0; i < showImg.Length; i++)
            {
                ImgGrid_trans.GetChild(i).gameObject.SetActive(true);
                switch (showImg[i])
                {
                    case '(':
                        sprite = sprites[0];
                        break;
                    case '0':
                        sprite = sprites[1];
                        break;
                    case '1':
                        sprite = sprites[2];
                        break;
                    case '2':
                        sprite = sprites[3];
                        break;
                    case '3':
                        sprite = sprites[4];
                        break;
                    case '4':
                        sprite = sprites[5];
                        break;
                    case '5':
                        sprite = sprites[6];
                        break;
                    case '6':
                        sprite = sprites[7];
                        break;
                    case '7':
                        sprite = sprites[8];
                        break;
                    case '8':
                        sprite = sprites[9];
                        break;
                    case '9':
                        sprite = sprites[10];
                        break;
                    case '/':
                        sprite = sprites[11];
                        break;
                    case ')':
                        sprite = sprites[12];
                        break;
                }
                Imgs[i].sprite = sprite;
                Imgs[i].SetNativeSize();
            }
            for (int i = showImg.Length; i < ImgGrid_trans.childCount; i++)
            {
                ImgGrid_trans.GetChild(i).gameObject.SetActive(false);
            }
        }
        protected override void OnHide()
        {
            //int childCount = ImgGrid_obj.transform.childCount;
            //for (int i = 0; i < childCount; i++)
            //{
            //    ImgGrid_obj.transform.GetChild(i).gameObject.SetActive(false);
            //}
        }
    }
}
