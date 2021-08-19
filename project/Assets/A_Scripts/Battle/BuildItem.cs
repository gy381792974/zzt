using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class BuildItem : MonoBehaviour
    {
        public int id;
        public string buildName;
        public string desc;
        public int unLockNeedStar;
        public int buyNeedCoin;
        public GameObject unLockObj;
        public GameObject lockObj;

        public List<Transform> buildPos;
        public List<Transform> showPos;
        public List<GameObject> effects;

        private Transform buildObj;

        protected bool isUnLock = false;

        private BuildDataModel buildDataModel;

        public BuildDataModel MBuildDataModel { get => buildDataModel; set => buildDataModel = value; }


        private void SetIsUnLock(bool isUnLock)
        {
            this.isUnLock = isUnLock;

            unLockObj.SetActive(isUnLock);
            lockObj.SetActive(!isUnLock);
        }

        private int level;

        public virtual void ShowBuild(string mPath, int level = 0)
        {
            SetIsUnLock(level != 0);
            for (int i = 0; i < buildPos.Count; i++)
            {
                buildPos[i].gameObject.SetActive(i == level - 1);
            }

            for (int i = 0; i < showPos.Count; i++)
            {
                showPos[i].gameObject.SetActive(i == level - 1);
            }
        }

        public virtual void ShowBuild()
        {
            for (int i = 0; i < buildPos.Count; i++)
            {
                buildPos[i].gameObject.SetActive(i == buildDataModel.Level - 1);
            }

            for (int i = 0; i < showPos.Count; i++)
            {
                showPos[i].gameObject.SetActive(i == buildDataModel.Level - 1);
            }
        }

        public virtual void BuildData(BuildDataModel buildDataModel)
        {
            this.buildDataModel = buildDataModel;

            SetIsUnLock(MBuildDataModel.Level > 0);

            level = MBuildDataModel.Level;
        }

        public virtual Vector3 GetShowBuildPos()
        {
            if (level >= 1 && level <= 4)
            {
                return buildPos[level - 1].GetChild(0).GetComponentInChildren<MeshRenderer>().bounds.center;
            }

            return Vector3.zero;
        }

        public virtual MeshRenderer GetShowBuildMR()
        {
            if (level >= 1 && level <= 4)
            {
                return buildPos[level - 1].GetChild(0).GetComponentInChildren<MeshRenderer>();
            }

            return buildPos[0].GetChild(0).GetComponentInChildren<MeshRenderer>();
        }

        public virtual Vector3 GetShowBuildPos(int pos, int level)
        {
            if (level >= 1 && level <= 4)
            {
                return buildPos[level - 1].GetChild(0).GetComponentInChildren<MeshRenderer>().bounds.center;
            }

            return Vector3.zero;
        }

        public virtual MeshRenderer GetShowBuildMR(int pos, int level)
        {
            return buildPos[level - 1].GetChild(0).GetComponentInChildren<MeshRenderer>();
        }

        public Transform GetShowBuildBoxTf()
        {
            if (level >= 1 && level <= 4)
            {
                return buildPos[level - 1].GetChild(1).transform;
            }

            return buildPos[0].GetChild(1).transform;
        }

        public virtual Transform GetShowBuildBoxTf(int pos)
        {
            if (level >= 1 && level <= 4)
            {
                return buildPos[level - 1].GetChild(1).transform;
            }

            return buildPos[0].GetChild(1).transform;
        }


        public void HideShow()
        {
            for (int i = 0; i < showPos.Count; i++)
            {
                showPos[i].gameObject.SetActive(false);
            }
        }


        public virtual void PlayerEffect()
        {
            return;

            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].gameObject.SetActive(false);
            }

            effectGrid.gameObject.SetActive(true);

            effects[level - 1].gameObject.SetActive(true);
        }

        public void HidePlayEffect()
        {
            effectGrid.gameObject.SetActive(false);
        }

        public Transform effectGrid;


        public void InitEffectPos()
        {
            effectGrid.gameObject.SetActive(true);
            effects.Clear();

            for (int i = 0; i < effectGrid.childCount; i++)
            {
                effects.Add(effectGrid.GetChild(i).gameObject);
                effects[i].name = $"effect{i}";
                effects[i].gameObject.SetActive(false);
            }

            Debug.LogError("成功");
        }
    }
}
