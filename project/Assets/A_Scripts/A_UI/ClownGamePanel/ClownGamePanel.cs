using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    public class ClownGamePanelData : UIDataBase
    {

        public int mHashCode;

        public ClownGamePanelData(int hashCode)
        {
            this.mHashCode = hashCode;
        }
    }

    public partial class ClownGamePanel : UIBase
    {

        [SerializeField] private GameObject award1_obj;
        [SerializeField] private List<Button> SelectedBtns;
        [SerializeField] private Button closeBtn;
        //[SerializeField] private List<SK> sgs;
        RoleDialoguePanel dialoguePanel;
        AwardData[] ad = new AwardData[3];
        int k;
        bool isSelect = false;
        protected override void OnInit()
        {
            title_text.text = LanguageMgr.GetTranstion(CustomerSpecial_DataBase.GetPropertyByID(1).titleText);
            dialoguePanel = UIMgr.GetUI<RoleDialoguePanel>();
            ad[0] = new AwardData(2, 1);
            ad[1] = new AwardData(Random.Range(Item_Data.DataArray[3].ID, Item_Data.DataArray[Item_Data.ArrayLenth - 1].ID + 1), 10);
            ad[2] = new AwardData(1, 100);
            for (int i = 0; i < SelectedBtns.Count; i++)
            {
                int index = i;
                SelectedBtns[i].onClick.AddListener(() =>
                {
                    SelectedAward(index);
                    UIMgr.ShowPanel<ClownPDPanel>();
                    MusicMgr.Instance.PlayMusicEff("d_guests_c_select");
                });
            }

            closeBtn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<ClownGamePanel>();
            });
        }

        private void SelectedAward(int index)
        {
            Debug.LogWarning(" SelectedAward " + index);

            if (isSelect)
            {
                return;
            }

            isSelect = true;

            int[] rangArr = new int[] { 0, 1, 2 };
            for (int i = 0; i < rangArr.Length; i++)
            {
                int r = Random.Range(0, rangArr.Length);

                if (i != r)
                {
                    int v = rangArr[i];
                    rangArr[i] = rangArr[r];
                    rangArr[r] = v;
                }
            }

            int k = rangArr[index];

            UIMgr.HideUI<ClownGamePanel>(() =>
            {
                UIMgr.ShowPanel<ComRewardPanel>(new ComRewardPanelData(new List<AwardData>() { ad[k] }));
                UIMgr.ShowPanel<RoleDialoguePanel>();
                dialoguePanel.ShowVictoryDialogue();
            });
        }

        protected override void OnShow(UIDataBase clowngamepanelData = null)
        {
            if (clowngamepanelData != null)
            {
                mPanelData = clowngamepanelData as ClownGamePanelData;
            }

            isSelect = false;
        }

        protected override void OnHide()
        {

        }
    }
}
