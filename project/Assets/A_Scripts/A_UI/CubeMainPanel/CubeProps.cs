using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class CubeProps : MonoBehaviour
    {
        public Button mBtn;
        public Text item_Num;
        public Button addBtn;
        public GameObject txtBg;
        private SkeletonGraphic sg;
        int num;
        int id;

        public void BindData(int id)
        {
            num = ItemPropsManager.Intance.GetItemNum(id);

            this.id = id;

            item_Num.text = num.ToString();

            txtBg.SetActive(num > 0);
            addBtn.gameObject.SetActive(num <= 0);
        }

        private void Start()
        {
            sg = mBtn.GetComponent<SkeletonGraphic>();

            mBtn.onClick.AddListener(MBtnClick);

            addBtn.onClick.AddListener(MBtnClick);
        }

        private void MBtnClick()
        {
            if (num <= 0)
            {
                UIMgr.ShowPanel<ItemBuyPanel>(new ItemBuyPanelData(id));
            }
            else
            {
                if (CubeGameMgr.Instance.UserItem(id))
                {
                    ItemPropsManager.Intance.CoseItem(id, 1, true);
                    sg.AnimationState.SetAnimation(0, "animation", false);
                    ClickItemAudio();
                }
                else
                {
                    TipCommonTool.Instance.ShowTip(LanguageMgr.GetTranstion(new int[2] { 3, 1 }));
                    MusicMgr.Instance.PlayMusicEff("g_btn_err");
                }
            }
        }

        private void ClickItemAudio()
        {
            switch (id)
            {
                case 10:
                    MusicMgr.Instance.PlayMusicEff("c_item_magnifier");
                    break;
                case 11:
                    MusicMgr.Instance.PlayMusicEff("c_item_lodestone");
                    break;
                case 12:
                    MusicMgr.Instance.PlayMusicEff("c_item_renumber");
                    break;

            }
        }

    }
}
