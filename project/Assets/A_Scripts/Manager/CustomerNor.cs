using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public enum NormalRoleStatus
    {
        Breath, //站立
        Breath_meal,//端盘子站立
        Breath_panzi,//空盘子站立
        Move,
        Move_meal,
        Move_panzi,
        Sit,
    }

    public class CustomerNor : MonoBehaviour
    {
        Queue<Vector3> queue = new Queue<Vector3>();

        private bool isTargetPos = true;
        public float moveSpeed;

        Vector3 targetPos = Vector3.zero;

        public delegate void GoToTargetPosHandle(CustomerNor car);
        public event GoToTargetPosHandle onHandle;
        public event GoToTargetPosHandle onHandle2;  //将队列中的点走完时的回调

        public NCSuatus curState;

        private int queueIndex;
        private int canBuyFoodNum = 0;

        public int lineIndex;
        public int posIndex;
        public int enterIndex;
        public int level;

        public int targetStallId;
        public int targetBuildId;
        public int getFoodNum;
        public int tkNum;
        public bool isEnterStatus;
        public int createTime;
        public bool isFirst;

        CustomerNormal_Property data;
        CustomerNorBubble_PropertyBase bubbleData;
        int showBBType = -1;
        int curBxIndex = -1;

        public List<SkeletonAnimation> sas;

        public List<Transform> bxTf;

        List<SkeletonDataAsset> skDatas = new List<SkeletonDataAsset>();

        private Vector3 dirPos;

        string[] spNames = new string[4] { "normalRole1", "normalRole1back", "normalRole2", "normalRole2back" };
        string[] spAssetName = new string[4] { "role_1001_2_4_5", "role_1001_245_back", "role_1003_6_7", "role_1003_67_back" };

        public int QueueIndex { get => queueIndex; set => queueIndex = value; }
        public int ShowBBType { get => showBBType; set => showBBType = value; } //
        public CustomerNormal_Property Data { get => data; set => data = value; }

        public CustomerNorBubble_PropertyBase BubbleData { get => bubbleData; set => bubbleData = value; }

        public Vector3 DirPos { get => dirPos; set => dirPos = value; }
        public int CurBxIndex { get => curBxIndex; set => curBxIndex = value; }

        private Vector3 dir;

        private int buyFoodSpaceCoin;

        [ContextMenu("FillBXTf")]
        public void FillBXTf()
        {
            bxTf.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject go = transform.GetChild(i).gameObject;
                go.name = "" + i;
                bxTf.Add(go.transform);

                go.transform.GetChild(0).gameObject.name = "SaUP";
                go.transform.GetChild(1).gameObject.name = "SaDown";
            }
        }

        public void ResetData()
        {
            QueueIndex = -1;
            ShowBBType = -1;

            data = null;
            onHandle2 = null;

            getFoodNum = 0;
            tkNum = 0;

            haveBeenStalls.Clear();
            queue.Clear();
            buyFoodSpaceCoin = 0;

            isFirst = false;

            lastPlayAniName = "";
            lastSaIndex = -1;
            isGaveELTip = true;
            curBxIndex = -1;
            RemoveHaveBeenStall();
        }

       
        public void SetBX(int index, bool isUpdateFashion = true)
        {
            if (index == -1 || bxTf == null || curBxIndex == index || index >= bxTf.Count)
            {
                return;
            }

            curBxIndex = index;

            for (int i = 0; i < bxTf.Count; i++)
            {
                if (bxTf[i] == null)
                {
                    return;
                }

                bxTf[i].gameObject.SetActive(i == index);
            }

            sas[0] = bxTf[index].GetChild(0).GetComponent<SkeletonAnimation>();
            sas[1] = bxTf[index].GetChild(1).GetComponent<SkeletonAnimation>();

            if (isUpdateFashion)
            {
                CusFashionMgr.Instance.UpdateFashionIndex(Data.ID, curBxIndex);
            }
        }

        public void AddBuyFoodCostCoin()
        {
            int coinNum = BuildMgr.GetStillFoodPrice(targetStallId);
            buyFoodSpaceCoin += coinNum;
        }

        public void SettmentCoin()
        {
            ItemPropsManager.Intance.AddItem(1, buyFoodSpaceCoin);
        }

        public void MoveToTargetPoint(List<Vector3> pos)
        {
            for (int i = 0; i < pos.Count; i++)
            {
                queue.Enqueue(pos[i]);
            }

            SetRoleMoveAni();
        }

        public void MoveToTargetPoint(Vector3 pos)
        {
            queue.Enqueue(pos);

            SetRoleMoveAni();
        }

        public void MoveToTargetPoint(List<Transform> tf, bool isSpe = false, int index = 0)
        {
            if (isSpe)
            {
                for (int i = tf.Count - 1; i >= index; i--)
                {
                    queue.Enqueue(tf[i].transform.position);
                }
            }
            else
            {
                for (int i = 0; i < tf.Count; i++)
                {
                    queue.Enqueue(tf[i].transform.position);
                }
            }

            SetRoleMoveAni();
        }
        public void MoveToTargetPoint(Vector3[] tf)
        {
            for (int i = 0; i < tf.Length; i++)
            {
                queue.Enqueue(tf[i]);
            }

            SetRoleMoveAni();
        }

        public void MoveToTargetPoint(Transform tf)
        {
            queue.Enqueue(tf.position);

            SetRoleMoveAni();
        }

        private void SetRoleMoveAni()
        {
            if (curState == NCSuatus.Enter || curState == NCSuatus.TakeMeal || curState == NCSuatus.Pedestrian
                || curState == NCSuatus.Leave)
            {
                SetRoalStatus(NormalRoleStatus.Move);
            }
            else if(curState == NCSuatus.TrayMakeMeal || curState == NCSuatus.BuyFood || curState == NCSuatus.HangOut)
            {
                SetRoalStatus(NormalRoleStatus.Move_panzi);
            }
            else
            {
                SetRoalStatus(NormalRoleStatus.Move_meal);
            }
        }

        private void SetRoleStill()
        {
            //if (curState == NCSuatus.TrayMakeMeal || curState == NCSuatus.BuyFood || curState == NCSuatus.HangOut || curState == NCSuatus.DiningQuque
            //    || curState == NCSuatus.EnterDiningQuque)

              if ((int)curState >= 3 && (int)curState <= 7)
             {
                if (curState == NCSuatus.BuyFood && lineIndex == 2)
                {
                    SetTurnByDir();
                }

                if (curState == NCSuatus.BuyFood || curState == NCSuatus.HangOut)
                {
                    SetRoalStatus(NormalRoleStatus.Breath_panzi);

                    if (lineIndex == 2 || lineIndex == 3)
                    {
                        SetTurnByDir();
                    }
                }
                else
                {
                    SetRoalStatus(NormalRoleStatus.Breath_meal);
                }

            }
            else if (curState == NCSuatus.Dining)
            {
                SetTurnByDir();

                if (lineIndex == 0 && isEnterStatus) 
                {
                    SetRoalStatus(NormalRoleStatus.Breath_meal);
                }
                else
                {
                    SetRoalStatus(NormalRoleStatus.Sit);
                }
            }
            else
            {
                if (curState == NCSuatus.TakeMeal || (curState == NCSuatus.Leave && lineIndex == 1 && queueIndex == 0))
                {
                    if (queueIndex == 0)
                    {
                        SetTurnByDir();
                    }
                }

                SetRoalStatus(NormalRoleStatus.Breath);
            }
        }

        private void SetTurnByDir()
        {
            dir = dirPos - transform.position;
            SetTurn();
        }

        string lastSkeletonName = "";

        public void SetRole()
        {
            //SetRoleShowByIndex(0);
            //SetRoleShowByIndex(1);
            //curBxIndex = -1;

            int bxindex = CusFashionMgr.Instance.GetFashionByCusId(Data.ID);

            if (bxindex == -1)
            {
                bxindex = 0;
            }
            SetBX(bxindex, false);

            sas[0].gameObject.SetActive(true);
            sas[1].gameObject.SetActive(true);



            //skDatas.Clear();
            //for (int i = 0; i < 2; i++)
            //{
            //    skDatas.Add(CustomerLogic.LoadSkeletonDataAsset($"{spNames[Data.RoleIndex + i]}", spNames[Data.RoleIndex + i]));
            //    //skDatas.Add(CustomerLogic.LoadSkeletonDataAsset("role", spNames[Data.RoleIndex + i]));
            //}

            //sa.skeletonDataAsset;


            //sa.initialSkinName = skeletonName;

            //--
            //string skeletonName = Data.RoleName + "_0";
            //sa.skeletonDataAsset = CustomerLogic.GetSkeletonDataAssetByName(spNames[Data.RoleIndex + 1], spAssetName[Data.RoleIndex + 1]);
            //sa.Initialize(true);

            //sa.Skeleton.SetSkin(skeletonName);

            //lastSkeletonName = skeletonName;
        }


        public void SetTurn()
        {
            if (IsForward())
            {
                //string skeletonName = Data.RoleName + "_0";
                //if (lastSkeletonName != skeletonName)
                //{
                //    sa.skeletonDataAsset = CustomerLogic.GetSkeletonDataAssetByName(spNames[Data.RoleIndex + 1], spAssetName[Data.RoleIndex + 1]);
                //    sa.Initialize(true);
                //    sa.Skeleton.SetSkin(skeletonName);
                //}

                //lastSkeletonName = skeletonName;
                //SetRoleShowByIndex(0);
                curSaIndex = 0;

                if (IsRight())
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                curSaIndex = 1;

                //SetRoleShowByIndex(1);

                //string skeletonName = Data.RoleName;
                //if (lastSkeletonName != skeletonName)
                //{
                //    //sa.skeletonDataAsset = CustomerLogic.GetSkeletonDataAssetByName(spNames[Data.RoleIndex], spAssetName[Data.RoleIndex]);
                //    ////sa.initialSkinName = skeletonName;
                //    //sa.Initialize(true);
                //    //sa.Skeleton.SetSkin(skeletonName);

                //    //Debug.LogError(spNames[Data.RoleIndex] + " 0 " + spAssetName[Data.RoleIndex] + " " + skeletonName);


                //    //Debug.LogError(spNames[Data.RoleIndex] + " 1 " + spAssetName[Data.RoleIndex]);

                //    //sa.Initialize(true);
                //}
                //lastSkeletonName = skeletonName;

                if (IsRight())
                {
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }

            PlayAni(lastPlayAniName);
        }

        //0 朝前 1朝后
        private void SetRoleShowByIndex(int index)
        {
         
            sas[0].gameObject.SetActive(0 == index);
            sas[1].gameObject.SetActive(1 == index);
        }

        int curSaIndex = 0;
        string lastPlayAniName;
        int lastSaIndex = -1;

        public void PlayAni(string animName)
        {
            if (lastPlayAniName == animName && lastSaIndex == curSaIndex)
            {
                return;
            }

            lastPlayAniName = animName;
            lastSaIndex = curSaIndex;

            if (sas[curSaIndex].AnimationState != null)
            {
                sas[curSaIndex].AnimationState.SetAnimation(0, animName, true);

                sas[curSaIndex].gameObject.SetActive(true);
            }
            else
            {
                lastSaIndex = -1;
            }

            if (sas[1 - curSaIndex].AnimationState != null)
            {
                //sas[1 - curSaIndex].AnimationState.SetAnimation(0, animName, true);
                sas[1 - curSaIndex].ClearState();
                sas[1 - curSaIndex].gameObject.SetActive(false);
            }
        }

        private bool IsForward()
        {
            return Vector3.Dot(dir, Camera.main.transform.forward) >= 0;
        }

        private bool IsRight()
        {
            return Vector3.Dot(dir, Camera.main.transform.right) >= 0;
        }

        public void SetCurState(NCSuatus nCSuatus)
        {
            this.curState = nCSuatus;
        }

        public void SetRoalStatus(NormalRoleStatus normalRoleStatus)
        {
            string aniName = "breath";

            switch (normalRoleStatus)
            {
                case NormalRoleStatus.Breath:
                    aniName = "breath";
                
                    break;
                
                case NormalRoleStatus.Breath_meal:
                    aniName = "breath_meal";
                    break;

                case NormalRoleStatus.Breath_panzi:
                    aniName = "breath_panzi";
                    break;

                case NormalRoleStatus.Move:
                    aniName = "move";
                
                    break;
                
                case NormalRoleStatus.Move_meal:

                    aniName = "move_meal";
                    
                    break;

                case NormalRoleStatus.Move_panzi:

                    aniName = "move_panzi";

                    break;

                case NormalRoleStatus.Sit:

                    aniName = "sit";
                    break;
                default:
                    break;
            }

            PlayAni(aniName);
        }

        //Vector3 moveDir;

        public void UpdateNC()
        {
            if (queue.Count > 0 && isTargetPos)
            {
                targetPos = queue.Dequeue();
                isTargetPos = false;

                dir = (targetPos - transform.position).normalized;
            
                SetTurn();
            }

            if (!isTargetPos)
            {
                if (Vector3.Distance(transform.position, targetPos) <= moveSpeed * Time.deltaTime)
                {
                    transform.position = targetPos;
                    isTargetPos = true;
                    targetPos = Vector3.zero;

                    if (onHandle != null && queue.Count > 0)
                    {
                        onHandle(this);
                    }

                    if (onHandle2 != null && queue.Count == 0)
                    {
                        SetRoleStill();
                        onHandle2(this);
                    }
                }
                else
                {
                    transform.transform.position += dir * moveSpeed * Time.deltaTime;
                }
            }
        }

        public void RandomTip()
        {
            bool gaveRandomTip = true;

            if (gaveRandomTip)
            {
                return;
            }

            int ratio = UnityEngine.Random.Range(0, 100);

            if (ratio <= Data.TipRatio && data.ID != 1)
            {
                DropCoin dropCoin = AssetMgr.Instance.LoadGameobjFromPool("Coin").GetComponent<DropCoin>();
                dropCoin.transform.position = transform.position + new Vector3(Random.Range(-3, 3), 0, 0);
                dropCoin.transform.localEulerAngles = new Vector3(30, 0, 0);
                dropCoin.BuildData(Data.TipMultiple);
            }

        }

        public void BindData(CustomerNormal_Property data)
        {
            Data = data;
            BubbleData = CustomerNorBubble_Data.GetCustomerNorBubble_DataByID(data.ID);

            if (data.ID == 1)
            {
                canBuyFoodNum = 1;
            }
            else
            {
                canBuyFoodNum = CustomerLogic.GetCanBuyFoodNum();
            }
            
        }

        List<int> haveBeenStalls = new List<int>();

        public bool IsHaveBeenStall(int stallid)
        {
            return haveBeenStalls.Contains(stallid);
        }

        public int GetBuyFoodNum()
        {
            return haveBeenStalls.Count;
        }

        public void RemoveHaveBeenStall()
        {
            haveBeenStalls.Clear();
        }

        public void AddBaveStall(int stallId)
        {
            if (haveBeenStalls.Contains(stallId))
            {
                Debug.LogError("已经去过该摊位");
            }

            haveBeenStalls.Add(stallId);
        }

        public bool IsGoOnBuyFood()
        {
            return haveBeenStalls.Count < canBuyFoodNum; 

            return false;
        }

        public bool NCISCanRepateTK()
        {

            return false;

            //int rang = UnityEngine.Random.Range(1, 101);
         
            //if (tkNum == 1)
            //{
            //    return data.SecondMlRatio >= rang;
            //}
            //else if (tkNum == 2)
            //{
            //    return data.ThreeTimesMRatio >= rang;
            //}

            //return false;
        }


        bool isGaveELTip = true;
        //二楼用餐是否给小费
        public bool IsGaveELTip()
        {
            if (isGaveELTip && curState == NCSuatus.Dining && lineIndex == 0)
            {
                isGaveELTip = false;
                return true;
            }

            return false;
        }
    }
}
