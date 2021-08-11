using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{ 
    class NewShopItem : MonoBehaviour
    {
        public Text titleTxt;
        public List<ShopBuildItem> list;
        public Transform selectFragment;

        public void Awake()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].onHandle += OnSelectHandle;
            }
        }

        private void OnSelectHandle(Transform tf)
        {
            selectFragment.SetParent(tf);
            selectFragment.localPosition = Vector3.zero;
            selectFragment.gameObject.SetActive(true);
        }

        public void BindData(int type, int id)
        {
            selectFragment.gameObject.SetActive(false);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].BindData(type, id, i + 1);
            }

            titleTxt.text = LanguageMgr.GetTranstion(BuildInital_DataBase.GetPropertyByID(id).BuildTitle);
        }
    }
}
