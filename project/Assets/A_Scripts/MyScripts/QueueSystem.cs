using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueSystem : Singleton<QueueSystem>
{
    #region 餐厅排队容器
    [Header("餐厅排队点状态容器")]
    [SerializeField]
    private List<bool> cantingQueueState = new List<bool>();

    [Header("餐厅排队点位置容器")]
    [SerializeField]
    private Queue<Vector3> cantingQueuePos = new Queue<Vector3>();
    #endregion

    #region 咖啡厅排队容器
    [Header("咖啡厅排队点状态容器")]
    [SerializeField]
    private List<bool> coffeeQueueState = new List<bool>();

    [Header("咖啡厅排队点位置容器")]
    [SerializeField]
    private Queue<Vector3> coffeeQueuePos= new Queue<Vector3>();
    #endregion

    #region 超市排队容器
    [Header("超市排队点状态容器")]
    [SerializeField]
    private List<bool> marketQueueState = new List<bool>();

    [Header("超市排队点位置容器")]
    [SerializeField]
    private Queue<Vector3> marketQueuePos = new Queue<Vector3>();
    #endregion

    #region 酒吧排队容器
    [Header("酒吧排队点状态容器")]
    [SerializeField]
    private List<bool> qbarStateQueue = new List<bool>();

    [Header("酒吧排队点位置容器")]
    [SerializeField]
    private Queue<Vector3> qbarPathQueue = new Queue<Vector3>();
    #endregion

    [Header("排队时间")]
    [SerializeField]
    private float lineUpTime = 3.0f;

    [Header("排队状态")]
    [SerializeField]
    private bool isOver = false;

    //入队
    public void LineUp(Transform trans, BuildArea _type)
    {
        switch (_type)
        {
            case BuildArea.BA_CanTing:
                {
                    cantingQueuePos.Enqueue(trans.position);
                }
                break;
            case BuildArea.BA_Coffee:
                {
                    coffeeQueuePos.Enqueue(trans.position);
                }
                break;
            case BuildArea.BA_Market:
                {
                    marketQueuePos.Enqueue(trans.position);
                }
                break;
            case BuildArea.BA_QBar:
                {
                    qbarPathQueue.Enqueue(trans.position);
                }
                break;
        }
    }

    //出队
    public void OutTheLine(BuildArea _type)
    {
        switch (_type)
        {
            case BuildArea.BA_CanTing:
                {
                    cantingQueuePos.Dequeue();
                }
                break;
            case BuildArea.BA_Coffee:
                {
                    coffeeQueuePos.Dequeue();
                }
                break;
            case BuildArea.BA_Market:
                {
                    marketQueuePos.Dequeue();
                }
                break;
            case BuildArea.BA_QBar:
                {
                    qbarPathQueue.Dequeue();
                }
                break;
        }

    }

    //清除排队系统
    public void ClearQueueSystem(BuildArea _type)
    {
        switch (_type)
        {
            case BuildArea.BA_CanTing:
                {
                    cantingQueuePos.Clear();
                }
                break;
            case BuildArea.BA_Coffee:
                {
                    coffeeQueuePos.Clear();
                }
                break;
            case BuildArea.BA_Market:
                {
                    marketQueuePos.Clear();

                }
                break;
            case BuildArea.BA_QBar:
                {
                    qbarPathQueue.Clear();
                }
                break;
        }
    }

    //Vip插队
    public void VipJumpQueue()
    {

    }
}