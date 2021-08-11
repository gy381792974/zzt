using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class MagicGamePanelData : UIDataBase
    {
        public int mHashCode;
        public MagicGamePanelData(int mHashCode)
        {
            this.mHashCode = mHashCode;
        }
    }

    public partial class MagicGamePanel : UIBase
    {
        [SerializeField] private List<MagicGameItem> magicGameItemList;
        [SerializeField] private List<Sprite> spriteList;
        List<int> turnBrandList = new List<int>();
        private bool isDebug = true;
        RoleDialoguePanel dialoguePanel;
        protected override void OnInit()
        {
            dialoguePanel = UIMgr.GetUI<RoleDialoguePanel>();
            title_text.text = LanguageMgr.GetTranstion(CustomerSpecial_DataBase.GetPropertyByID(3).titleText);
            close_btn.onClick.AddListener(()=> { UIMgr.HideUI<MagicGamePanel>(); });
        }

        public Transform grid;
        [ContextMenu("InitComponent")]
        private void InitComponent()
        {
            magicGameItemList.Clear();

            for (int i = 0; i < grid.childCount; i++)
            {
                magicGameItemList.Add(grid.GetChild(i).GetComponent<MagicGameItem>());
            }
        }

        protected override void OnShow(UIDataBase magicgamepanelData = null)
        {
            if (magicgamepanelData != null)
            {
                mPanelData = magicgamepanelData as MagicGamePanelData;
            }

            for (int i = 0; i < magicGameItemList.Count; i++)
            {
                int val = Random.Range(1, 10);

                magicGameItemList[i].InitData(i, val, spriteList[val]);
                magicGameItemList[i].SetOnClickHandle(TurnBrandHandle);
            }

            turnBrandList.Clear();
        }

        private void TurnBrandHandle(int index, int value)
        {
            turnBrandList.Add(value);

            if (turnBrandList.Count == 1)
            {
                for (int i = 0; i < turnBrandList.Count; i++)
                {
                    if (i != index)
                    {
                        magicGameItemList[i].BindData(value, spriteList[value]);
                    }
                }

                int robotIndex = Random.Range(0, magicGameItemList.Count);

                while (robotIndex == index)
                {
                    robotIndex = Random.Range(0, magicGameItemList.Count);
                }
                magicGameItemList[robotIndex].OnClick();
            }

            if (turnBrandList.Count >= 2)
            {
                if (turnBrandList[0] >= turnBrandList[1])
                {
                    Victory();
                }
                else
                {
                    Fail();
                }

                UIMgr.HideUI<MaskPanel>();
            }
        }

        private void Victory()
        {
            MusicMgr.Instance.PlayMusicEff("d_guests_m_win");
            UIMgr.ShowPanel<RoleDialoguePanel>();
            dialoguePanel.ShowVictoryDialogue();
            UIMgr.HideUI<MagicGamePanel>();
            //UIMgr.HideUI<MaskPanel>();
            //EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, mPanelData.mHashCode);
        }

        private void Fail()
        {
            MusicMgr.Instance.PlayMusicEff("d_guests_m_lose");
            UIMgr.ShowPanel<RoleDialoguePanel>();
            dialoguePanel.ShowFailDialogue();
            //EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, mPanelData.mHashCode);
            UIMgr.HideUI<MagicGamePanel>();
           // UIMgr.HideUI<MaskPanel>();
        }

        protected override void OnHide()
        {

        }
    }
}
