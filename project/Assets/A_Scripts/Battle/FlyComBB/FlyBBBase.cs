using UnityEngine;

namespace EazyGF
{
    public class FlyBBBase : MonoBehaviour
    {
        int type;
        Transform followTf;
        Vector2 off;

        protected CBBData data;

        public int Type { get => type; set => type = value; }
        public CBBData Data { get => data; set => data = value; }

        protected virtual void UpateContent()
        {

        }

        public void BindData(CBBData cBBData)
        {
            this.data = cBBData;
            this.type = cBBData.type;
            this.followTf = cBBData.tf;

            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                Vector2 vector3 = ScreenToWorldMgr.Instance.GetUILocalPostion(followTf.position);
                transform.localPosition = vector3;
            }

            UpateContent();
        }

        public void FlyBBUpdate()
        {
            Vector2 vector3 = ScreenToWorldMgr.Instance.GetUILocalPostion(followTf.position) + Vector2.up * 50;
            transform.localPosition = vector3;
        }
    }
}