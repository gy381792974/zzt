using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

public class StaffBehaviour : MonoBehaviour
{
    protected ParticleSystem skillFX;
    protected int[] skillParam;
    protected int curLevel;
    protected float timer = 0;//计时器
    protected SkeletonAnimation sa;//spine 动画
    protected Ray ray;
    protected RaycastHit hit;
    protected Vector3 rayOrigin;
    protected Transform camTrans;

    void Start()
    {
        InitStaffLevel();
    }

    protected virtual void InitStaffLevel()
    {
        sa = GetComponent<SkeletonAnimation>();
    }

    private bool IsForward(Vector3 dir)
    {
        return Vector3.Dot(dir, Camera.main.transform.forward) >= 0;
    }
    private bool IsRight(Vector3 dir)
    {
        return Vector3.Dot(dir, Camera.main.transform.right) >= 0;
    }
    protected virtual void SetNPCDirection(Vector3 dir)
    {
        if (IsForward(dir))
        {
            if (IsRight(dir))
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
            if (IsRight(dir))
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public virtual void InitStaffSkill(int[] skillParam)
    {
        this.skillParam = skillParam;
    }
}
