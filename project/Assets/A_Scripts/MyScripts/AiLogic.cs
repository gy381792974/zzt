using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BuildArea
{
    BA_CanTing,
    BA_Coffee,
    BA_Market,
    BA_QBar,
}

public class AiLogic : Singleton<AiLogic>
{
    private SimpleNavigate simpleNavigate;

    #region 场景信息

    #region 餐厅信息
    [Header("餐厅路径状态信息")]
    [SerializeField] private List<bool> pathStateList_CT;

    [Header("餐厅路径位置信息")]
    [SerializeField] private List<Vector3> pathPosList_CT;

    [Header("餐厅座椅状态信息")]
    [SerializeField] private List<bool> chairStateList_CT;

    [Header("餐厅座椅位置信息")]
    [SerializeField] private List<Vector3> chairPosList_CT;
    #endregion

    #region 咖啡信息
    [Header("咖啡路径状态信息")]
    [SerializeField] private List<bool> pathStateList_CF;

    [Header("咖啡路径位置信息")]
    [SerializeField] private List<Vector3> pathPosList_CF;

    [Header("咖啡座椅状态信息")]
    [SerializeField] private List<bool> chairStateList_CF;

    [Header("咖啡座椅位置信息")]
    [SerializeField] private List<Vector3> chairPosList_CF;
    #endregion

    #region 超市信息
    [Header("超市路径状态信息")]
    [SerializeField] private List<bool> pathStateList_MK;

    [Header("超市路径位置信息")]
    [SerializeField] private List<Vector3> pathPosList_MK;

    [Header("超市座椅状态信息")]
    [SerializeField] private List<bool> chairStateList_MK;

    [Header("超市座椅位置信息")]
    [SerializeField] private List<Vector3> chairPosList_MK;
    #endregion

    #region 酒吧信息
    [Header("酒吧路径状态信息")]
    [SerializeField] private List<bool> pathStateList_QB;

    [Header("酒吧路径位置信息")]
    [SerializeField] private List<Vector3> pathPosList_QB;

    [Header("酒吧座椅状态信息")]
    [SerializeField] private List<bool> chairStateList_QB;

    [Header("酒吧座椅位置信息")]
    [SerializeField] private List<Vector3> chairPosList_QB;
    #endregion

    #region 就餐信息
    [Header("就餐CD")]
    [SerializeField]
    private float eatTime = 3.0f;

    [Header("是否就餐结束")]
    [SerializeField]
    private bool isEatOver;
    #endregion

    #endregion

    [Header("是否排队完毕")]
    [SerializeField]
    public bool isQueueOver = false;
    public bool isQueueOver2 = false;
    public bool isQueueOver3 = false;
    public bool isQueueOver4 = false;

    private SimpleNavigate _SimpleNavigate
    {
        get
        {
            if (simpleNavigate == null)
            {
                simpleNavigate = GetComponent<SimpleNavigate>();
            }

            return simpleNavigate;
        }
    }

    private Queue<Action<AiLogic>> m_aiLogicActionQue = new Queue<Action<AiLogic>>();

    private void Start()
    {
        //当主场景生成AI的时候 默认走餐厅 先去排队
        MoveToTarget(CanTingPathMgr.Instance.GetCanTingMoveList(), x =>
        {
            QueueInCanTing();
        }
        );
    }

    private void Update()
    {
        if (isQueueOver == true)
        {
            StartCoroutine(Delay());
            isQueueOver = false;
        }
        if(isQueueOver2 == true)
        {
            StartCoroutine(Delay2());
            isQueueOver2 = false;
        }
        if (isQueueOver3 == true)
        {
            StartCoroutine(Delay3());
            isQueueOver3 = false;
        }
        if (isQueueOver4 == true)
        {
            StartCoroutine(Delay4());
            isQueueOver4 = false;
        }
        if (_SimpleNavigate.IsMoving)
        {
            //切换人物方向：面

            //Debug.Log("AI准备寻找座位");
            //aiCurrentPos = transform.position;//保存当前位置
   
        }
        else
        {
            if (queueCount > 0)
            {
                DeQueue();
            }
        }
    }

    public void MoveToTarget(List<Vector3> targetList, Action<AiLogic> action)
    {
        EnqueueAction(action);
        _SimpleNavigate.MoveToPoint(targetList);
    }
    public void MoveToTarget(Vector3 targetPos, Action<AiLogic> action)
    {
        EnqueueAction(action);
        _SimpleNavigate.MoveToPoint(targetPos);
    }

    private void EnqueueAction(Action<AiLogic> action)
    {
        if (action != null)
        {
            m_aiLogicActionQue.Enqueue(action);
            queueCount = m_aiLogicActionQue.Count;
        }
    }

    private int queueCount = 0;

    private void DeQueue()
    {
        Action<AiLogic> action = m_aiLogicActionQue.Dequeue();
        if (action != null)
        {
            action.Invoke(this);
            queueCount = m_aiLogicActionQue.Count;
        }
    }

    //去餐厅
    public void MoveToCanTing()
    {
        //餐厅
        chairStateList_CT = CanTingPathMgr.Instance.GetSitStateList();//餐厅座椅的状态信息
        chairPosList_CT = CanTingPathMgr.Instance.GetSitPosList();//餐厅座椅的位置信息

        int emptySetIndex = GetEmptySitIndex(chairStateList_CT);//获取餐厅空位下标  

        //如果没有空位置就去咖啡厅
        if (emptySetIndex == -1)
        {
            Debug.Log("餐厅没空位了 去咖啡厅了");
            //走 咖啡厅路径
            MoveToTarget(CoffeePathMgr.Instance.GetCoffeeMoveList(), x =>
            {
                //咖啡排队路径
                QueueInCoffee();
            }
            );
            return;
        }
        chairStateList_CT[emptySetIndex] = true;
        MoveToTarget(chairPosList_CT[emptySetIndex], m =>
        {
            //有人坐下(修改座椅状态)    
            Debug.Log("坐下吃饭");
            //就餐
            StartCoroutine(EatWait(BuildArea.BA_CanTing, emptySetIndex));
        }
        );

    }

    //餐厅排队
    public void QueueInCanTing()
    {
        //餐厅路径状态
        pathStateList_CT = CanTingPathMgr.Instance.GetCanTingPathSate();//获取路径点状态
        pathPosList_CT = CanTingPathMgr.Instance.GetCanTingPosList();//获取排队路径点位置
   
        int emptySetIndex = GetEmptyPathIndex(pathStateList_CT);//获取餐厅路径空位下标

        if (emptySetIndex == -1)
        {
            return;
        }

        pathStateList_CT[emptySetIndex] = true;

        if (emptySetIndex < pathPosList_CT.Count)
        {
            //if(pathStateList_CT[1]==true)
            //{
                MoveToTarget(pathPosList_CT[0], m =>
                {
                    MoveToTarget(pathPosList_CT[emptySetIndex], x =>
                    {
                        //第一个点设置为没有人
                        pathStateList_CT[0] = false;
                        pathStateList_CT[emptySetIndex] = false;
                        //排队结束
                        isQueueOver = true;
                        //关闭协程
                        StopCoroutine(Delay());
                    }
                    );
                }
                );
            //}

        }
    }

    //返回空位置下标 需要传一个椅子状态容器（每个房间都会有一个椅子状态容器）
    private int GetEmptySitIndex(List<bool> _chairStateList)
    {
        for (int i = 0; i < _chairStateList.Count; i++)
        {
            if (!_chairStateList[i])
            {
                return i;
            }
        }
        return -1;
    }

    //返回空位置下标 需要传一个路径状态容器（每个房间都会有一个路径状态容器）
    private int GetEmptyPathIndex(List<bool> _pathStateList)
    {
        for (int i = _pathStateList.Count-1; i>=0 ; i--)
        {
            if (!_pathStateList[i])
            {
                return i;
            }
        }
        return -1;
    }

    //咖啡排队
    private void QueueInCoffee()
    {
        //咖啡路径状态
        pathStateList_CF = CoffeePathMgr.Instance.GetCanTingPathSate();//获取路径点状态
        pathPosList_CF = CoffeePathMgr.Instance.GetCanTingPosList();//获取排队路径点位置

        int emptySetIndex = GetEmptyPathIndex(pathStateList_CF);//获取咖啡路径空位下标

        if (emptySetIndex == -1)
        {
            return;
        }

        pathStateList_CF[emptySetIndex] = true;

        if (emptySetIndex < pathPosList_CF.Count)
        {
            MoveToTarget(pathPosList_CF[0], m =>
            {
                MoveToTarget(pathPosList_CF[emptySetIndex], x =>
                {
                    //第一个点设置为没有人
                    pathStateList_CF[0] = false;
                    pathStateList_CF[emptySetIndex] = false;
                    isQueueOver2 = true;
                    //关闭协程
                    StopCoroutine(Delay2());
                }
                );
            }
            );
        }
    }

    //去往咖啡厅
    private void MoveToCoffee()
    {
        //咖啡厅
        chairStateList_CF = CoffeePathMgr.Instance.GetSitStateList();//餐厅座椅的状态信息
        chairPosList_CF = CoffeePathMgr.Instance.GetSitPosList();//餐厅座椅的位置信息

        int emptySetIndex = GetEmptySitIndex(chairStateList_CF);//获取咖啡厅空位下标

        if (emptySetIndex == -1)
        {
            //咖啡厅没位置 去超市
            Debug.Log("咖啡厅没有空位 去超市了");
            //走 超市路径
            MoveToTarget(MarketPathMgr.Instance.GetMarketMoveList(), x =>
            {
                //咖啡排队路径
                QueueInMarket();
            }
            );
            return;
        }
        chairStateList_CF[emptySetIndex] = true;
        MoveToTarget(chairPosList_CF[emptySetIndex], m =>
        {
            //有人坐下(修改座椅状态)    
            Debug.Log("坐下喝咖啡");
            //就餐
            StartCoroutine(EatWait(BuildArea.BA_Coffee, emptySetIndex));
        }
        );

    }

    //超市排队
    private void QueueInMarket()
    {
        //超市路径状态
        pathStateList_MK = MarketPathMgr.Instance.GetMarketPathSate();//获取路径点状态
        pathPosList_MK = MarketPathMgr.Instance.GetMarketPosList();//获取排队路径点位置

        int emptySetIndex = GetEmptyPathIndex(pathStateList_MK);//获取餐厅路径空位下标

        if (emptySetIndex == -1)
        {
            return;
        }

        pathStateList_MK[emptySetIndex] = true;

        if (emptySetIndex < pathPosList_MK.Count)
        {
            MoveToTarget(pathPosList_MK[0], m =>
            {
                MoveToTarget(pathPosList_MK[emptySetIndex], x =>
                {
                    //第一个点设置为没有人
                    pathStateList_MK[0] = false;
                    pathStateList_MK[emptySetIndex] = false;
                    isQueueOver3 = true;
                    StopCoroutine(Delay3());
                }
                );
            }
            );
        }
    }

    //去往超市
    private void MoveToMarket()
    {
        //超市
        chairStateList_MK = MarketPathMgr.Instance.GetSitStateList();//超市座椅的状态信息
        chairPosList_MK = MarketPathMgr.Instance.GetSitPosList();//超市座椅的位置信息

        int emptySetIndex = GetEmptySitIndex(chairStateList_MK);//获取超市空位下标

        if (emptySetIndex == -1)
        {
            //超市没位置 去酒吧
            Debug.Log("超市没有空位");
            MoveToTarget(QBarPathMgr.Instance.GetBarMoveList(), x =>
            {
                QueueInQBar();
            });
            return;
        }
        chairStateList_MK[emptySetIndex] = true;
        MoveToTarget(chairPosList_MK[emptySetIndex], m =>
        {
            //有人坐下(修改座椅状态)    
            Debug.Log("坐下吃零食");
            //就餐
            StartCoroutine(EatWait(BuildArea.BA_Market, emptySetIndex));
        }
        );
    }

    //酒吧排队
    private void QueueInQBar()
    {
        //酒吧路径状态
        pathStateList_QB = QBarPathMgr.Instance.GetQBarPathSate();//获取路径点状态
        pathPosList_QB = QBarPathMgr.Instance.GetQBarPosList();//获取排队路径点位置

        int emptySetIndex = GetEmptyPathIndex(pathStateList_QB);//获取酒吧路径空位下标

        if (emptySetIndex == -1)
        {
            return;
        }

        pathStateList_QB[emptySetIndex] = true;

        if (emptySetIndex < pathPosList_QB.Count)
        {
            MoveToTarget(pathPosList_QB[0], m =>
            {
                MoveToTarget(pathPosList_QB[emptySetIndex], x =>
                {
                    //第一个点设置为没有人
                    pathStateList_QB[0] = false;
                    pathStateList_QB[emptySetIndex] = false;
                    isQueueOver4 = true;
                    StopCoroutine(Delay4());
                }
                );
            }
            );
        }
    }

    //去往酒吧
    private void MoveToQBar()
    {
        //酒吧
        chairStateList_QB = QBarPathMgr.Instance.GetSitStateList();//酒吧座椅的状态信息
        chairPosList_QB = QBarPathMgr.Instance.GetSitPosList();//酒吧座椅的位置信息

        int emptySetIndex = GetEmptySitIndex(chairStateList_QB);//获取酒吧空位下标

        if (emptySetIndex == -1)
        {
            //酒吧没位置 销毁
            Debug.Log("酒吧没有空位 无路可走");
            Destroy(this, 0.5f);
            return;
        }
        chairStateList_QB[emptySetIndex] = true;
        MoveToTarget(chairPosList_QB[emptySetIndex], m =>
        {
            //有人坐下(修改座椅状态)    
            Debug.Log("坐下喝酒");
            Destroy(this, 10f);
        }
        );
    }


    //就餐
    private void HaveAMeal(BuildArea _buildArea , int _num)
    {
        //就餐结束
        switch(_buildArea)
        {
            case BuildArea.BA_CanTing:
                {
                    chairStateList_CT[_num] = false;
                    GoToNextRoom(_buildArea);
                }
                break;
            case BuildArea.BA_Coffee:
                {
                    chairStateList_CF[_num] = false;
                    GoToNextRoom(_buildArea);
                }
                break;
            case BuildArea.BA_Market:
                {
                    chairStateList_MK[_num] = false;
                    GoToNextRoom(_buildArea);
                }
                break;
            case BuildArea.BA_QBar:
                {
                    Destroy(this);
                }
                break;
        }
        
    }

    //传一个建筑区域 去下一个房间 
    private void GoToNextRoom(BuildArea _buildArea)
    {
        switch(_buildArea)
        {
            case BuildArea.BA_CanTing:
                {
                    MoveToTarget(CoffeePathMgr.Instance.GetCoffeeMoveList(), x =>
                    {
                        QueueInCoffee();
                    }
                    );
                }
                break;
            case BuildArea.BA_Coffee:
                {
                    MoveToTarget(MarketPathMgr.Instance.GetMarketMoveList(), x =>
                    {
                        QueueInMarket();
                    }
                    );
                }
                break;
            case BuildArea.BA_Market:
                {
                    MoveToTarget(QBarPathMgr.Instance.GetBarMoveList(), x =>
                    {
                        QueueInQBar();
                    }
                    );
                }
                break;
            case BuildArea.BA_QBar:
                {
                    //没空位 闲逛 或者 销毁
                }
                break;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        MoveToCanTing();
    }

    IEnumerator Delay2()
    {
        yield return new WaitForSeconds(1f);
        MoveToCoffee();
    }

    IEnumerator Delay3()
    {
        yield return new WaitForSeconds(1f);
        MoveToMarket();
    }

    IEnumerator Delay4()
    {
        yield return new WaitForSeconds(1f);
        MoveToQBar();
    }

    IEnumerator EatWait(BuildArea _type,int num)
    {
        yield return new WaitForSeconds(10f);

        HaveAMeal(_type,num);
    }
}
