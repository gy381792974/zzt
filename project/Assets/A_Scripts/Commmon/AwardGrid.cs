using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class AwardGrid : MonoBehaviour
    {
        public Image icon;
        public Text numText;

        public void BuildData(int id, int num)
        {
            Item_Property item = Item_DataBase.GetPropertyByID(id);
            icon.sprite = AssetMgr.Instance.LoadTexture(item.IconDir, item.IconName);
            numText.text = num.ToString();
            gameObject.SetActive(true);
        }

        public void BuildData(AwardData aD)
        {
            Item_Property item = Item_DataBase.GetPropertyByID(aD.id);
            icon.sprite = AssetMgr.Instance.LoadTexture(item.IconDir, item.IconName);
            numText.text = aD.num.ToString();
            gameObject.SetActive(true);
        }
    }
}
