using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

public class Reception : StaffBehaviour
{
    [SerializeField] private float interval = 2;
    float skillTimer;
    Vector3 rayDir;
    private void Update()
    {
        ReceptionBehaviour();
        CastSkill();
    }

    protected override void InitStaffLevel()
    {
        base.InitStaffLevel();
        rayOrigin = transform.position + Vector3.up * 0.8f;
        rayDir = Quaternion.Euler(0, 180, 0) * transform.forward;
        sa.AnimationState.SetAnimation(0, "breath", true);
    }

    private void ReceptionBehaviour()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            ray = new Ray(rayOrigin, rayDir);
            //Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
            bool hitObj = Physics.Raycast(ray, out hit, 5f);
            if (hitObj)
            {
                CustomerNor cn = hit.collider.GetComponent<CustomerNor>();
                if (cn != null && cn.curState == NCSuatus.Enter)
                {
                    if (timer >= interval)
                    {
                        timer = 0;
                        sa.AnimationState.SetEmptyAnimation(0, 0);
                        sa.AnimationState.SetAnimation(0, "work", false);
                        sa.AnimationState.AddAnimation(0, "breath", true, 0);
                    }
                }
            }
        }

    }

    protected void CastSkill()
    {
        skillTimer += Time.deltaTime;
        if (skillTimer >= skillParam[0])
        {
            skillTimer = 0;
            if (skillFX != null)
            {
                skillFX.gameObject.SetActive(false);
                skillFX.gameObject.SetActive(true);
            }
            EventManager.Instance.TriggerEvent(EventKey.FastCreateCusBuff, skillParam[1]);
        }
    }

    public override void InitStaffSkill(int[] skillParam)
    {
        base.InitStaffSkill(skillParam);
    }

}
