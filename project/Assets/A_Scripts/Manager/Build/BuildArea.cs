using System;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class BuildArea : MonoBehaviour
    {
        public List<BuildAreaItem> areaItems;
        public List<BuildAreaItem> buildMaskItems;

        public Transform areaGrid;
        public Transform maskItemGrid;

        [ContextMenu("FillItem")]
        public void FillItem()
        {
            areaItems.Clear();
            for (int i = 0; i < areaGrid.childCount; i++)
            {
                areaGrid.GetChild(i).gameObject.name = $"{i}";
                areaItems.Add(areaGrid.GetChild(i).GetComponent<BuildAreaItem>());
            }

            buildMaskItems.Clear();
            for (int i = 0; i < maskItemGrid.childCount; i++)
            {
                maskItemGrid.GetChild(i).gameObject.name = $"{i}";
                buildMaskItems.Add(maskItemGrid.GetChild(i).GetComponent<BuildAreaItem>());
            }

            Debug.LogError("SUCCESS");
        }

        void Start()
        {
            areaGrid.gameObject.SetActive(true);
            maskItemGrid.gameObject.SetActive(true);

            Dictionary<int, bool> areaMaskData = BuildAreaMgr.Instance.buildAreaMash;
            int index = 0;
            foreach (var item in areaMaskData)
            {
                buildMaskItems[index].BindData(item.Key, item.Value, 1);
                index++;
            }

            Dictionary<int, BuildAreadInfo> areaLockInfos = BuildAreaMgr.Instance.buildAreaUnlockDic;
            index = 0;

            foreach (var item in areaLockInfos)
            {
                bool isCanUnLock = BuildAreaMgr.Instance.GetIsUnLockAreaById(item.Key);

                areaItems[index].BindData(item.Key, isCanUnLock, 0);

                index++;
            }

            EventManager.Instance.RegisterEvent(EventKey.BuildAreaDataUpdate, BuildAreaDataUpdate);
        }

        private void BuildAreaDataUpdate(object arg0)
        {
            BuildAreadInfo buildAreadInfo = (BuildAreadInfo)arg0;

            UpdateBuildArea(buildAreadInfo.id, buildAreadInfo.isUnlock, buildAreadInfo.type);
        }

        public void UpdateBuildArea(int id, bool isUnlock, int type)
        {
            if (type == 0)
            {
                for (int i = 0; i < areaItems.Count; i++)
                {
                    if (areaItems[i].id == id)
                    {
                        areaItems[i].ReafreshUnLockInfo(isUnlock);
                        break;
                    }
                }
            }
            else if (type == 1)
            {
                for (int i = 0; i < buildMaskItems.Count; i++)
                {
                    if (buildMaskItems[i].id == id)
                    {
                        buildMaskItems[i].ReafreshUnLockInfo(isUnlock);
                        break;
                    }
                }
            }
        }

    }
}
