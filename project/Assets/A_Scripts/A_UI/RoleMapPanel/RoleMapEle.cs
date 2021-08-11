using System;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    class RoleMapEle : MonoBehaviour
    {
        public List<RoleMapItem> roleMapItems;

        internal void BindData(List<RoleMapData> roleMapDatas)
        {
            for (int i = 0; i < roleMapItems.Count; i++)
            {
                if (i < roleMapDatas.Count)
                {
                    roleMapItems[i].gameObject.SetActive(true);
                    roleMapItems[i].BindData(roleMapDatas[i]);
                }
                else
                {
                    roleMapItems[i].gameObject.SetActive(false);
                }
            }

        }
    }
}
