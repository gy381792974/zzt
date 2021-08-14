using System;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class CBBData
    {
        public int type; //1建筑金币的收集 2飘字
        public Transform tf;

        public string txt;
        public int num;
        public int id;
    }

    public class BuildCollectNode : MonoBehaviour
    {
        List<FlyBBBase> userBBs = new List<FlyBBBase>();

        List<FlyBBBase> closeBBs = new List<FlyBBBase>();

        private void OnEnable()
        {
            EventManager.Instance.RegisterEvent(EventKey.SendCBBData, SendCBBDataEvent);
            EventManager.Instance.RegisterEvent(EventKey.RecycleCBBData, RecycleCBBDataEvent);
        }

        private void SendCBBDataEvent(object arg0)
        {
            CBBData cBBData = (CBBData)arg0;

            CreateBTEle(cBBData);
        }

        private void RecycleCBBDataEvent(object arg0)
        {
            FlyBBBase cbb = (FlyBBBase)arg0;
            if (userBBs.Contains(cbb))
            {
                userBBs.Remove(cbb);
            }

            cbb.gameObject.SetActive(false);
            closeBBs.Add(cbb);
        }

        public void CreateBTEle(CBBData data)
        {
            FlyBBBase collect = GetCBBItem(data.type, data.id);

            if (collect != null)
            {
                collect.BindData(data);
                userBBs.Add(collect);
            }
        }

        private void Update()
        {
            for (int i = 0; i < userBBs.Count; i++)
            {
                userBBs[i].FlyBBUpdate();
            }
        }

        private FlyBBBase GetCBBItem(int type, int id)
        {
            FlyBBBase flyBBBase = null;

            for (int i = 0; i < userBBs.Count; i++)
            {
                if (userBBs[i].Data.type == type && userBBs[i].Data.id == id)
                {
                    return userBBs[i];
                }
            }

            for (int i = 0; i < closeBBs.Count; i++)
            {
                if (closeBBs[i].Type == type)
                {
                    flyBBBase = closeBBs[i];

                    closeBBs.Remove(flyBBBase);

                    return flyBBBase;
                }
            }

            if (flyBBBase == null)
            {
                Transform go = null;
                if (type == 1)
                {
                    go = AssetMgr.Instance.LoadGameobj("BuildCollect");
                }
                else if (type == 2)
                {
                    go = AssetMgr.Instance.LoadGameobj("BuildFlyTxt");
                }else if(type == 3)
                {
                    go = AssetMgr.Instance.LoadGameobj("CusTMWait");
                }

                if (go != null)
                {
                    go.gameObject.SetActive(false);
                    go.transform.SetParent(transform);
                    go.transform.localScale = Vector3.one;

                    flyBBBase = go.GetComponent<FlyBBBase>();
                }
            }

            return flyBBBase;
        }
    }
}