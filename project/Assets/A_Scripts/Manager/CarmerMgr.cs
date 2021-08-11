using System.Collections;
using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;

public class CarmerMgr : Singleton<CarmerMgr>
{
    private Transform m_MainCameraTrans;

    public Transform MainCameraTrans
    {
        get
        {
            if (m_MainCameraTrans == null)
            {
                m_MainCameraTrans = MainCamera.transform;
            }
            return m_MainCameraTrans;
        }
    }

    private Camera m_MainCamera;

    public Camera MainCamera
    {
        get
        {
            if (m_MainCamera == null)
            {
                m_MainCamera = Camera.main;
            }

            return m_MainCamera;
        }
    }

    private Transform m_UICameraTrans;

    public Transform UICameraTrans
    {
        get
        {
            if (m_UICameraTrans == null)
            {
                m_UICameraTrans = GameObject.Find("UICamera").transform;
            }

            return m_UICameraTrans;
        }
    }

    private Camera m_UICamera;

    public Camera UICamera
    {
        get
        {
            if (m_UICamera == null)
            {
                m_UICamera = UICameraTrans.GetComponent<Camera>();
            }

            return m_UICamera;
        }
    }
    private MobileTouchCamera m_mobileTouchCamera;

    public MobileTouchCamera MobileTouchCamer
    {
        get
        {
            if (m_mobileTouchCamera == null)
            {
                m_mobileTouchCamera = UICameraTrans.GetComponent<MobileTouchCamera>();
            }

            return m_mobileTouchCamera;
        }
    }

    //是否可以移动摄像机
    public void SetCamMoveCamera(bool canMove)
    {
        m_mobileTouchCamera.SetCamMoveCamera(canMove);
    }

    //摄像机移动限制 left固定-55 right没开liveroom之前是-14 开了之后是-5 top固定55 bottom根据解锁教室个数增加，最小值为 -35
}
