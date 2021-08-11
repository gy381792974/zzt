using System;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class DropCoin : MonoBehaviour
    {
        int coinNum = 10;

        public void BuildData(int coinNum)
        {
            this.coinNum = coinNum;
        }

        public void PackUpCoin()
        {
            ItemPropsManager.Intance.AddItem((int)CurrencyType.Coin, coinNum * 30);

            PoolMgr.Instance.DespawnOne(transform);
        }

        //void OnMouseDown()
        //{
        //    Debug.LogError("  " + gameObject.name);
        //}

    }
}
