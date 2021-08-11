using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public enum RoleType
    {
        Clown = 0,
        DancingGirl = 1,
        Magician = 2
    }

    public class RoleDialoguePanelData : UIDataBase
    {

        public CustomerSpecial_Property data;
        public int hashCode;

        public RoleDialoguePanelData(CustomerSpecial_Property data, int hashCode)
        {
            this.data = data;
            this.hashCode = hashCode;
        }
    }

    public partial class RoleDialoguePanel : UIBase
    {
        [SerializeField] Sprite[] SpriteSwitch;
        protected override void OnInit()
        {
            left_btn.onClick.AddListener(Left_btnClick);
            left_text.text = LanguageMgr.GetTranstion(1, 13);
            right_btn.onClick.AddListener(Right_btnClick);
            righ_text.text = LanguageMgr.GetTranstion(1, 12);
            middle_btn.onClick.AddListener(Middle_btnClick);
            middle_text.text = LanguageMgr.GetTranstion(1, 8);
        }

        private void Right_btnClick()
        {
            //BtnClickAnimation(right_btn.transform, () =>
            //{
            //    UIMgr.HideUI<RoleDialoguePanel>();
            //    EventManager.Instance.TriggerEvent(EventKey.SpeRoleTriggerEvent, mPanelData.hashCode);
            //});
            UIMgr.HideUI<RoleDialoguePanel>();
            EventManager.Instance.TriggerEvent(EventKey.SpeRoleTriggerEvent, mPanelData.hashCode);
        }

        private void Left_btnClick()
        {
            //BtnClickAnimation(left_btn.transform, () => { UIMgr.HideUI<RoleDialoguePanel>(); });
            EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, mPanelData.hashCode);
            UIMgr.HideUI<RoleDialoguePanel>();
        }

        private void Middle_btnClick()
        {
            if (mPanelData.data.ID != 2)
                EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, mPanelData.hashCode);
            UIMgr.HideUI<RoleDialoguePanel>();
        }


        protected override void OnShow(UIDataBase roledialoguepanelData = null)
        {
            if (roledialoguepanelData != null)
            {
                mPanelData = roledialoguepanelData as RoleDialoguePanelData;
            }

            Icon_img.sprite = SpriteSwitch[mPanelData.data.RoleIndex];
            Title_Text.text = LanguageMgr.GetTranstion(mPanelData.data.Name);
        }

        public void ShowClickDialogue()
        {
            left_btn.gameObject.SetActive(true);
            right_btn.gameObject.SetActive(true);
            middle_btn.gameObject.SetActive(false);
            content_text.text = LanguageMgr.GetTranstion(mPanelData.data.Content);
            switch (mPanelData.data.ID)
            {
                case 1:
                    MusicMgr.Instance.PlayMusicEff("d_guests_c_hi");
                    break;
                case 2:
                    MusicMgr.Instance.PlayMusicEff("d_guests_dancer_hi");
                    break;
                case 3:
                    MusicMgr.Instance.PlayMusicEff("d_guests_m_hi");
                    break;
            }
        }

        public void ShowFailDialogue()
        {
            left_btn.gameObject.SetActive(false);
            right_btn.gameObject.SetActive(false);
            middle_btn.gameObject.SetActive(true);
            content_text.text = LanguageMgr.GetTranstion(mPanelData.data.GameFail);
        }
 
        public void ShowVictoryDialogue()
        {
            left_btn.gameObject.SetActive(false);
            right_btn.gameObject.SetActive(false);
            middle_btn.gameObject.SetActive(true);
            content_text.text = LanguageMgr.GetTranstion(mPanelData.data.GameSuc);
        }

        protected override void OnHide()
        {



        }
    }
}
