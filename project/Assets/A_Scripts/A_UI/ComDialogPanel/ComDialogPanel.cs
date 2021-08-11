using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

	public class ComDialogPanelData : UIDataBase
	{
        public int type;
        public string imagePath;
        public string content;
        public string title;
        public Action leftFun;
        public Action rightFun;

        public ComDialogPanelData(int type, string content = "", string title = "", Action leftFun = null, Action rightFun = null, string imagePath = "")
        {
            this.type = type;
            this.imagePath = imagePath;
            this.content = content;
            this.title = title;
            this.leftFun = leftFun;
            this.rightFun = rightFun;
        }
    }

	public partial class ComDialogPanel : UIBase
	{

		protected override void OnInit()
		{
            left_btn.onClick.AddListener(Left_btnClick);
            left_text.text = LanguageMgr.GetTranstion(1, 13);
            right_btn.onClick.AddListener(Right_btnClick);
            righ_text.text = LanguageMgr.GetTranstion(1, 12);
            middle_btn.onClick.AddListener(Middle_btnClick);
            middle_text.text = LanguageMgr.GetTranstion(1, 8);
        }

        protected override void OnShow(UIDataBase comdialogpanelData = null)
		{
			if (comdialogpanelData != null)
			{
                mPanelData = comdialogpanelData as ComDialogPanelData;
			}

            if (mPanelData.type == 0)
            {
                left_btn.gameObject.SetActive(true);
                right_btn.gameObject.SetActive(true);
                middle_btn.gameObject.SetActive(false);
            }

            if (mPanelData.title != "")
            {
                Title_Text.text = mPanelData.title;
            }

            if (mPanelData.content != "")
            {
                content_text.text = mPanelData.content;
            }

            if (mPanelData.imagePath != "")
            {
                Icon_img.sprite = AssetMgr.Instance.LoadTexture("TestTexture", mPanelData.imagePath);
            }
        }

		protected override void OnHide()
		{

		}

        private void Right_btnClick()
        {
            
        }

        private void Left_btnClick()
        {
            if (mPanelData != null && mPanelData.leftFun != null)
            {
                mPanelData.leftFun();
            }
        }

        private void Middle_btnClick()
        {
            if (mPanelData != null && mPanelData.rightFun != null)
            {
                mPanelData.rightFun();
            }
        }
    }
}
