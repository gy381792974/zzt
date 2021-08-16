using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EazyGF
{
    public enum NCSuatus
    {
        Null = -1,
        Pedestrian,     //行人
        Enter = 1,
        TakeMeal,
        TrayMakeMeal = 3, //购买食物前 刚取完餐托盘的样子
        BuyFood,
        HangOut,
        EnterDiningQuque,//用餐排队
        DiningQuque = 7,//用餐排队
        Dining, //用餐
        Leave, //离开
    }

    [Serializable]
    public class StallQueue
    {
        public List<Transform> tfs;
    }

    [Serializable]
    public class PosLevelQueue
    {
        public List<StallQueue> sqs;
    }

    //等级位置排队序列
    [Serializable]
    public class LevelPMQ
    {
        public List<PosMultiQueue> datas;
    }

    [Serializable]
    public class PosMultiQueue
    {
        public List<PosQueue> datas;
    }


    [Serializable]
    public class PosQueue
    {
        public List<Transform> tfs;
        //public Vector3[] tf;
    }

    [Serializable]
    public class MNavigate
    {
        public BFSNode[] bfs;
        
    }

    public class CustomerMgr : MonoBehaviour
    {
        List<CustomerNor> unlockNCs = new List<CustomerNor>();

        public Transform bornPos;
        public List<Transform> nCEnter;
        public List<Transform> pedestrianTfs;

        public List<Transform> takeMealDirTf;
        public List<PosQueue> nCTakeMeal;
        public List<Transform> nCTrayMakeMeal;

        public List<Transform> stallDirTf;

        public List<PosLevelQueue> nCStallTf;
        public List<PosLevelQueue> nCStallGetFoodPos;

        public List<PosLevelQueue> nCHangOut;

        public List<StallQueue> dcEnterQuqueTf;
        public Transform enterQuequePoint;
        public List<Transform> dcQuqueTf;

        //public List<Transform> diningDir;
        public List<PosQueue> nCEnterDining;
        public List<Transform> nCDining;

        public List<PosQueue> leaveTf;

        public List<Transform> restaurantSeat;

        public BFSNode[] bfs;
        public BFSNode[] pedestrianBfs;

        public MNavigate[] mNavigate; //0 普通用餐区

        int nCTakeMealNum = 0;//排队拿盘
        Dictionary<int, int> queueDic = new Dictionary<int, int>(); //排队
        List<CustomerNormal_Property> readyUnLockCNs;

        //正在吃饭的位置
        List<int> diningIndexs = new List<int>();
        int[] dinOccupyPosArr;

        //0 二楼包间 1 一楼露台 2 吧台 3 一楼普通用餐(3靠近一楼包间， 4靠近露台的)
        int[] diningAreaIds = new int[4] { 22003, 22004, 12007, 22001 };

        public List<PosQueue> diningSeats;
        public List<PosQueue> diningDir;

        public List<Transform> buyBoodQueue;

        public List<LevelPMQ> stallMultiQueue;

        Dictionary<int, int[]> eStallQueue = new Dictionary<int, int[]>(); //排队

        bool isSingleTeam = true; //是否使用单队伍 
        bool isReadTable = false;

        bool isTargetIndex = false;  //是否使用目标客户
        public static int faseCreateNum = -1; //调试 测试人数
        public static int targetIndex = -1;

        //得到最少人数的队伍index 以及队伍排队的人数 
        public int[] GetStallInfos(int targetStallId, int teamNum)
        {
            int num = 0;
            int teamIndex = 0;

            //扩充队伍
            if (eStallQueue.TryGetValue(targetStallId, out int[] value))
            {
                if (teamNum > value.Length)
                {
                    int[] nValue = new int[teamNum];

                    for (int i = 0; i < nValue.Length; i++)
                    {
                        if (i < value.Length)
                        {
                            nValue[i] = value[i];
                        }
                        else
                        {
                            nValue[i] = 0;
                        }
                    }

                    eStallQueue[targetStallId] = nValue;

                    return new int[] { value.Length, 0 };
                }

                teamIndex = 0;
                num = value[0];
                for (int i = 0; i < teamNum; i++)
                {
                    if (num > value[i])
                    {
                        num = value[i];
                        teamIndex = i;
                    }
                }
            }
            else
            {
                int[] nValue = new int[teamNum];

                for (int i = 0; i < nValue.Length; i++)
                {
                    nValue[i] = 0;
                }

                eStallQueue.Add(targetStallId, nValue);
            }

            return new int[] { teamIndex, num };
        }

        public int GetNCNum()
        {
            return unlockNCs.Count;
        }

        private void Start()
        {
            //GlobeFunction.AddGold += NCCreateCus;

            readyUnLockCNs = CustomerLogic.ReadyUnLockCNs;

            createNewNcTime = Random.Range(3, 15);

            dinOccupyPosArr = new int[diningSeats[4].tfs.Count];

            for (int i = 0; i < dinOccupyPosArr.Length; i++)
            {
                dinOccupyPosArr[i] = 0;
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.RegisterEvent(EventKey.NCDiningEnd, DiningQueueEndEvent);

            EventManager.Instance.RegisterEvent(EventKey.DancingGameVictory, DancingGirlBuffer);

            EventManager.Instance.RegisterEvent(EventKey.FastCreateCusBuff, FastCreateCusBuffEvent);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListening(EventKey.NCDiningEnd, DiningQueueEndEvent);

            EventManager.Instance.RemoveListening(EventKey.DancingGameVictory, DancingGirlBuffer);

            EventManager.Instance.RemoveListening(EventKey.FastCreateCusBuff, FastCreateCusBuffEvent);
        }

        private void DancingGirlBuffer(object obj)
        {
            dlBufferTimer = (int)obj;
            if (dlBufferTimer > 0)
            {
                createNcTime = 1;
            }
        }

        private void FastCreateCusBuffEvent(object obj)
        {
            fCNum = (int)obj;
        }

        #region 顾客用餐流程

        //创建预备解锁的顾客
        public void NCReadyUnLockCreate()
        {
            LoadCus(readyUnLockCNs[0], true);
            readyUnLockCNs.Remove(readyUnLockCNs[0]);
        }

        //生成顾客
        public void NCCreateCus()
        {
            List<CustomerNormal_Property> UnLockCns = CustomerLogic.unlockCNs;
            if (UnLockCns.Count > 0)
            {
                int index = CustomerLogic.GetNCCreateIndex();

                if (targetIndex >= 0)
                {
                    int i = GetTarCusIndexByid(targetIndex);
                    if (i >= 0)
                    {
                        index = i;
                    }
                }
                else
                {
                    index = CustomerLogic.GetNCCreateIndex();
                }

                LoadCus(UnLockCns[index], false);
            }
        }

        public int GetTarCusIndexByid(int index)
        {
            List<CustomerNormal_Property> unLockCns =  CustomerLogic.unlockCNs;
            for (int i = 0; i < unLockCns.Count; i++)
            {
                if (unLockCns[i].ID == index)
                {
                    return i;
                }
            }

            return -1;
        }

        private void LoadCus(CustomerNormal_Property pro, bool isFirst)
        {
            CustomerNor cn = AssetMgr.Instance.LoadGameobjFromPool(pro.Path).GetComponent<CustomerNor>();
            cn.ResetData();

            cn.transform.position = bornPos.position;

            cn.onHandle2 += NCGoToTargetPosHandle;

            cn.BindData(pro);
            cn.getFoodNum = 1;
            cn.isFirst = isFirst;

            cn.SetRole();

            int index = GetOccupyDic((int)NCSuatus.Pedestrian, pedestrianTfs.Count);

            if (index != -1)
            {
                float rote = Random.Range(1, 100);

                int diners = GetCurDiners();
                int seats = GetAllUnlockSeat();

                if (rote <= ((float)diners /seats) * 100)
                {
                    //NCPedestrian(cn, index);

                    NCPedestrian(cn, index);
                }
                else
                {
                    NCEnter(cn);
                }
            }
            else
            {
                NCEnter(cn);
            }

            unlockNCs.Add(cn);
        }

        //变成行人
        public void NCPedestrian(CustomerNor cn, int index)
        {
            cn.SetCurState(NCSuatus.Pedestrian);
            cn.lineIndex = 0;

            //List<Vector3> mPath = BFS_Mgr.GetMoveList(pedestrianBfs, cn.transform, pedestrianTfs[index]);

            occupyDic[(int)NCSuatus.Pedestrian][index] = 1;
            cn.MoveToTargetPoint(pedestrianBfs[0].Pos);
            cn.MoveToTargetPoint(pedestrianBfs[1].Pos);
            cn.MoveToTargetPoint(pedestrianTfs[index]);
            cn.QueueIndex = index;
        }   

        //变成顾客
        public void NCPedToCus()
        {
            CustomerNor cn = null;
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                if (unlockNCs[i].curState == NCSuatus.Pedestrian && unlockNCs[i].lineIndex == 1)
                {
                    cn = unlockNCs[i];

                    break;
                }
            }

            if (cn != null)
            {
                cn.SetCurState(NCSuatus.Enter);

                occupyDic[(int)NCSuatus.Pedestrian][cn.QueueIndex] = 0;

                cn.MoveToTargetPoint(pedestrianBfs[1].Pos);
                cn.MoveToTargetPoint(pedestrianBfs[0].Pos);
                NCEnter(cn);
            }
        }


        //进入顾客
        public void NCEnter(CustomerNor cn)
        {
            cn.SetCurState(NCSuatus.Enter);
            cn.MoveToTargetPoint(nCEnter);
        }

        public void NCEnterEnd(CustomerNor cn)
        {
            NCTackMead(cn);
        }

        //取餐
        public void NCTackMead(CustomerNor cn)
        {
            //if (nCTakeMealNum < nCTakeMeal.Count)
            //{
            //    cn.SetCurState(NCSuatus.TakeMeal);

            //    cn.DirPos = takeMealDirTf.position;
            //    cn.QueueIndex = nCTakeMealNum;
            //    cn.MoveToTargetPoint(new List<Transform>() { nCTakeMeal[nCTakeMealNum] });

            //    nCTakeMealNum++;

            //    Debug.LogWarning($"nCTakeMealNum {nCTakeMealNum}  : ");
            //    //cn.MoveToTargetPoint(nCTakeMeal, true, nCTakeMealNum);
            //}
            //else
            //{
            //    //TODO
            //    unlockNCs.Remove(cn);
            //    PoolMgr.Instance.DespawnOne(cn.transform);

            //    //GameObject.Destroy(cn.gameObject);
            //}

            int key = (int)NCSuatus.TakeMeal - 100;

            int num1 = GetQueueIndex(key);
            int num2 = GetQueueIndex(key + 1);

            int num = 0;
            cn.lineIndex = 0;

            if (num1 <= num2)
            {
                num = num1;
                cn.lineIndex = 0;
            }
            else
            {
                num = num2;
                cn.lineIndex = 1;
            }

            if (num < nCTakeMeal[cn.lineIndex].tfs.Count)
            {
                cn.SetCurState(NCSuatus.TakeMeal);

                cn.DirPos = takeMealDirTf[cn.lineIndex].position;
                cn.QueueIndex = num;
                cn.MoveToTargetPoint(nCTakeMeal[cn.lineIndex].tfs, true, num);

                queueDic[key + cn.lineIndex]++;
            }
            else
            {
                unlockNCs.Remove(cn);
                PoolMgr.Instance.DespawnOne(cn.transform);
            }
        }

        //取餐位置为0结束
        public IEnumerator NCTakeMealEnd(CustomerNor cn)
        {
            CustomerLogic.AddUnLockCN(cn.Data);

            int watitTime = 2;
            LocalCommonUtil.ShowBB(3, cn.transform, cn.GetHashCode(), watitTime);
            yield return new WaitForSeconds(watitTime);

            int key = (int)NCSuatus.TakeMeal - 100;

            queueDic[key + cn.lineIndex]--;

            NCTrayMakeMeal(cn);

            //取餐队伍跟进
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor mCn = unlockNCs[i];

                if (mCn.curState == NCSuatus.TakeMeal && cn.lineIndex == mCn.lineIndex)
                {
                    mCn.QueueIndex = mCn.QueueIndex - 1;

                    //Debug.LogError(" index： " + i + "    "+ mCn.QueueIndex + "  hashCode " + mCn.GetHashCode());

                    if (mCn.QueueIndex >= 0)
                    {
                        mCn.MoveToTargetPoint(nCTakeMeal[mCn.lineIndex].tfs[mCn.QueueIndex]);
                    }
                }
            }
        }

        //准备食物购买
        public void NCTrayMakeMeal(CustomerNor cn)
        {
            cn.SetCurState(NCSuatus.TrayMakeMeal);
            cn.MoveToTargetPoint(nCTrayMakeMeal[cn.lineIndex]);
        }

        //准备食物结束
        public void NCTrayMakeMealEnd(CustomerNor cn)
        {
            if (isSingleTeam)
            {
                NCEnterBuyFood(cn);
            }
            else
            {
                MNCEnterBuyFood(cn);
            }
        }

        #region 一条队伍多个取餐点

        //第一阶段 寻路到要排队的点
        public void NCEnterBuyFood(CustomerNor cn, bool isCal = true)
        {
            int stallID = 0;

            if (isCal)
            {
                if (cn.Data.MustGoStall != -1 && cn.GetBuyFoodNum() == 0)
                {
                    stallID = cn.Data.MustGoStall;
                }
                else
                {
                    List<int> ids = BuildMgr.GetRangUnlockStall();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        stallID = ids[i];

                        if (stallID == -1)
                        {
                            continue;
                        }
                        else if (!cn.IsHaveBeenStall(stallID))
                        {
                            break;
                        }
                        else
                        {
                            stallID = -1;
                            break;
                        }
                    }

                    if (stallID == -1)
                    {
                        Debug.LogWarning("找不到可以排队的摊位");

                        //RemoveCn(cn);

                        NCHangOut(cn);

                        return;
                    }
                }

                int posIndex = CustomerLogic.GetStallIndexById(stallID);
                int level = BuildMgr.GetUserBuildLevelById(stallID);

                cn.targetStallId = stallID;
                cn.posIndex = posIndex;
                cn.level = level;
            }

            if (cn.level <= 0)
            {
                Debug.LogWarning("找不到可以排队的摊位");

                //RemoveCn(cn);

                NCHangOut(cn);

                return;
            }

            cn.SetCurState(NCSuatus.BuyFood);

            PosLevelQueue pq = nCStallTf[cn.level - 1];

            StallQueue stallQueue = pq.sqs[cn.posIndex];

            cn.QueueIndex = stallQueue.tfs.Count - 1;
            cn.lineIndex = 1;

            List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, stallQueue.tfs[stallQueue.tfs.Count - 1]);
            cn.MoveToTargetPoint(mPath);
        }


        // 第二阶段 开始排队
        public void NCBuyFoodQueue(CustomerNor cn)
        {
            cn.SetCurState(NCSuatus.BuyFood);
            cn.lineIndex = 2;

            int queueNum = GetCanQueueIndex(cn);

            if (queueNum == -2)
            {
                NCBuyFoodGetFood(cn, true);
                return;
            }

            if (queueNum != -1)
            {
                cn.QueueIndex = queueNum;
                cn.DirPos = stallDirTf[cn.posIndex].position;

                PosLevelQueue pq = nCStallTf[cn.level - 1];
                StallQueue stallQueue = pq.sqs[cn.posIndex];

                cn.MoveToTargetPoint(stallQueue.tfs[queueNum]);

                queueNum++;

                queueDic[cn.targetStallId] = queueNum;
            }
            else
            {
                NCHangOut(cn);
            }
        }

        //得到购买食物能排队的index
        public int GetCanQueueIndex(CustomerNor cn, int stallId = -1)
        {
            int posIndex = cn.posIndex;
            int level = cn.level;
            int targetId = cn.targetStallId;

            if (stallId != -1)
            {
                targetId = stallId;
                posIndex = CustomerLogic.GetStallIndexById(stallId);
                level = BuildMgr.GetUserBuildLevelById(stallId);
            }

            PosLevelQueue pq = nCStallTf[level - 1];
            StallQueue stallQueue = pq.sqs[posIndex];

            int queueNum = GetQueueIndex(targetId);

            int totalCanQueueNum = BuildMgr.GetCurCanQueueNum(targetId);

            if (totalCanQueueNum > stallQueue.tfs.Count)
            {
                totalCanQueueNum = stallQueue.tfs.Count;
            }

            totalCanQueueNum = 0;

            if (totalCanQueueNum == 0)
            {
                cn.targetStallId = targetId;
                cn.posIndex = posIndex;
                cn.level = level;

                return -2;
            }

            if (queueNum < totalCanQueueNum)
            {
                cn.targetStallId = targetId;
                cn.posIndex = posIndex;
                cn.level = level;

                return queueNum;
            }

            return -1;
        }

        //第三阶段 排队结束 去取食物的点
        public void NCBuyFoodGetFood(CustomerNor cn, bool isNoHaveQueue = false)
        {
            if (cn.QueueIndex == 0 || isNoHaveQueue)
            {
                List<Transform> tfs = nCStallGetFoodPos[cn.level - 1].sqs[cn.posIndex].tfs;

                int num = BuildMgr.GetCurTeamNumByStallId(cn.targetStallId);

                if (num > tfs.Count)
                {
                    Debug.LogWarning($"购买食物的点的数量超过配置的人数{num}  > {tfs.Count}");
                    num = tfs.Count;
                }

                int index = GetOccupyDic(cn.targetStallId, num, true);

                if (index != -1)
                {
                    cn.QueueIndex = index;
                    cn.lineIndex = 3;

                    queueDic[cn.targetStallId]--;
                    int targetStallId = cn.targetStallId;

                    cn.DirPos = stallDirTf[cn.posIndex].position;

                    //摊位队伍跟进
                    BuyFoodQuequeGJ(targetStallId);

                    List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, tfs[index]);
                    cn.MoveToTargetPoint(mPath);
                    cn.MoveToTargetPoint(tfs[index]);
                }
                else if(isNoHaveQueue)
                {
                    NCHangOut(cn);
                }
            }
        }

        //购买食物结束
        public void NCBuyFoodEnd(CustomerNor cn)
        {
            StartCoroutine(BuyFoodEndIE(cn));
        }

        public IEnumerator BuyFoodEndIE(CustomerNor cn)
        {
            EventManager.Instance.TriggerEvent(EventKey.EnterGetFoodPos, cn.targetStallId);

            CustomerBubbleMgr.Instance.TargetBubble(cn);

            int stayTime = BuildMgr.GetUserBuildStayTimeById(cn.targetStallId);

            yield return new WaitForSeconds(2);

            SetBuyFoodEndData(cn);

            occupyDic[cn.targetStallId][cn.QueueIndex] = 0;
            //摊位队伍跟进
            int targetStallId = cn.targetStallId;
            BuyFoodQuequeGJ(targetStallId, false);

            if (cn.IsGoOnBuyFood())
            {
                NCEnterBuyFood(cn);
            }
            else
            {
                cn.RemoveHaveBeenStall();

                SelectDiningArea(cn);

                NCHangOutEndEvent();
            }
        }

        private void BuyFoodQuequeGJ(int targetStallId, bool isTraverse = true)
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor cn = unlockNCs[i];

                if (cn.curState == NCSuatus.BuyFood && targetStallId == cn.targetStallId && cn.lineIndex == 2)
                {
                    if (isTraverse)
                    {
                        if (cn.QueueIndex == 0)
                        {
                            NCBuyFoodGetFood(cn);
                        }
                        else
                        {
                            cn.QueueIndex--;

                            if (cn.QueueIndex >= 0)
                            {
                                int posIndex = cn.posIndex;
                                int level = cn.level;

                                Vector3 tarPos = nCStallTf[level - 1].sqs[posIndex].tfs[cn.QueueIndex].position;

                                //List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform.position, tarPos);

                                //cn.MoveToTargetPoint(mPath);
                                cn.MoveToTargetPoint(tarPos);
                            }
                        }
                    }
                    else
                    {
                        if (cn.QueueIndex == 0)
                        {
                            NCBuyFoodGetFood(cn);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region   多队伍多取餐点

        //第一阶段 寻路到要排队的点
        public void MNCEnterBuyFood(CustomerNor cn, bool isCal = true)
        {
            int stallID = 0;

            if (isCal)
            {
                cn.getFoodNum = 0;
                if (cn.Data.ID != 1 && cn.GetBuyFoodNum() == 0)
                {
                    stallID = cn.Data.MustGoStall;
                }
                else
                {
                    List<int> ids = BuildMgr.GetRangUnlockStall();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        stallID = ids[i];

                        if (stallID == -1)
                        {
                            continue;
                        }
                        else if (!cn.IsHaveBeenStall(stallID))
                        {
                            break;
                        }
                        else
                        {
                            stallID = -1;
                            break;
                        }
                    }
                }

                if (stallID == -1)
                {
                    Debug.LogWarning("找不到可以排队的摊位");

                    RemoveCn(cn);
                    return;
                }

                int posIndex = CustomerLogic.GetStallIndexById(stallID);
                int level = BuildMgr.GetUserBuildLevelById(stallID);

                cn.targetStallId = stallID;
                cn.posIndex = posIndex;
                cn.level = level;
            }


            if (cn.level <= 0)
            {
                Debug.LogWarning("找不到可以排队的摊位");

                RemoveCn(cn);
                return;
            }

            cn.SetCurState(NCSuatus.BuyFood);
            cn.lineIndex = 1;

            PosMultiQueue multiQueue = stallMultiQueue[cn.level - 1].datas[cn.posIndex];

            int[] datas = GetStallInfos(cn.targetStallId, multiQueue.datas.Count);
            cn.enterIndex = datas[0];

            List<Transform> tfs = multiQueue.datas[cn.enterIndex].tfs;

            List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, tfs[tfs.Count - 1]);
            cn.MoveToTargetPoint(mPath);
            //cn.MoveToTargetPoint(tfs[tfs.Count - 1]);
        }


        // 第二阶段 开始排队
        public void MNCBuyFoodQueue(CustomerNor cn)
        {
            cn.lineIndex = 2;

            cn.SetCurState(NCSuatus.BuyFood);
         
            cn.level = BuildMgr.GetUserBuildLevelById(cn.targetStallId);
            PosMultiQueue multiQueue = stallMultiQueue[cn.level - 1].datas[cn.posIndex];

            //int canQueueTeamNum = BuildMgr.GetCurTeamNumByStallId(cn.targetStallId);
            //int maxTeamNum = multiQueue.datas.Count;
            //if (canQueueTeamNum > maxTeamNum)
            //{
            //    canQueueTeamNum = maxTeamNum;
            //}
            //int[] datas = GetStallInfos(cn.targetStallId, canQueueTeamNum);

            //cn.enterIndex = datas[0]; //队伍的idnex
            //int queueNum = datas[1];

            //int maxQueueNum = tfs.Count;
            //int curCanQueueNum = BuildMgr.GetCurCanQueueNumStall(cn.targetStallId);
            //if (curCanQueueNum > maxQueueNum)
            //{
            //    curCanQueueNum = maxQueueNum;
            //}

            int[] data = GetStallQueueInfo(cn.targetStallId, cn.level - 1, cn.posIndex);

            if (data != null)
            {
                cn.enterIndex = data[0];
                cn.QueueIndex = data[1];

                List<Transform> tfs = multiQueue.datas[cn.enterIndex].tfs;
                cn.DirPos =  stallDirTf[cn.posIndex].position;

                eStallQueue[cn.targetStallId][cn.enterIndex]++;

                //List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, tfs[cn.QueueIndex]);
                //cn.MoveToTargetPoint(mPath);

                cn.MoveToTargetPoint(tfs[cn.QueueIndex]);
            }
            else
            {
                NCHangOut(cn);
            }
        }

        //购买食物结束
        public void MNCBuyFoodEnd(CustomerNor cn)
        {
            if (cn.QueueIndex == 0)
            {
                cn.lineIndex = 3;
                StartCoroutine(MBuyFoodEndIE(cn));
            }
        }

        public IEnumerator MBuyFoodEndIE(CustomerNor cn)
        {
            EventManager.Instance.TriggerEvent(EventKey.EnterGetFoodPos, cn.targetStallId);

            CustomerBubbleMgr.Instance.TargetBubble(cn);

            yield return new WaitForSeconds(BuildMgr.GetUserBuildStayTimeById(cn.targetStallId));

            SetBuyFoodEndData(cn);

            eStallQueue[cn.targetStallId][cn.enterIndex]--;

            //摊位队伍跟进
            int targetStallId = cn.targetStallId;
            MBuyFoodQuequeGJ(targetStallId, cn.enterIndex);

            if (cn.IsGoOnBuyFood())
            {
                MNCEnterBuyFood(cn);
            }
            else
            {
                cn.RemoveHaveBeenStall();

                SelectDiningArea(cn);

                NCHangOutEndEvent();
            }
        }

        private void MBuyFoodQuequeGJ(int targetStallId, int teamIndex)
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor cn = unlockNCs[i];

                if (cn.curState == NCSuatus.BuyFood && targetStallId == cn.targetStallId && cn.lineIndex == 2 && cn.enterIndex == teamIndex)
                {
                    if (cn.QueueIndex == 0)
                    {
                        MNCEnterBuyFood(cn);
                    }
                    else
                    {
                        cn.QueueIndex--;

                        if (cn.QueueIndex >= 0)
                        {
                            int posIndex = cn.posIndex;
                            int level = cn.level;

                            Vector3 tarPos = stallMultiQueue[level - 1].datas[posIndex].datas[cn.enterIndex].tfs[cn.QueueIndex].position;

                            //List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform.position, tarPos);

                            //cn.MoveToTargetPoint(mPath);
                            cn.MoveToTargetPoint(tarPos);
                        }
                    }
                }
            }
        }

        //0 队伍index  1队伍人数 返回null 就是没有team可以排队
        public int[] GetStallQueueInfo(int id, int level, int posIndex)
        {
            PosMultiQueue multiQueue = stallMultiQueue[level].datas[posIndex];

            int canQueueTeamNum = BuildMgr.GetCurTeamNumByStallId(id);
            int maxTeamNum = multiQueue.datas.Count;
            if (canQueueTeamNum > maxTeamNum)
            {
                //Debug.LogError("表中数据大于实际配置的表数据");
                canQueueTeamNum = maxTeamNum;
            }

            int[] datas = GetStallInfos(id, canQueueTeamNum);

            int teamIndex = datas[0]; //队伍的idnex
            int queueNum = datas[1];

            List<Transform> tfs = multiQueue.datas[teamIndex].tfs;

            int maxQueueNum = tfs.Count;
            int curCanQueueNum = BuildMgr.GetCurCanQueueNum(id);

            if (curCanQueueNum > maxQueueNum)
            {
                //Debug.LogError("表中数据大于实际配置的表数据");
                curCanQueueNum = maxQueueNum;
            }

            if (queueNum < curCanQueueNum)
            {
                return datas;
            }

            return null;
        }


        #endregion
        private void SetBuyFoodEndData(CustomerNor cn)
        {
            if (cn.ShowBBType == 0)
            {
                EventManager.Instance.TriggerEvent(EventKey.CanceLoopBubble, cn.transform);
            }

            EventManager.Instance.TriggerEvent(EventKey.LeaveGetGoodPos, cn.targetStallId);

            int foodPrice = BuildMgr.GetCurFoodPriceById(cn.targetStallId);

            int num = BuildCollectMgr.Instance.AddCoin(cn.targetStallId, foodPrice);

            if (num >= BuildCollectMgr.cCoinMinShowNum)
            {
                LocalCommonUtil.ShowBB(1, MainSpace.Instance.stallList[cn.posIndex].GetShowBuildBoxTf(),
                    cn.targetStallId, num);
            }

            cn.AddBaveStall(cn.targetStallId);

            CusFashionMgr.Instance.AddGTStallNum(cn.Data.ID, cn.targetStallId);

            int bxIndex = CusFashionMgr.Instance.CalFashionIndex(cn.Data.ID, cn.targetStallId, cn.CurBxIndex);

            cn.SetBX(bxIndex);
        }

        Dictionary<int, int[]> occupyDic = new Dictionary<int, int[]>();

        private int GetOccupyDic(int key, int num = 5, bool isOcSeat = false)
        {
            if (num <= 0)
            {
                return -1;
            }

            if (occupyDic.TryGetValue(key, out int[] value))
            {
                if (num > value.Length)
                {
                    int[] datas = new int[num];

                    for (int i = 0; i < value.Length; i++)
                    {
                        datas[i] = value[i];
                    }

                    if (isOcSeat)
                    {
                        datas[value.Length] = 1;
                    }

                    occupyDic[key] = datas;

                    return value.Length;
                }

                for (int i = 0; i < num; i++)
                {
                    if (value[i] == 0)
                    {
                        if (isOcSeat)
                        {
                            occupyDic[key][i] = 1;
                        }

                        return i;
                    }
                }

                return -1;
            }
            else
            {
                int[] datas = new int[num];

                for (int i = 0; i < datas.Length; i++)
                {
                    datas[i] = 0;
                }

                if (isOcSeat)
                {
                    datas[0] = 1;
                }

                occupyDic.Add(key, datas);

                return 0;
            }
        }

        //闲逛
        private void NCHangOut(CustomerNor cn)
        {
            int buildId = -1;

            List<int> ids = BuildMgr.GetRangUnlockBorn();

            for (int i = 0; i < ids.Count; i++)
            {
                buildId = ids[i];
                cn.QueueIndex = GetOccupyDic(buildId, BuildMgr.GetQueueNumById(buildId, 3));

                if (cn.QueueIndex != -1)
                {
                    break;
                }
            }

            if (cn.QueueIndex == -1 || buildId == -1 )
            {
                RemoveCn(cn);
                Debug.LogWarning("装饰位置没有地方可以停");
                return;
            }
           
            cn.targetBuildId = buildId;
            cn.SetCurState(NCSuatus.HangOut);

            int posIndex = CustomerLogic.GetAdornPosIndexById(buildId);
            int level = BuildMgr.GetUserBuildLevelById(buildId);
            cn.posIndex = posIndex;
            cn.level = level;
            PosLevelQueue pq = nCHangOut[level - 1];
            StallQueue stallQueue = pq.sqs[posIndex];
            cn.isEnterStatus = true;

            List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, stallQueue.tfs[cn.QueueIndex]);
            cn.MoveToTargetPoint(mPath);
            cn.MoveToTargetPoint(stallQueue.tfs[cn.QueueIndex]);

            occupyDic[cn.targetBuildId][cn.QueueIndex] = 1;
        }


        /// <summary>
        /// 闲逛结束
        /// </summary>
        /// <param name="cn"></param>
        private void NCHangOutEnd(CustomerNor cn)
        {
            int[] datas = new int[2];
            cn.isEnterStatus = false;

            //occupyDic[cn.targetBuildId][cn.QueueIndex] = 0;

            if (isSingleTeam)
            {
                if (cn.Data.MustGoStall == -1 || cn.GetBuyFoodNum() > 0)
                {
                    List<int> ids = BuildMgr.GetRangUnlockStall();

                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (!cn.IsHaveBeenStall(ids[i]))
                        {
                            int queueIndex = GetCanQueueIndex(cn, ids[i]);
                            if (queueIndex != -1)
                            {
                                occupyDic[cn.targetBuildId][cn.QueueIndex] = 0;
                                NCEnterBuyFood(cn, false);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    int queueIndex = GetCanQueueIndex(cn, cn.Data.MustGoStall);

                    if (queueIndex != -1)
                    {
                        occupyDic[cn.targetBuildId][cn.QueueIndex] = 0;
                        NCEnterBuyFood(cn, false);
                    }
                }

            }
            else
            {
                if (cn.Data.MustGoStall == -1)
                {
                    MNCEnterBuyFood(cn);
                }
                else
                {
                    int stallId = cn.Data.MustGoStall;
                    int posIndex = CustomerLogic.GetAdornPosIndexById(stallId);
                    int level = BuildMgr.GetUserBuildLevelById(stallId);

                    datas = GetStallQueueInfo(stallId, level - 1, posIndex);

                    if (datas != null)
                    {
                        cn.level = level;
                        cn.posIndex = posIndex;
                        cn.targetStallId = cn.Data.MustGoStall;

                        MNCEnterBuyFood(cn, false);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void NCHangOutEndEvent()
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor cn = unlockNCs[i];

                if (cn.curState == NCSuatus.HangOut && !cn.isEnterStatus)
                {
                    NCHangOutEnd(cn);
                    break;
                }
            }
        }

        /// <summary>
        /// lineIndex  0 二楼包间 1 一楼露台 2 一楼包间 3 一楼普通用餐(3靠近露台的 4大厅用餐区)
        /// </summary>
        /// <param name="cn"></param>
        private void SelectDiningArea(CustomerNor cn)
        {
            //if (true)
            //{
            //    cn.lineIndex = 4;
            //    NCEnterDiningQueue(cn);

            //    return;
            //}

            int value = Random.Range(1, 101);

            //int value = 5;

            int[] sDRatio = new int[2];
          
            int ratio = 0;

            for (int i = 0; i < sDRatio.Length; i++)
            {
                ratio += 20 * IsHaveSeatAndIsIsUnlock(diningAreaIds[i], diningSeats[i].tfs.Count);
                sDRatio[i] = ratio;
            }

            if (cn.targetStallId == diningAreaIds[2] &&
                IsHaveSeatAndIsIsUnlock(diningAreaIds[2], diningSeats[2].tfs.Count) > 0)
            {
                cn.lineIndex = 2;
            }
            else if (value <= sDRatio[0])
            {
                cn.lineIndex = 0;
            }
            else if (value > sDRatio[0] && value <= sDRatio[1])
            {
                cn.lineIndex = 1;
            }
            else
            {
                cn.lineIndex = -1;
            }

            cn.lineIndex = 1;
            
            if (cn.lineIndex == 0)
            {
                int level = StaffMgr.Instance.FindStaffById(2).level;
                if (level <= 0)
                {
                    int index = GetOccupyDic(diningAreaIds[cn.lineIndex], diningSeats[cn.lineIndex].tfs.Count, true);
                    GoToDiningSeat(cn, index);
                }
                else
                {
                    cn.SetCurState(NCSuatus.Dining);
                    cn.isEnterStatus = true;


                    Vector3 enterPos = mNavigate[cn.lineIndex].bfs[0].Pos;
                    Vector3 waitPos = mNavigate[cn.lineIndex].bfs[1].Pos;
                    cn.DirPos = waitPos + Vector3.forward * 100;

                    List<Vector3> strPos = BFS_Mgr.GetMoveList(bfs, cn.transform.position, enterPos);
                    cn.MoveToTargetPoint(strPos);

                    List<Vector3> mPath = BFS_Mgr.GetMoveList(mNavigate[cn.lineIndex].bfs, enterPos,
                        waitPos);
                    cn.MoveToTargetPoint(mPath);
                }

                return;
            }

            if (cn.lineIndex > 0 && cn.lineIndex <= 2)
            {
                int index = GetOccupyDic(diningAreaIds[cn.lineIndex], diningSeats[cn.lineIndex].tfs.Count, true);
                GoToDiningSeat(cn, index);
            }
            else
            {
                //int index = GetOccupyDic(diningAreaIds[3], diningSeats[3].tfs.Count, true);
                int index = -1;

                if (index != -1)
                {
                   cn.lineIndex = 3;

                   int enterIndex = index >= 6 ? 1:0;

                   GoToDiningSeat(cn, index, enterIndex);
                }
                else
                {
                    cn.lineIndex = 4;
                    NCEnterDiningQueue(cn);
                }
            }
        }

        //去找用餐的座位
        private void GoToDiningSeat(CustomerNor cn, int index, int enterIndex = 0)
        {
            cn.QueueIndex = index;

            cn.SetCurState(NCSuatus.Dining);

            int i = index / 2;
            cn.DirPos = diningDir[cn.lineIndex].tfs[i].position;

            cn.enterIndex = enterIndex;

            Vector3 enterPos = cn.transform.position;

            //不从入口点走
            if (cn.lineIndex != 2)
            {
                enterPos = mNavigate[cn.lineIndex].bfs[enterIndex].Pos;//入口点
                List<Vector3> strPos = BFS_Mgr.GetMoveList(bfs, cn.transform.position, enterPos);
                cn.MoveToTargetPoint(strPos);
            }

            List<Vector3> mPath = BFS_Mgr.GetMoveList(mNavigate[cn.lineIndex].bfs, enterPos, diningSeats[cn.lineIndex].tfs[index].position);
            cn.MoveToTargetPoint(mPath);
            cn.MoveToTargetPoint(diningSeats[cn.lineIndex].tfs[index]);
        }

        private void GTSecFloorSeat(CustomerNor cn)
        {
            Vector3 waitPos = mNavigate[cn.lineIndex].bfs[1].Pos;

            cn.isEnterStatus = false;

            int index = GetOccupyDic(diningAreaIds[cn.lineIndex], diningSeats[cn.lineIndex].tfs.Count, true);
            cn.QueueIndex = index;

            if (index >= -1)
            {
                RemoveCn(cn);
                Debug.LogWarning("没有位置");
                return;
            }

            Vector3 pos = diningSeats[cn.lineIndex].tfs[index].position;

            List<Vector3> endPath = BFS_Mgr.GetMoveList(mNavigate[cn.lineIndex].bfs, waitPos,
                pos);

            cn.MoveToTargetPoint(endPath);

            cn.MoveToTargetPoint(pos);
        }

        private int IsHaveSeatAndIsIsUnlock(int buildId, int SeatNum = 5)
        {
            BuildDataModel bdm = BuildMgr.GetUserBuildDataById(buildId);

            if (!BuildAreaMgr.Instance.GetIsUnLockAreaById(bdm.AreaIndex))
            {
                return 0;
            }

            int level = bdm.Level;

            if (level > 0)
            {
                level = 1;
            }
            else
            {
                level = 0;
            }
           
            int index = GetOccupyDic(buildId, SeatNum);

            if (index == -1)
            {
                index = 0;
            }
            else
            {
                index = 1;
            }

            return level * index;
        }

        #region 去一楼靠近大厅餐区


        /// <summary>
        /// 进入用餐排队
        /// </summary>
        /// <param name="cn"></param>
        //
        private void NCEnterDiningQueue(CustomerNor cn)
        {
            //cn.targetStallId = cn.Data.MustGoStall;
            //int posIndex = CustomerLogic.GetStallIndexById(cn.targetStallId);
            //cn.MoveToTargetPoint(dcEnterQuqueTf[posIndex].tfs);

            cn.SetCurState(NCSuatus.EnterDiningQuque);
            List<Vector3> mPath = BFS_Mgr.GetMoveList(bfs, cn.transform, enterQuequePoint);
            cn.MoveToTargetPoint(mPath);
            cn.MoveToTargetPoint(enterQuequePoint);
        }

        //进入用餐排队结束
        private void NCEnterDiningQueueEnd(CustomerNor cn)
        {
            NCDiningQueue(cn);
        }

        //排队用餐
        private void NCDiningQueue(CustomerNor cn)
        {
            int diningQuque = (int)NCSuatus.DiningQuque;
            cn.QueueIndex = GetQueueIndex(diningQuque);

            if (cn.QueueIndex < dcQuqueTf.Count)
            {
                cn.SetCurState(NCSuatus.DiningQuque);

                cn.MoveToTargetPoint(dcQuqueTf[cn.QueueIndex]);

                //cn.MoveToTargetPoint(dcQuqueTf, true, cn.QueueIndex);

                //Debug.LogWarning(" NCDingQueue " + cn.QueueIndex + " " + cn.gameObject.name);

                queueDic[diningQuque]++;
            }
            else
            {
                //TODO
                //PoolMgr.Instance.DespawnOne(cn.transform);
                //unlockNCs.Remove(cn);
                //GameObject.Destroy(cn.gameObject);
                RemoveCn(cn);
            }
        }

        //餐厅排队结束
        private void NCDiningQueueEnd(CustomerNor cn)
        {
            if (cn.QueueIndex != 0)
            {
                return;
            }

            if (!NCDiningFun(cn))
            {
                return;
            }

            queueDic[(int)NCSuatus.DiningQuque]--;

            ////队伍跟进
            DiningQueueEndEvent(null);
        }

        private void DiningQueueEndEvent(object obj)
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor cn = unlockNCs[i];
                if (cn.curState == NCSuatus.DiningQuque)
                {
                    if (cn.QueueIndex == 0)
                    {
                        queueDic[(int)NCSuatus.DiningQuque]--;
                        NCDiningFun(cn);
                    }
                    else
                    {
                        cn.QueueIndex--;

                        if (cn.QueueIndex >= 0)
                        {
                            cn.MoveToTargetPoint(dcQuqueTf[cn.QueueIndex]);
                        }
                    }
                }
            }
        }

        //开始用餐
        public bool NCDiningFun(CustomerNor cn)
        {
            int index = FindCanDiningIndex(cn);

            if (index != -1)
            {
                dinOccupyPosArr[index] = 1;
                
                cn.SetCurState(NCSuatus.Dining);

                cn.QueueIndex = index;

                int i = index / 2;
                cn.DirPos = diningDir[4].tfs[i].position;

                Vector3 enter = mNavigate[4].bfs[0].Pos;

                cn.MoveToTargetPoint(enter);
                List<Vector3> vs = BFS_Mgr.GetMoveList(mNavigate[4].bfs, enter, diningSeats[4].tfs[index].position);
                cn.MoveToTargetPoint(vs);
                cn.MoveToTargetPoint(diningSeats[4].tfs[index]);

                //cn.MoveToTargetPoint(nCEnterDining[index].tfs);
                //cn.MoveToTargetPoint(new List<Vector3>() {nCDining[index].position});

                return true;
            }

            return false;
        }
        #endregion 

        // 去二楼包见给小费处
        public IEnumerator DiningTip(CustomerNor cn)
        {
            yield return new WaitForSeconds(1);

            GTSecFloorSeat(cn);
            EventManager.Instance.RegisterEvent(EventKey.SecFloorTip, null);
        }

        //用餐结束（不管是在那个区）
        public IEnumerator NCDiningEnd(CustomerNor cn)
        {
            yield return new WaitForSeconds(5);

            if (cn.lineIndex == 1)
            {
                bool isGaveTip = UICommonUtil.Instance.IsFillConditByRotia(cn.Data.TipRatio);
                if (isGaveTip|| true)
                {
                    BuildDataModel bdm = BuildMgr.GetUserBuildDataById(diningAreaIds[1]);

                    Transform tf = MainSpace.Instance.equipList[bdm.Pos].GetShowBuildBoxTf(cn.QueueIndex / 2);

                    int tipMultiple = LocalCommonUtil.TipMultipleNor(cn.Data.TipMultiple);

                    LocalCommonUtil.ShowBB(4, tf, bdm.Id + cn.QueueIndex / 2, tipMultiple * 100);
                }
            }

            if (cn.lineIndex == 4)
            {
                List<Vector3> vs = BFS_Mgr.GetMoveList(mNavigate[cn.lineIndex].bfs, cn.transform, leaveTf[0].tfs[0]);
                cn.MoveToTargetPoint(vs);
                cn.MoveToTargetPoint(leaveTf[0].tfs[0]);
            }
            else
            {
                int enterPos = cn.enterIndex;
                Vector3 endPos = mNavigate[cn.lineIndex].bfs[enterPos].Pos;

                List<Vector3> vs = BFS_Mgr.GetMoveList(mNavigate[cn.lineIndex].bfs, cn.transform.position, endPos);

                List<Vector3> ls = BFS_Mgr.GetMoveList(bfs, endPos, leaveTf[0].tfs[0].position);

                cn.MoveToTargetPoint(vs);
                
                cn.MoveToTargetPoint(ls);
                cn.MoveToTargetPoint(leaveTf[0].tfs[0]);
            }

            if (cn.lineIndex < 4)
            {
                  occupyDic[diningAreaIds[cn.lineIndex]][cn.QueueIndex] = 0;
            }
            else
            {
                dinOccupyPosArr[cn.QueueIndex] = 0;
                EventManager.Instance.TriggerEvent(EventKey.NCDiningEnd, null);
            }

            cn.tkNum++;
            if (cn.NCISCanRepateTK())
            {
                cn.SetCurState(NCSuatus.Enter);
                
                List<Vector3> ls = BFS_Mgr.GetMoveList(bfs, cn.transform.position, nCEnter[2].position);

                cn.MoveToTargetPoint(ls);

                cn.MoveToTargetPoint(nCEnter[nCEnter.Count - 2]);
                cn.MoveToTargetPoint(nCEnter[nCEnter.Count - 1]);
            }
            else
            {
                CNLeave(cn);
            }
        }

        //开始离开 
        private void CNLeave(CustomerNor cn)
        {
            cn.lineIndex = 0;
            cn.SetCurState(NCSuatus.Leave);
            cn.MoveToTargetPoint(leaveTf[0].tfs);
        }


        //开始离开 
        private void CNLeaveEnd(CustomerNor cn)
        {
            //CheckoutCounter(cn);

            NewLeaveScene(cn);
        }

        //到收银台
        private void CheckoutCounter(CustomerNor cn)
        {
            cn.lineIndex = 1;

            int queueIndex = GetQueueIndex((int)NCSuatus.Leave);

            cn.QueueIndex = queueIndex;

            if (cn.QueueIndex < leaveTf[1].tfs.Count)
            {
                cn.DirPos = leaveTf[1].tfs[0].position + Vector3.up * 10;

                cn.MoveToTargetPoint(leaveTf[1].tfs, true, queueIndex);
                queueDic[(int)NCSuatus.Leave]++;
            }
            else
            {
                RemoveCn(cn);
            }
        }

        //收银台结束
        private IEnumerator CheckoutCounterEnd(CustomerNor cn)
        {
            yield return new WaitForSeconds(2);
            queueDic[(int)NCSuatus.Leave]--;

            cn.SettmentCoin();
            LeaveScene(cn);

            GjCheckoutCounter();
        }

        private void GjCheckoutCounter()
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                CustomerNor cn = unlockNCs[i];

                if (cn.curState == NCSuatus.Leave && cn.lineIndex == 1)
                {
                    cn.QueueIndex--;

                    cn.MoveToTargetPoint(leaveTf[cn.lineIndex].tfs[cn.QueueIndex]);
                }
            }
        }

        //离开场景
        private void LeaveScene(CustomerNor cn)
        {
            cn.lineIndex = 2;
            cn.MoveToTargetPoint(leaveTf[cn.lineIndex].tfs);
        }


        private void NewLeaveScene(CustomerNor cn)
        {
            cn.lineIndex = 3;
            cn.MoveToTargetPoint(leaveTf[cn.lineIndex].tfs);
        }

        // 离开结束
        public void LeaveSceneEnd(CustomerNor cn)
        {
            unlockNCs.Remove(cn);
            PoolMgr.Instance.DespawnOne(cn.transform);
        }

        float timer = 0;

        long curTime = 0;

        int gameDuration = 0;

        private void Update()
        {
            UpdteaNC();

            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                gameDuration++;
                timer = 0;

                CreateCN();

                CreateNewCN();

                if (gameDuration % 60 == 0)
                {
                    NCPedToCus();
                }
            }
        }

        private int maxCsNum = 120;

        private void CreateNewCN()
        {
            if (readyUnLockCNs.Count > 0 && unlockNCs.Count <= maxCsNum)
            {
                createNewNcTime--;

                if (createNewNcTime <= 0)
                {
                    createNewNcTime = Random.Range(10, 15);
                    NCReadyUnLockCreate();
                }
            }
        }

        Dictionary<int, long> createCNDic = new Dictionary<int, long>();

        int createNewNcTime = 0;
        float timeInterval = 1f;
        int createNcTime = 0;

        int dlBufferTimer = 0;
        int fCNum = 0; //快速创建顾客个数

        private void CreateCN()
        {
            createNcTime--;
            if (createNcTime <= 0)
            {
                if (unlockNCs.Count < faseCreateNum)
                {
                    createNcTime = 1;
                    NCCreateCus();

                    //createNcTime = Random.Range(10, 15) / ((int)(1 / timeInterval));
                    //NCCreateCus();
                    return;
                }

                if (fCNum > 0)   //前台的buff技能
                {
                    fCNum--;
                    createNcTime = 1;
                    NCCreateCus();
                    return;
                }

                if (dlBufferTimer > 0) //舞女的buff技能
                {
                    createNcTime = dlBufferTimer;
                    NCCreateCus();
                }
                else
                {
                    if (gameDuration <= 300)
                    {
                        createNcTime = Random.Range(5, 10) / ((int)(1 / timeInterval));
                        NCCreateCus();
                    }
                    else 
                    {
                        int cusNum = unlockNCs.Count;
                       
                        int ctIntNum = GetCTInNum();

                        if (cusNum >= ctIntNum * 0.9)
                        {
                            createNcTime = 60;
                            return;
                        }
                        else if (cusNum >= ctIntNum * 0.8)
                        {
                            createNcTime = Random.Range(10, 20) / ((int)(1 / timeInterval));
                            NCCreateCus();
                        }
                        else if (cusNum >= ctIntNum * 0.7)
                        {
                            createNcTime = Random.Range(10, 20) / ((int)(1 / timeInterval));
                            NCCreateCus();
                        }
                        else if (cusNum >= ctIntNum * 0.6)
                        {
                            createNcTime = Random.Range(12, 18) / ((int)(1 / timeInterval));
                            NCCreateCus();
                        }
                        else
                        {
                            int ctOutNum = ctIntNum + pedestrianTfs.Count;

                            if (cusNum <= ctOutNum * 0.9)
                            {
                                createNcTime = Random.Range(10, 15) / ((int)(1 / timeInterval));
                                NCCreateCus();
                            }
                            else
                            {
                                createNcTime = 60;
                                return;
                            }

                        }
                    }
                }

            }
        }

        private long GetNCCreateTimeById(int id, long curTime)
        {
            if (createCNDic.ContainsKey(id))
            {
                return createCNDic[id];
            }
            else
            {
                long time = curTime + Random.Range(4, 18);
                createCNDic.Add(id, time);

                return time;
            }
        }

        private void UpdteaNC()
        {
            for (int i = 0; i < unlockNCs.Count; i++)
            {
                unlockNCs[i].UpdateNC();
            }
        }

        #endregion

        #region 一个流程结束的回调

        public void NCGoToTargetPosHandle(CustomerNor cn)
        {
            if (cn.curState == NCSuatus.Enter)
            {
                NCEnterEnd(cn);
            }
            else if (cn.curState == NCSuatus.Pedestrian)
            {
                cn.lineIndex = 1;
            }
            else if (cn.curState == NCSuatus.TakeMeal)  //取餐具
            {
                //cn.SetRoalStatus(NormalRoleStatus.Breath);
                if (cn.QueueIndex == 0)
                {
                    StartCoroutine(NCTakeMealEnd(cn));
                }
            }
            else if (cn.curState == NCSuatus.TrayMakeMeal) //准备去摊位
            {
                NCTrayMakeMealEnd(cn);
            }
            else if (cn.curState == NCSuatus.BuyFood)
            {
                //cn.SetRoalStatus(NormalRoleStatus.Breath_meal);

                if (isSingleTeam)
                {
                    if (cn.lineIndex == 1)
                    {
                        NCBuyFoodQueue(cn);
                    }
                    else if (cn.lineIndex == 2)
                    {
                        NCBuyFoodGetFood(cn);
                    }
                    else if (cn.lineIndex == 3)
                    {
                         NCBuyFoodEnd(cn);
                    }
                }
                else
                {
                    if (cn.lineIndex == 1)
                    {
                        MNCBuyFoodQueue(cn);
                    }
                    else if (cn.lineIndex == 2)
                    {
                        MNCBuyFoodEnd(cn);
                    }
                }
            }
            else if (cn.curState == NCSuatus.HangOut)
            {
                NCHangOutEnd(cn);
            }
            else if (cn.curState == NCSuatus.EnterDiningQuque)
            {
                NCEnterDiningQueueEnd(cn);
            }
            else if (cn.curState == NCSuatus.DiningQuque)
            {
                NCDiningQueueEnd(cn);
            }
            else if (cn.curState == NCSuatus.Dining)
            {
                if (cn.lineIndex == 0 && cn.isEnterStatus)
                {
                    StartCoroutine(DiningTip(cn));
                }
                else
                {
                    StartCoroutine(NCDiningEnd(cn));
                }
            }
            else if (cn.curState == NCSuatus.Leave)
            {
                if (cn.lineIndex == 0)
                {
                    CNLeaveEnd(cn);
                }else if(cn.lineIndex == 1)
                {
                    if (cn.QueueIndex == 0)
                    {
                        StartCoroutine(CheckoutCounterEnd(cn));
                    }
                }
                else if (cn.lineIndex == 2 || cn.lineIndex == 3)
                {
                    LeaveSceneEnd(cn);
                }
            }
        }

        public int GetQueueIndex(int index)
        {
            if (!queueDic.ContainsKey(index))
            {
                queueDic.Add(index, 0);
            }
            else
            {
                return queueDic[index];
            }

            return 0;
        }

        #endregion

        public List<Vector3> GetTranPoss(List<Transform> tf)
        {
            List<Vector3> list = new List<Vector3>();

            for (int i = 0; i < tf.Count; i++)
            {
                list.Add(tf[i].transform.position);
            }

            return list;
        }

        private int FindCanDiningIndex(CustomerNor cn)
        {
            float minDis = -1;
            int canUseIndex = -1;

            List<Transform> seat = diningSeats[4].tfs;

            for (int i = 0; i < seat.Count; i++)
            {
                if (dinOccupyPosArr[i] == 0)
                {
                    float dis = Vector3.Distance(cn.transform.position, seat[i].position);

                    if (minDis == -1)
                    {
                        minDis = dis;
                        canUseIndex = i;
                    }
                    else
                    {
                        if (dis < minDis)
                        {
                            minDis = dis;
                            canUseIndex = i;
                        }
                    }
                }
            }

            return canUseIndex;
        }

        //    //椅子从0级开始解锁
        //private int FindCanDiningIndex(CustomerNor cn)
        //{
        //    float minDis = -1;
        //    int canUseIndex = -1;

        //    List<Transform> seat = diningSeats[4].tfs;

        //    List<int> canUseChair = BuildMgr.GetCanUseCom(diningAreaIds[3]);

        //    for (int i = 0; i < canUseChair.Count; i++)
        //    {
        //        int index = canUseChair[i];

        //        if (dinOccupyPosArr[index] == 0)
        //        {
        //            float dis = Vector3.Distance(cn.transform.position, seat[index].position);

        //            if (minDis == -1)
        //            {
        //                minDis = dis;
        //                canUseIndex = index;
        //            }
        //            else
        //            {
        //                if (dis < minDis)
        //                {
        //                    minDis = dis;
        //                    canUseIndex = index;
        //                }
        //            }
        //        }
        //    }

        //    return canUseIndex;
        //}

        private void RemoveCn(CustomerNor cn, bool isDespawn = false)
        {
            //if (isDespawn)
            //{
            //    List<Vector3> ls = BFS_Mgr.GetMoveList(bfs, cn.transform.position, leaveTf[0].tfs[0].position);

            //    cn.MoveToTargetPoint(ls);

            //    CNLeave(cn);
            //}
            //else
            //{
            //    PoolMgr.Instance.DespawnOne(cn.transform);
            //    unlockNCs.Remove(cn);
            //}

            PoolMgr.Instance.DespawnOne(cn.transform);
            unlockNCs.Remove(cn);
        }


        bool isDebug = false;

        private int GetCurDiners()
        {
            int num = 0;

            for (int i = 0; i < unlockNCs.Count; i++)
            {
                if (unlockNCs[i].curState == NCSuatus.Dining)
                {
                    num++;
                }
            }

            if (isDebug)
            {
                Debug.LogWarning($"GetCurDiners{num}");
            }

            return num;
        }

        public int GetAllUnlockSeat()
        {
            int num = 0;
            for (int i = 0; i < diningSeats.Count - 1; i++)
            {
                int level = BuildMgr.GetUserBuildLevelById(diningAreaIds[i]);

                if (level > 0)
                {
                    num += diningSeats[i].tfs.Count;
                }
            }

            if (BuildMgr.GetUserBuildLevelById(diningAreaIds[3]) > 0)
            {
                num += diningSeats[4].tfs.Count;
            }

            if (isDebug)
            {
                Debug.LogWarning($"GetAllUnlockSeat{num}");
            }

            return num;
        }


        //得到餐厅里面的人数
        public int GetCTInNum()
        {
            int takeMealNum = 0;

            for (int i = 0; i < nCTakeMeal.Count; i++)
            {
                takeMealNum += nCTakeMeal[i].tfs.Count;
            }

            return takeMealNum + GetAllUnlockSeat() + BuildMgr.GetAllStallAndBornPosNum();
        }

    }
}