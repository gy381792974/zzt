using System;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    class RoleItem : MonoBehaviour
    {
        public GameObject unlock_obj;

        public GameObject lock_obj;

        public Image image_img;

        public Text name_text;

        public Button desc_btn;

        RoleMapData roleMapData;

        private void Start()
        {
            desc_btn.onClick.AddListener(OnDescBtnClick);
        }


        private void OnDescBtnClick()
        {
            //UIMgr.ShowPanel<Sh>();
        }

        public void  BindData(RoleMapData roleMapData)
        {
            this.roleMapData = roleMapData;

            bool isUnlock = CustomerLogic.IsUnlock((CusType)roleMapData.type, roleMapData.id);

            unlock_obj.gameObject.SetActive(isUnlock);

            lock_obj.gameObject.SetActive(!isUnlock);
        }
    }
}
