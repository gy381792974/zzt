using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleNavigate : MonoBehaviour
{
    [Header("移动速度")]
    [SerializeField]
    private float MoveSpeed=5;

    [Header("是否更新旋转")]
    [SerializeField]
    private bool EnableRotation=true;

    [Header("旋转速度")]
    [SerializeField]
    private float RotationSpeed=0.5f;

    [Header("是否循环")]
    [SerializeField]
    private bool IsLoop;
    
    [Header("停止距离")]
    [SerializeField]
    private float StopDistanc = 0.1f;

    //当前路径点
    private List<Vector3> m_pointList=new List<Vector3>();

    //当前路径点的个数
    private int m_pointCount = 0;

    //当前路径列表下标
    private int m_curIndex = 0;
    
    //指定移动到当前路径的哪个点上
    private int m_endIndex= -1;

    private bool m_isMoving = false;
    public bool IsMoving
    {
        get
        {
            return m_isMoving;
        }
    }
    
    //下一个点的坐标
    private Vector3 m_nextPoint=Vector3.zero;
    //下一个点的期望朝向
    private Vector3 newRotatinForward;
    public void Update()
    {
        if (m_isMoving)
        {
            m_nextPoint = GetNextPoint();
            //科技树果实怎么增加移动速度 
            transform.position= Vector3.MoveTowards(transform.position, m_nextPoint, Time.deltaTime * MoveSpeed);

            if (EnableRotation)
            {
                newRotatinForward = m_nextPoint - transform.position;
                if (newRotatinForward != Vector3.zero)
                {
                    Vector3 targetForward = Vector3.Lerp(transform.forward, newRotatinForward, Time.deltaTime * RotationSpeed);
                    transform.forward = targetForward;
                }

            }
            
            float distance = Vector3.Distance(transform.position, m_nextPoint);
            
            if (distance < StopDistanc)
            {
                //到达目的地
                if (m_curIndex == m_endIndex)
                {
                    if (IsLoop)
                    {
                        m_curIndex = 0;
                    }
                    else
                    {
                        Stop();
                    }
                }
                else//到达一个节点
                {
                    m_curIndex++;
                }
            }
        }
    }

    /// <summary>
    /// 获取下一个移动点的坐标
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNextPoint()
    {
        if (m_pointCount < 1)
        {
            Debug.LogError("请先使用 SetPath() 去设置路径点！");
            return Vector3.zero;
        }
        if ( m_curIndex < m_pointCount)
        {
            if (m_endIndex != -1)
            {
                if (m_curIndex <= m_endIndex)
                {
                    return m_pointList[m_curIndex];
                }
            }
            else
            {
                return m_pointList[m_curIndex];
            }
        }
        else
        {
            if (IsLoop)
            {
                m_curIndex = 0;
                return m_pointList[m_curIndex];
            }
            else
            {
                m_isMoving = false;
                return m_pointList[m_curIndex-1];
            }
        }
        return m_pointList[0];
    }
    
    /// <summary>
    /// 开始移动
    /// </summary>
    public void StartMove()
    {
        m_isMoving = true;
    }
    
    /// <summary>
    /// 移动到指定下标
    /// </summary>
    /// <param name="index"></param>
    public void MoveToIndex(int index)
    {
        m_endIndex = index;
        m_isMoving = true;
    }

    public void MoveToPoint(Vector3 targetPos)
    {
        m_isMoving = false;
        m_curIndex = 0;
        m_endIndex = 0;
        m_pointList.Clear();
        m_pointList.Add(targetPos);
        m_pointCount = 1;
        m_isMoving = true;
    }

    public void MoveToPoint(List<Vector3> pointList)
    {
        m_isMoving = false;
        m_pointList.Clear();
        m_pointList.AddRange(pointList);
        m_pointCount = m_pointList.Count;
        m_endIndex = m_pointCount - 1;
        m_curIndex = 0;
        m_isMoving = true;
    }


    
    /// <summary>
    /// 停止移动
    /// </summary>
    public void Stop()
    {
        m_isMoving = false;
    }

    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetMoveSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    /// <summary>
    /// 设置是否循环
    /// </summary>
    /// <param name="loop"></param>
    public void SetLoop(bool loop)
    {
        IsLoop = loop;
    }

    public void SetEnableRotate(bool enableRotation)
    {
        EnableRotation = enableRotation;
    }
    
    /// <summary>
    /// 获取目标终点位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetPoint()
    {
        if (m_pointCount > 0)
        {
            return m_pointList[m_pointCount - 1];
        }
        Debug.LogError("请先使用 SetPath() 去设置路径点！");
       return Vector3.zero;
    }
    
    /// <summary>
    /// 获取移动方向
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDesiredVelocity()
    {
        return (GetNextPoint() - transform.position).normalized;
    }
}

