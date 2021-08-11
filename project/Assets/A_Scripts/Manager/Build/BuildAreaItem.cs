using UnityEngine;
using UnityEditor;
namespace EazyGF
{
    public class BuildAreaItem : MonoBehaviour
    {
        public GameObject areaObj;

        public bool isUnLock;
        public int id;
        public bool isLock;

        int type; //0 区域 1 区域的遮罩
        public int AreaType { get => type; set => type = value; }

        public void BindData(int id, bool isUnLock, int type)
        {
            areaObj.layer = 14;

            this.id = id;

            this.type = type;

            ReafreshUnLockInfo(isUnLock);
        }

        public void ReafreshUnLockInfo(bool isUnLock)
        {
            this.isUnLock = isUnLock;

            gameObject.SetActive(!isUnLock);
        }
    }
}