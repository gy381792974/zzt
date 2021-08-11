using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class CubeItem : MonoBehaviour
    {
        public Image imge;
      
        public bool isUnLock;

        public Transform sFPosTf;

        CubeItem_Property data;

        public Button selectedBtn;

        int posIndex;
        int mLayout;

        public int PosIndex { get => posIndex; set => posIndex = value; }

        public CubeItem_Property Data { get => data; set => data = value; }
        public int MLayout { get => mLayout; set => mLayout = value; }

        private void Start()
        {
            selectedBtn.onClick.AddListener(SelectedBtnClick);
        }

        private void SelectedBtnClick()
        {
            if (isUnLock)
            {
                CubeGameMgr.Instance.SetSelectedCube(this);
            }
        }

        //public List<int> unLockNeedIndexs = null;
        //public List<int> canLockIndexs = null;

        public void BindData(CubeItem_Property data, CubeLaoutOff lastData, CubeLaoutOff nextData)
        {
            this.data = data;

            imge.sprite = AssetMgr.Instance.LoadTexture(data.AbName, data.TextureName);

            selectedBtn.interactable = true;
            imge.color = Color.white;
        }

        public void UpdateData()
        {
            imge.sprite = AssetMgr.Instance.LoadTexture(data.AbName, data.TextureName);
        }

        public void SetLockInfo()
        {
            this.isUnLock = CubeGameMgr.Instance.IsUnlock(PosIndex, mLayout);

            imge.color = isUnLock ? Color.white : Color.gray;

            selectedBtn.interactable = isUnLock;
        }
    }
}
