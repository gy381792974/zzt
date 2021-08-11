using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class DancingGirlGameItem : MonoBehaviour
    {
        public Button selectBtn;
        public GameObject meObj;
        public GameObject otherObj;

        int index;
        Action<int> action;

        private void Start()
        {
            selectBtn.onClick.AddListener(SelectedBtnClick);
        }

        int type = 0;

        public void BindData(int type, int index = 0)
        {
            this.type = type;
            this.index = index;

            UpdateUIType();
        }

        public void SetOnClickHandle(Action<int> fun)
        {
            this.action = fun;
        }

        private void UpdateUIType()
        {
            if (type == 0)
            {
                meObj.SetActive(false);
                otherObj.SetActive(false);
            }
            else if (type == 1)
            {
                meObj.SetActive(true);
                otherObj.SetActive(false);
            }
            else if (type == 2)
            {
                meObj.SetActive(false);
                otherObj.SetActive(true);
            }

            selectBtn.interactable = type == 0;
        }

        private void SelectedBtnClick()
        {
            type = 1;
            UpdateUIType();
            MusicMgr.Instance.PlayMusicEff("d_guests_d_select");
            this.action(index);
        }
    }
}
