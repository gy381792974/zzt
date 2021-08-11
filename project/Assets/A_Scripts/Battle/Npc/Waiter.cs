using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

public class Waiter : StaffBehaviour
{
    [SerializeField] float offsetY = 1.5f;
    string lastCusName = "";
    Vector3 rayDir;
    ClickGainGold gold;
    Spine.TrackEntry track;
    [SerializeField] StaffDir dir = StaffDir.Left;

    void Update()
    {
        WaiterBehaviour();
    }
    protected override void InitStaffLevel()
    {
        base.InitStaffLevel();
        Init();
    }
    private void Init()
    {
        rayOrigin = transform.position + Vector3.up * 0.8f;
        StaffRayDir();
        sa.AnimationState.SetAnimation(0, "breath", true);
        //GetGold();
    }
    private void StaffRayDir()
    {
        switch (dir)
        {
            case StaffDir.Left:
                rayDir = Quaternion.Euler(0, -135, 0) * transform.forward;
                break;
            case StaffDir.Right:
                rayDir = Quaternion.Euler(0, 45, 0) * transform.forward;
                break;
            case StaffDir.Forword:
                rayDir = Quaternion.Euler(0, -45, 0) * transform.forward;
                break;
            case StaffDir.Back:
                rayDir = Quaternion.Euler(0, 135, 0) * transform.forward;
                break;
        }
    }




    private void OnEnable()
    {
        //EventManager.Instance.RegisterEvent(EventKey.CheckoutCounterLevelUpdate, Pos);
    }


    /// <summary>
    /// 获取不同等级的位置
    /// </summary>
    /// <param name="arg0"></param>
    //private void Pos(object arg0)
    //{
    //    transform.position = pos.GetChild((int)arg0 - 1).position;
    //}

    private void WaiterBehaviour()
    {
        ray = new Ray(rayOrigin, rayDir);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 3f);
        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.transform.tag == "Cus")
            {
                CustomerNor cn = hit.transform.GetComponent<CustomerNor>();
                if (cn != null && cn.curState == NCSuatus.Dining)
                {
                    if (hit.transform.name != lastCusName && gold.Id == 1)
                    {
                        if (timer >= 2)
                        {
                            lastCusName = hit.transform.name;
                            sa.AnimationState.SetEmptyAnimation(0, 0);
                            sa.AnimationState.SetAnimation(0, "work", false);
                            sa.AnimationState.AddAnimation(0, "breath", true, 0);
                            timer = 0;
                        }

                    }
                    if (gold.GoldNum >= skillParam[1])
                    {


                    }
                    else if (cn.IsGaveELTip())
                    {
                        gold.GoldNum += skillParam[0];
                        ShowImg();
                        Debug.Log($"金币{gold.GoldNum}");
                    }
                }
            }
        }
        if (timer < 2)
        {
            timer += Time.deltaTime;
        }
        if (gold != null && gold.gameObject.activeSelf)
        {
            ScreenToWorldMgr.Instance.AddChild(gold.transform);
            gold.transform.localPosition = ScreenToWorldMgr.Instance.GetUILocalPostion(transform.position + Vector3.up * offsetY);
        }
    }

    private void CastSkill()
    {
        if (gold.GoldNum >= skillParam[1])
        {
            return;
        }

        ray = new Ray(rayOrigin, rayDir);
        if (Physics.Raycast(ray, out hit, 4f))
        {
            if (hit.transform.tag == "cus")
            {
                CustomerNor cn = hit.transform.GetComponent<CustomerNor>();
                if (cn.IsGaveELTip())
                {
                    ItemPropsManager.Intance.AddItem(1, skillParam[0]);
                    gold.GoldNum += skillParam[0];
                }
            }

        }
    }

    public void InitData(int num, int id)
    {
        GetGold(num, id);
        if (gold.gameObject.activeSelf)
        {
            ScreenToWorldMgr.Instance.AddChild(gold.transform);
            gold.transform.localPosition = ScreenToWorldMgr.Instance.GetUILocalPostion(transform.position + Vector3.up * offsetY);
        }
    }

    private void GetGold(int num, int id)
    {
        if (gold == null)
        {
            Transform goldtrans = AssetMgr.Instance.LoadGameobj("GoldSpine");
            gold = goldtrans.GetComponent<ClickGainGold>();
            gold.Id = id;
            gold.GoldNum = num;
            gold.gameObject.SetActive(num > 0);
        }
    }

    private void ShowImg()
    {
        if (gold.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            gold.gameObject.SetActive(true);
        }
    }

    public override void InitStaffSkill(int[] skillParam)
    {
        base.InitStaffSkill(skillParam);
    }

}
public enum StaffDir
{
    Left,
    Right,
    Forword,
    Back
}