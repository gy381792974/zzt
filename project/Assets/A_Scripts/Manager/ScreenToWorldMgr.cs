using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//public class ScreenToWorldBind
//{
//    public Transform WorldObj_Trans;
//    public Transform UIObj_Trans;
//    public Vector2 Offset;

//    public ScreenToWorldBind(Transform worldObj_Trans, Transform uIObj_Trans , Vector2 offset)
//    {
//        WorldObj_Trans = worldObj_Trans;
//        UIObj_Trans = uIObj_Trans;
//        Offset = offset;
//    }
//}

public class ScreenToWorldMgr : Singleton<ScreenToWorldMgr>
{
    
#pragma warning disable 649

    [SerializeField]
    private Transform FollowParentTrans;
    private RectTransform FollowParentRectTrans;

#pragma warning restore 649
    //摄像机初始缩放
    private float CarmerOriginalOrthographicSize;
    //目标缩放
    private Vector3 TargetScale;
    
    void OnEnable()
    {
        FollowParentRectTrans = FollowParentTrans as RectTransform;
        CarmerOriginalOrthographicSize = CarmerMgr.Instance.MainCamera.orthographicSize;
        SetScale();
    }

    private void LateUpdate()
    {
        SetScale();
    }

    private float cameraRate = 1;
    private float canvasScale = 1.6f;

    private void SetScale()
    {
        cameraRate = canvasScale * CarmerOriginalOrthographicSize / CarmerMgr.Instance.MainCamera.orthographicSize;
        TargetScale.x = cameraRate;
        TargetScale.y = cameraRate;
        TargetScale.z = cameraRate;
        FollowParentTrans.localScale = TargetScale;
    }

    
    public void AddChild(Transform childTrans)
    {
        childTrans.SetParent(FollowParentTrans);
        childTrans.localScale = Vector3.one;
        childTrans.localPosition = Vector3.zero;
    }

    private Vector3 screenPoint;
    private Vector2 uiLocalPostion;
    public Vector2 GetUILocalPostion(Vector3 wolrdPos)
    {
        screenPoint = CarmerMgr.Instance.MainCamera.WorldToScreenPoint(wolrdPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(FollowParentRectTrans, screenPoint, CarmerMgr.Instance.UICamera, out uiLocalPostion);
        return uiLocalPostion;
    }
}
