using UnityEngine;
using UnityEditor;
namespace EazyGF
{
    public class BuildAreaMask : MonoBehaviour
    {
        public GameObject areaObj;
        public bool isUnLock;
        public int id;

        public void BindData(int id, bool isUnLock)
        {
            this.id = id;
            ReafreshUnLockInfo(isUnLock);
        }

        public void ReafreshUnLockInfo(bool isUnLock)
        {
            this.isUnLock = isUnLock;
            areaObj.SetActive(!isUnLock);
        }
    }
}