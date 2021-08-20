using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{

    class CustomerSpe : MonoBehaviour
    {
        private Vector3 dir;

        Queue<Vector3> queue = new Queue<Vector3>();

        private bool isTargetPos = true;
        public float moveSpeed;

        Vector3 targetPos = Vector3.zero;

        public delegate void GoToTargetPosHandle(CustomerSpe car);

        public event GoToTargetPosHandle onHandle2;  //将队列中的点走完时的回调

        public SkeletonAnimation[] sas;

        public SRoleStage curSRoleStage;

        CustomerSpecial_Property data;

        public CustomerSpecial_Property Data { get => data; set => data = value; }
        public int MHashCode { get => mHashCode; set => mHashCode = value; }

        private int mHashCode;

        public bool isClicked;
        [SerializeField] Animator animator;
        private void Start()
        {
            PlayAnim();
        }

        public void PlayAnim()
        {
            animator.Play("VIP");
        }



        public void StopPlayAnim()
        {
            //animator.SetBool("VIP", true);
            //animator.Play("null");
        }

        public void ResetData()
        {
            onHandle2 = null;
            onHandle2 += (x) => { StopPlayAnim(); };
        }

        public void MoveToTargetPoint(List<Vector3> pos)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                queue.Enqueue(pos[i]);
            }

            PlayAni("move");
        }

        public void MoveToTargetPoint(Vector3[] pos)
        {
            for (int i = 0; i < pos.Length; i++)
            {
                queue.Enqueue(pos[i]);
            }

            PlayAni("move");
        }

        public void MoveToTargetPoint(List<Transform> tf)
        {
            for (int i = 0; i < tf.Count; i++)
            {
                queue.Enqueue(tf[i].transform.position);
            }
            PlayAni("move");
        }
        private void SetSpeCustomerStatus(SRoleStage sRole)
        {
            switch (sRole)
            {
                case SRoleStage.MoveTarget:
                    PlayAni("move");
                    break;
                case SRoleStage.Breath:
                    PlayAni("breath");
                    break;
                case SRoleStage.Action:
                    PlayAni("action");
                    break;
                case SRoleStage.Leave:
                    PlayAni("move");
                    break;
            }
        }


        public virtual void ShowAction()
        {
            if (data.RoleIndex == 0)
            {
                UIMgr.ShowPanel<ClownGamePanel>(new ClownGamePanelData(mHashCode));
            }
            else if (data.RoleIndex == 1)
            {
                UIMgr.ShowPanel<DancingGirlGamePanel>(new DancingGirlGamePanelData(mHashCode));
            }
            else if (data.RoleIndex == 2)
            {
                UIMgr.ShowPanel<MagicGamePanel>(new MagicGamePanelData(mHashCode));
            }
        }

        string lastPlayAniName;
        int curIndex;
        private void PlayAni(string playAni)
        {
            if (playAni == "action")
            {
                curIndex = 0;
            }
            sas[1 - curIndex].gameObject.SetActive(false);
            sas[curIndex].gameObject.SetActive(true);
            lastPlayAniName = playAni;
            if (sas[curIndex].skeletonDataAsset != null)
            {
                sas[curIndex].AnimationState.SetAnimation(0, playAni, true);
                sas[curIndex].skeleton.SetToSetupPose();
            }
        }
        public void SetTurn()
        {
            if (IsForward())
            {
                curIndex = 1;
                if (IsRight())
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                curIndex = 0;
                if (IsRight())
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }

            PlayAni(lastPlayAniName);
        }

        private bool IsForward()
        {
            return Vector3.Dot(dir, Camera.main.transform.forward) >= 0;
        }

        private bool IsRight()
        {
            return Vector3.Dot(dir, Camera.main.transform.right) >= 0;
        }

        public void DancingGirlPlayAnim(string action)
        {
            PlayAni(action);
        }
        protected virtual void SetRoleStill()
        {
            if (curSRoleStage == SRoleStage.MoveTarget)
            {
                //sa.skeletonDataAsset = CustomerLogic.GetSkeletonDataAssetByName(data.ABName, data.RoleName);
                //sa.Initialize(true);
                //SetTurn();
                RoleBehaviourAtEndPoint();
            }
        }

        private void RoleBehaviourAtEndPoint()
        {
            switch (data.RoleName)
            {
                case "role_2001":
                    PlayAni("action");
                    break;
                case "role_2002":
                    curIndex = 1;
                    PlayAni("breath");
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
                case "role_2003":
                    PlayAni("action");
                    break;
            }
            MusicMgr.Instance.PlayMusicEff("d_guests_visit");
        }


        public void UpdateSC()
        {
            if (queue.Count > 0 && isTargetPos)
            {
                targetPos = queue.Dequeue();
                isTargetPos = false;

                dir = (targetPos - transform.position).normalized;
                SetTurn();
            }

            if (!isTargetPos)
            {
                if (Vector3.Distance(transform.position, targetPos) <= 0.03f)
                {
                    transform.position = targetPos;
                    isTargetPos = true;
                    targetPos = Vector3.zero;

                    if (onHandle2 != null && queue.Count == 0)
                    {
                        SetRoleStill();
                        isClicked = false;
                        onHandle2(this);
                    }
                }
                else
                {
                    transform.position += dir * moveSpeed * Time.deltaTime;
                }
            }
        }
    }
}