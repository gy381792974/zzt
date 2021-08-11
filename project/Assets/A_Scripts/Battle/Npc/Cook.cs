using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using Spine.Unity;

public class Cook : StaffBehaviour
{
    [SerializeField] private float interval = 4;
    [SerializeField] private float cookSpeed = 1f;
    [SerializeField] SkeletonAnimation[] sas;
    public string assetBundleName;
    public string skeletonDataName;
    public CookStatus cook = CookStatus.Idle;//厨师的初始状态
    private Transform cookTarget;
    private bool isMove = false;             //判断厨师是否处于移动动态
    bool defaultDir = true;
    private Vector3 targetPoint;             //厨师的工作位置
    int curIndex = 0;
    int index = 0;
    Vector3 dir;
    Transform curPath = null;

    private void Awake()
    {
        LoadSpineAsset();
    }
    private void LoadSpineAsset()
    {
        if (assetBundleName.IsNull() || skeletonDataName.IsNull())
        {
            return;
        }
        SkeletonDataAsset data = UICommonUtil.Instance.LoadSkeletonDataAsset(assetBundleName, skeletonDataName);
        if (data != null)
        {
            sas[0].skeletonDataAsset = data;
            sas[1].skeletonDataAsset = data;
        }
    }

    private void Update()
    {
        CookStatusCtrl();
    }

    private void OnEnable()
    {
        //EventManager.Instance.RegisterEvent(EventKey.KitChenLevelChange, TriggerKitchenChange);
    }

    public void UpdateCookLevel()
    {
        ItemPropsManager.Intance.AddItem((int)CurrencyType.Bottle, skillParam[0]);
    }

    private void TriggerKitchenChange(object arg)
    {
        GetLineByStallLevel(arg);
        sas[curIndex].gameObject.SetActive(false);
        timer = 0;
        defaultDir = true;
        Invoke("SetActiveCook", 2f);
        this.enabled = false;
    }
    public void SetCookMoveTarget(Transform target)
    {
        cookTarget = target;
    }


    private void SetActiveCook()
    {
        this.enabled = true;
        sas[0].gameObject.SetActive(true);
    }

    //厨师初始状态
    protected override void InitStaffLevel()
    {
        base.InitStaffLevel();
        //LoadSpineAsset();
        sas[0].gameObject.SetActive(true);
        sas[1].gameObject.SetActive(true);
        GetLineByStallLevel(BuildMgr.GetKitchenLevel());
        InitCook();
    }

    private void InitCook()
    {
        transform.position = targetPoint = curPath.GetChild(0).position;
        index = 0;
        cook = CookStatus.Idle;
        SetCookIsMove();
        SetCookBehaviour();
    }

    private void CookStatusCtrl()
    {
        if (!isMove)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                SwitchStatus();
            }
        }
        else
        {
            CookMoveToTarget(targetPoint);
        }
    }

    private void GetLineByStallLevel(object arg)
    {
        curPath = cookTarget.GetChild((int)arg - 1);
        InitCook();
    }

    private Vector3 GetPathPoint(int index)
    {
        dir = curPath.GetChild(0).position - transform.position;
        return curPath.GetChild(index).position;
    }

    private void CookMoveToTarget(Vector3 targetPoint)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, cookSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
        {
            if (defaultDir)
            {
                if (index == curPath.childCount - 1)
                {
                    defaultDir = false;
                    SwitchStatus();
                    return;
                }
                index++;
            }
            else
            {
                if (index == 0)
                {
                    defaultDir = true;
                    SwitchStatus();
                    return;
                }
                index--;
            }
            this.targetPoint = GetPathPoint(index);
            SetCookDir();
        }
    }

    private void SwitchStatus()
    {
        switch (cook)
        {
            case CookStatus.Work:
                cook = CookStatus.WorkToMove_1;
                break;
            case CookStatus.MoveTOWork:
                cook = CookStatus.Work;
                break;
            case CookStatus.WorkToMove:
                cook = CookStatus.Idle;
                break;
            case CookStatus.Idle:
                cook = CookStatus.MoveTOWork;
                break;
            case CookStatus.Idle_1:
                cook = CookStatus.Idle;
                break;
            case CookStatus.WorkToMove_1:
                cook = CookStatus.Idle;
                break;
            case CookStatus.MoveToWork_1:
                cook = CookStatus.WorkToMove_1;
                break;
        }
        SetCookIsMove();
        SetCookBehaviour();
    }

    private void SetCookIsMove()
    {
        if (cook == CookStatus.Idle || cook == CookStatus.Idle_1 || cook == CookStatus.Work)
        {
            isMove = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            isMove = true;
        }
    }

    private void DiffAnim(string name)
    {
        if (name.Contains("0"))
        {
            curIndex = 0;
        }
        else if (name.Contains("1"))
        {
            curIndex = 1;
        }
        sas[1 - curIndex].gameObject.SetActive(false);
        sas[curIndex].gameObject.SetActive(true);
        sas[curIndex].ClearState();
    }
    private void SetCookBehaviour()
    {
        string name = "0_breath";
        switch (cook)
        {
            case CookStatus.Work:
                name = "0_work";
                break;
            case CookStatus.MoveTOWork:
                name = "0_move";
                break;
            case CookStatus.WorkToMove:
                name = "0_move_meal";
                break;
            case CookStatus.Idle:
                name = "0_breath";
                break;
            case CookStatus.Idle_1:
                name = "1_breath";
                break;
            case CookStatus.MoveToWork_1:
                name = "1_move";
                break;
            case CookStatus.WorkToMove_1:
                name = "1_move_meal";
                break;
        }
        DiffAnim(name);
        PlayAnim(name);
        SetCookDir();
    }

    /// <summary>
    /// 设置cook的朝向
    /// </summary>
    private void SetCookDir()
    {
        dir = targetPoint - transform.position;
        if (dir.magnitude < 0.05f)
            return;
        SetNPCDirection(dir);
    }

    private void PlayAnim(string animName)
    {
        sas[curIndex].AnimationState.SetAnimation(0, animName, true);
    }

    private void RandomSwitchAnimation()
    {
        if (cook == CookStatus.Idle || cook == CookStatus.Idle_1)
        {
            int rd = Random.Range(0, 2);
            cook = rd == 0 ? CookStatus.Idle : CookStatus.Idle_1;
        }
    }

    public override void InitStaffSkill(int[] skillParam)
    {
        base.InitStaffSkill(skillParam);
    }


}
public enum CookStatus
{
    Work,
    MoveTOWork,
    WorkToMove,
    Idle,
    Idle_1,
    MoveToWork_1,
    WorkToMove_1
}