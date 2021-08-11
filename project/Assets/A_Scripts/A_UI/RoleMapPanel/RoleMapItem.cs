using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{

    class RoleMapItem : MonoBehaviour
    {
        public GameObject unlock_obj;

        public GameObject lock_obj;

        public Image image_img;

        public Text name_text;

        public Button desc_btn;

        RoleMapData roleMapData;

        public GameObject purpleBox;
        private void Start()
        {
            desc_btn.onClick.AddListener(OnDescBtnClick);
        }

        bool isUnlock;

        private void OnDescBtnClick()
        {
            UIMgr.ShowPanel<RoleMapInfoPanel>(new RoleMapInfoPanelData(roleMapData.id, roleMapData.type, isUnlock));
        }

        public void BindData(RoleMapData roleMapData)
        {
            this.roleMapData = roleMapData;

            isUnlock = CustomerLogic.IsUnlock((CusType)roleMapData.type, roleMapData.id);

            unlock_obj.gameObject.SetActive(isUnlock);

            lock_obj.gameObject.SetActive(!isUnlock);
            purpleBox.SetActive(roleMapData.type == 2);
            name_text.text = roleMapData.name_text;

            if (isUnlock)
            {
                
                image_img.sprite = AssetMgr.Instance.LoadTexture("CusIcon", $"{roleMapData.iconPath}");
            }
            else
            {
                image_img.sprite = AssetMgr.Instance.LoadTexture("CusIcon", $"{roleMapData.iconPath}_shadow");
            }
        }
    }
}
