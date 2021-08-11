using EazyGF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNorEditTool : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform buildDirGrid;

    public Transform stallDirGrid;
    public Transform stallBuyFoodQueueGrid;

    public CustomerMgr customerMgr;

    public Transform enterGrid;
    [ContextMenu("SetEnterTfs")]
    public void SetEnterTfs()
    {
        AddTf(enterGrid, customerMgr.nCEnter, "Enter");
    }

    public Transform enterPedestrianGrid;
    [ContextMenu("SetpedestrianTfs")]
    public void SetpedestrianTfs()
    {
        AddTf(enterPedestrianGrid, customerMgr.pedestrianTfs, "pedSeats");
    }

    public Transform dininerQueue;
    [ContextMenu("SetdininerQueue")]
    public void SetdininerQueue()
    {
        AddTf(dininerQueue, customerMgr.dcQuqueTf, "queue");
    }

    public Transform enterEnterGrid;
    public Transform tackMealDirGrid;
    public Transform tackMealGrid;

    [ContextMenu("SetEnterAndTackMeal")]
    public void SetEnterAndTackMeal()
    {
        AddTf(enterEnterGrid, customerMgr.nCEnter, "Enter");
        AddTf(tackMealDirGrid, customerMgr.takeMealDirTf, "Enter");
        SetPosQueueTypeData(customerMgr.nCTakeMeal, tackMealGrid, "tm");
    }

    [ContextMenu("InitBuidStallDir")]
    public void IInitBuidStallDirnitPos()
    {
        AddTf(buildDirGrid, customerMgr.stallDirTf, "dir_");
    }


    [ContextMenu("SetBuyFoodQueque")]
    public void SetBuyFoodQueque()
    {
        SetPosLevelQueueData(customerMgr.nCStallTf, stallBuyFoodQueueGrid, "SQ");
    }

    public Transform qcdGrid;

    [ContextMenu("SetQCD")]
    public void SetQCD()
    {
        SetPosLevelQueueData(customerMgr.nCStallGetFoodPos, qcdGrid, "QCD");
    }

    public Transform levelGrid;

    [ContextMenu("InitLeavePos")]
    public void InitLeavePos()
    {
        customerMgr.leaveTf.Clear();

        for (int i = 0; i < levelGrid.childCount; i++)
        {
            Transform tf = levelGrid.GetChild(i);
            tf.gameObject.name = i.ToString();

            PosQueue sq = new PosQueue();
            sq.tfs = new List<Transform>();

            for (int j = 0; j < tf.childCount; j++)
            {
                tf.GetChild(j).gameObject.name = $"{i}lp{j}";
                sq.tfs.Add(tf.GetChild(j));
            }

            customerMgr.leaveTf.Add(sq);
        }

        Debug.LogError("设置成功");
    }

    public Transform cusGrid;
    public GameObject cus;

    [ContextMenu("SetCusAtStallBuild")]
    public void SetCusAtStallBuild()
    {
        RemoveCus();

        BlStallQueue(true);
    }

    public void BlStallQueue(bool isSetCusPos)
    {
        for (int i = 0; i < customerMgr.nCStallTf.Count; i++)
        {
            PosLevelQueue pl = customerMgr.nCStallTf[i];

            for (int j = 0; j < pl.sqs.Count; j++)
            {
                StallQueue sq = pl.sqs[j];

                for (int k = 0; k < sq.tfs.Count; k++)
                {
                    if (sq.tfs[k].parent.parent.gameObject.activeSelf)
                    {
                        CreatePre(cusGrid, cus, sq.tfs[k], $"{i}_{j}_{k}", isSetCusPos);
                    }
                }
            }
        }
    }

    [ContextMenu("RemoveCus")]
    public void RemoveCus()
    {
        for (int i = cusGrid.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(cusGrid.GetChild(i).gameObject);
        }
    }


    [ContextMenu("SetStallQueuePointByCus")]
    public void SetStallQueuePointByCus()
    {
        BlStallQueue(false);
    }

    public Transform bfsGrid;
    public Transform bfsCusGrid;
    [ContextMenu("InitStallNav")]
    public void InitStallNav()
    {
        customerMgr.bfs = new BFSNode[bfsGrid.childCount];
        RemoveTfChild(bfsCusGrid);
        for (int i = 0; i < bfsGrid.childCount; i++)
        {
            BFSNode bn = bfsGrid.GetChild(i).GetComponent<BFSNode>();

            if (bn.transform.childCount > 0)
            {
                RemoveTfChild(bn.transform);
            }

            CreatePre2(bn.transform, cus, bfsCusGrid, "Bgcus");

            bn.Init();

            bn.gameObject.name = $"bs{i}";

            customerMgr.bfs[i] = bn;
        }
    }

    public Transform pedestrianNavGrid;
    [ContextMenu("InitPedNav")]
    public void InitPedNav()
    {
        customerMgr.pedestrianBfs = new BFSNode[pedestrianNavGrid.childCount];
        
        for (int i = 0; i < pedestrianNavGrid.childCount; i++)
        {
            BFSNode bn = pedestrianNavGrid.GetChild(i).GetComponent<BFSNode>();

            if (bn.transform.childCount > 0)
            {
                RemoveTfChild(bn.transform);
            }

            bn.Init();

            bn.gameObject.name = $"Pede{i}";

            customerMgr.pedestrianBfs[i] = bn;
        }

        Debug.LogError("设置成功");
    }

    [ContextMenu("RemvoeBFSCus")]
    public void RemvoeBFSCus()
    {
        RemoveTfChild(bfsCusGrid);
    }

    public Transform mNavGrid;
   [ContextMenu("InitMNavBFS")]
    public void InitMNavBFS()
    {
        customerMgr.mNavigate = new MNavigate[mNavGrid.childCount];
       
        for (int j = 0; j < mNavGrid.childCount; j++)
        {
            Transform tf = mNavGrid.GetChild(j);

            tf.gameObject.name = $"nav{j}";


            BFSNode[] bfs = new BFSNode[tf.childCount];

            for (int i = 0; i < tf.childCount; i++)
            {
                BFSNode bn = tf.GetChild(i).GetComponent<BFSNode>();

                if (bn.transform.GetComponent<Collider>() != null)
                {
                    GameObject.DestroyImmediate(bn.transform.GetComponent<Collider>());
                }

                if (bn.transform.childCount > 0)
                {
                    RemoveTfChild(bn.transform);
                }

                bn.Init();

                bn.gameObject.name = $"nbs{i}";

                bfs[i] = bn;
            }

            MNavigate mNavigate = new MNavigate();
            mNavigate.bfs = bfs;

            customerMgr.mNavigate[j] = mNavigate;
        }

        Debug.LogError("设置成功");
    }

    public Transform hangOutGrid;
    [ContextMenu("SetHangOut")]
    public void SetHangOut()
    {
        customerMgr.nCHangOut.Clear();

        for (int i = 0; i < hangOutGrid.childCount; i++)
        {
            //stallMoveGrid.GetChild(i).transform.position = buildModelTfGrid.GetChild(i).transform.position;
            //stallMoveGrid.GetChild(i).gameObject.name = $"{i}";

            Transform ptf = hangOutGrid.GetChild(i);
            ptf.gameObject.name = $"level{i + 1}";

            PosLevelQueue pq = new PosLevelQueue();
            pq.sqs = new List<StallQueue>();

            for (int k = 0; k < ptf.childCount; k++)
            {
                Transform tf = ptf.GetChild(k);
                tf.gameObject.name = $"pos{k}";

                StallQueue stallQueue = new StallQueue();
                stallQueue.tfs = new List<Transform>();

                for (int j = 0; j < tf.childCount; j++)
                {
                    tf.GetChild(j).gameObject.name = $"{i} {k} {j}";
                    stallQueue.tfs.Add(tf.GetChild(j));
                }

                pq.sqs.Add(stallQueue);
            }

            customerMgr.nCHangOut.Add(pq);
        }

        Debug.LogError("设置成功");
    }


    public Transform diningDirPosGrid;
    [ContextMenu("InitDiningDirPos")]
    public void InitDiningDirPos()
    {
        SetPosQueueTypeData(customerMgr.diningDir, diningDirPosGrid, "Dr");
    }


    public Transform diningSeatPosGrid;
    [ContextMenu("InitDiningSeatPos")]
    public void InitDiningSeatsPos()
    {
        SetPosQueueTypeData(customerMgr.diningSeats, diningSeatPosGrid, "Ds");
    }

    //public Transform getFoodPosGrid;

    //[ContextMenu("SetGetFoodPos")]
    //public void SetGetFoodPos()
    //{
    //    SetPosLevelQueueData(customerMgr.nCStallGetFoodPos, getFoodPosGrid, "GF");
    //}

    public Transform mBuyFoodPosGrid;
    [ContextMenu("SetMBuyFoodPos")]
    public void SetMBuyFoodPos() {

        customerMgr.stallMultiQueue.Clear();

        for (int i = 0; i < mBuyFoodPosGrid.childCount; i++)
        {
            Transform leveltf = mBuyFoodPosGrid.GetChild(i);
            leveltf.gameObject.name = $"level{i + 1}";

            LevelPMQ pq = new LevelPMQ();
            pq.datas = new List<PosMultiQueue>();

            for (int k = 0; k < leveltf.childCount; k++)
            {
                Transform pos = leveltf.GetChild(k);
                pos.gameObject.name = $"pos{k}";

                PosMultiQueue pmq = new PosMultiQueue();
                pmq.datas = new List<PosQueue>();

                for (int j = 0; j < pos.childCount; j++)
                {
                    Transform queueTf = pos.GetChild(j);
                    queueTf.gameObject.name = $"line{j}";

                    PosQueue stallQueue = new PosQueue();
                    stallQueue.tfs = new List<Transform>();

                    for (int y = 0; y < queueTf.childCount; y++)
                    {
                        queueTf.GetChild(y).gameObject.name = $"{name}_{i}_{k}_{j}_{y}";
                        stallQueue.tfs.Add(queueTf.GetChild(y));
                    }

                    pmq.datas.Add(stallQueue);
                }

                pq.datas.Add(pmq);
            }

            customerMgr.stallMultiQueue.Add(pq);
        }

        Debug.LogError("设置成功");
    }

    #region 通用方法
    //--------------------------------------------------------------------------------

    private void SetPosLevelQueueData(List<PosLevelQueue> list, Transform grid, string name) 
    {
        list.Clear();

        for (int i = 0; i < grid.childCount; i++)
        {
            Transform ptf = grid.GetChild(i);
            ptf.gameObject.name = $"level{i + 1}";

            PosLevelQueue pq = new PosLevelQueue();
            pq.sqs = new List<StallQueue>();

            for (int k = 0; k < ptf.childCount; k++)
            {
                Transform tf = ptf.GetChild(k);
                tf.gameObject.name = $"pos{k}";

                StallQueue stallQueue = new StallQueue();
                stallQueue.tfs = new List<Transform>();

                for (int j = 0; j < tf.childCount; j++)
                {
                    tf.GetChild(j).gameObject.name = $"{name}_{i}_{k}_{j}";
                    stallQueue.tfs.Add(tf.GetChild(j));
                }

                pq.sqs.Add(stallQueue);
            }
            list.Add(pq);
        }

        Debug.LogError("设置成功");
    }

    private void SetPosQueueTypeData(List<PosQueue> list, Transform grid, string name)
    {
        list.Clear();

        for (int i = 0; i < grid.childCount; i++)
        {
            Transform tf = grid.GetChild(i);
            tf.gameObject.name = i.ToString();

            PosQueue sq = new PosQueue();
            sq.tfs = new List<Transform>();

            for (int j = 0; j < tf.childCount; j++)
            {
                tf.GetChild(j).gameObject.name = $"{i}{name}{j}";
                sq.tfs.Add(tf.GetChild(j));
            }

            list.Add(sq);
        }

        Debug.LogError("设置成功");
    }

    private void RemoveTfChild(Transform tf)
    {
        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(tf.GetChild(i).gameObject);
        }

    }

    private void CreatePre2(Transform tf, GameObject pre,Transform grid,  string name)
    {
        GameObject go = GameObject.Instantiate(pre);
        go.name = name;
        go.transform.SetParent(grid);
        go.SetActive(true);
        go.transform.position = tf.transform.position;
    }

    private void CreatePre(Transform tf, GameObject pre, Transform posTf,string name, bool isSetCusPos = true)
    {
        if (isSetCusPos)
        {
            GameObject go = GameObject.Instantiate(pre);
            go.name = $"cus_{name}";
            go.transform.SetParent(tf);
            go.SetActive(true);
            go.transform.position = posTf.position;
        }
        else
        {
            GameObject cus = GameObject.Find($"cus_{name}");
            if (cus == null)
            {
                Debug.LogError("没有找到" + name);
                return;
            }

            posTf.position = cus.transform.position;
        }
    }


    private void AddTf(Transform grid, List<Transform> tfs, string name = "")
    {
        tfs.Clear();

        for (int i = 0; i < grid.childCount; i++)
        {
            grid.GetChild(i).name = $"{name}_{i}";
            tfs.Add(grid.GetChild(i));
        }

        Debug.LogError("设置成功");
    }

    #endregion

}
