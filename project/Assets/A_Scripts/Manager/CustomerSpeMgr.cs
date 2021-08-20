using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace EazyGF
{
    public enum SRoleStage //角色阶段
    {
        MoveTarget,
        Breath,
        Action,
        Leave,
    }

    class CustomerSpeMgr : MonoBehaviour
    {
        List<CustomerSpe> customerSpes = new List<CustomerSpe>();
        DateTime[] onlineTime;
        //public Vector3 bornsTf;
        //Vector3 bornPoint;
        public List<PosQueue> pathLines;
        //public List<PosQueue> lines;  //所有去目的地的路线

        public List<PosQueue> leaveTf;  //离开的所有角色的路线

        Dictionary<int, int> recordLineDic;

        float timeInterve = 30;
        float timer = 0;

        List<CustomerSpecial_Property> customers;
        private void Start()
        {
            customers = new List<CustomerSpecial_Property>();
            recordLineDic = new Dictionary<int, int>();
            InitCustomer();
        }

        /// <summary>
        /// 用来管理场上的特殊顾客
        /// </summary>
        public void InitCustomer()
        {
            onlineTime = new DateTime[CustomerSpecial_Data.ArrayLenth];
            for (int i = 0; i < CustomerSpecial_Data.ArrayLenth; i++)
            {
                customers.Add(CustomerSpecial_Data.DataArray[i]);
                onlineTime[i] = DateTime.Now;
            }
        }

        private void OnEnable()
        {
            EventManager.Instance.RegisterEvent(EventKey.SpeRoleLeave, SpeRoleLeave);
            EventManager.Instance.RegisterEvent(EventKey.SpeRoleTriggerEvent, SpeRoleTriggerEvent);
            EventManager.Instance.RegisterEvent(EventKey.DancingPlayAction, PlayDancingAction);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListening(EventKey.SpeRoleLeave, SpeRoleLeave);
            EventManager.Instance.RemoveListening(EventKey.SpeRoleTriggerEvent, SpeRoleTriggerEvent);
            EventManager.Instance.RemoveListening(EventKey.DancingPlayAction, PlayDancingAction);
        }

        private void SpeRoleTriggerEvent(object arg0)
        {
            CustomerSpe cs = GetCustomerSpeById((int)arg0);

            if ((cs != null && cs.curSRoleStage == SRoleStage.Action))
            {
                cs.ShowAction();
            }

        }

        private void SpeRoleLeave(object arg0)
        {
            //CustomerSpe id = (CustomerSpe)arg0;

            CustomerSpe cs = GetCustomerSpeById((int)arg0);

            if (cs != null && cs.curSRoleStage != SRoleStage.Leave)
            {
                cs.curSRoleStage = SRoleStage.Leave;

                cs.gameObject.layer = 0;

                bool isAddSuc = CustomerLogic.AddUnLockCS(cs.Data);

                Transform leave = leaveTf[cs.Data.RoleIndex].tfs[recordLineDic[cs.Data.RoleIndex]];

                Vector3[] nodes = new Vector3[leave.childCount];

                for (int i = 0; i < leave.childCount; i++)
                {
                    nodes[i] = leave.GetChild(i).position;
                }

                cs.MoveToTargetPoint(nodes);

                cs.onHandle2 += (x) =>
                {
                    customers.Add(CustomerSpecial_Data.DataArray[cs.Data.RoleIndex]);
                    recordLineDic.Remove(cs.Data.RoleIndex);

                    onlineTime[cs.Data.RoleIndex] = DateTime.Now;
                    if (isAddSuc)
                    {
                        UIMgr.ShowPanel<RoleMapPanel>();
                    }
                };
            }
        }

        private void PlayDancingAction(object arg0)
        {
            CustomerSpe cs = GetCustomerSpeById((int)arg0);
            if (cs != null && cs.curSRoleStage != SRoleStage.Leave)
            {
                cs.DancingGirlPlayAnim("action");
                StartCoroutine(CSAction(cs));
            }
        }

        public CustomerSpe GetCustomerSpeById(int hashCode)
        {
            for (int i = 0; i < customerSpes.Count; i++)
            {
                if (customerSpes[i].MHashCode == hashCode)
                {
                    return customerSpes[i];
                }
            }
            return null;
        }

        public int GetSCNum()
        {
            return customerSpes.Count;
        }

        int createScTimer = 1;
        public void CreateCs()
        {
            createScTimer--;

            if (createScTimer <= 0)
            {
                //createScTimer = Random.Range(10, 30);

                createScTimer = 5;

                // CustomerSpecial_Property[] datas = CustomerSpecial_Data.DataArray;

                if (customers.Count > 0)
                {
                    int index = Random.Range(0, customers.Count);
                    LoadCs(customers[index]);
                    customers.Remove(customers[index]);
                }
            }
        }

        bool isDancerShowed = false;
        //创建特殊顾客
        private void CreateRoleAtOnlineTime()
        {
            for (int i = 0; i < customers.Count; i++)
            {
                if (IsCreate(P(customers[i])))
                {
                    if (customers[i].ID == 2)
                    {
                        isDancerShowed = true;
                    }
                    LoadCs(customers[i]);
                    customers.Remove(customers[i]);
                    i = 0;
                }
            }
        }

        private float P(CustomerSpecial_Property customer)
        {
            float p = 0;
            //获取玩家在线的时长(分钟)
            int minutes = TimeHelp.DiffSecondByTwoDateTime(DateTime.Now, onlineTime[customer.ID - 1]) / 60;
            switch (customer.ID)
            {
                case 1:
                    p = Mathf.Pow(2, (minutes - 10) / 10 - 1);
                    break;
                case 2:
                    p = Mathf.Pow(2, (minutes - 15) / 15 - 1);
                    if (minutes < 10 && isDancerShowed)
                    {
                        p = -1;
                    }
                    break;
                case 3:
                    p = Mathf.Pow(2, (minutes - 25) / 30 - 1);
                    break;
            }
            return p;
        }

        private bool IsCreate(float time)
        {
            float rd = Random.value;
            if (rd < time)
            {
                return true;
            }
            return false;
        }

        private void LoadCs(CustomerSpecial_Property pro)
        {
            CustomerSpe cs = AssetMgr.Instance.LoadGameobjFromPool(pro.Path).GetComponent<CustomerSpe>();
            cs.ResetData();
            cs.PlayAnim();
            List<Transform> lines = pathLines[pro.RoleIndex].tfs; //随机的角色的所有路线
            int index = Random.Range(0, lines.Count);
            Transform line = lines[index];  //随机的路线

            cs.transform.position = line.GetChild(0).position; // 该路线上的第一个点为出生点
            if (!recordLineDic.ContainsKey(pro.RoleIndex))
            {
                recordLineDic.Add(pro.RoleIndex, index);
            }
            cs.curSRoleStage = SRoleStage.MoveTarget;

            cs.Data = pro;

            int count = line.childCount;
            Vector3[] pos = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                pos[i] = line.GetChild(i).position;
            }
            cs.MoveToTargetPoint(pos);
            cs.onHandle2 += MoveEndHandle;
            customerSpes.Add(cs);
        }

        public void MoveEndHandle(CustomerSpe cs)
        {
            if (cs.curSRoleStage == SRoleStage.MoveTarget)
            {
                cs.curSRoleStage = SRoleStage.Action;

                cs.gameObject.layer = 9;

                //StartCoroutine(CSAction(cs));
            }
            if (cs.curSRoleStage == SRoleStage.Leave)
            {
                customerSpes.Remove(cs);
                PoolMgr.Instance.DespawnOne(cs.transform);

                //GameObject.Destroy(cs.gameObject);
            }
        }

        private IEnumerator CSAction(CustomerSpe cs)
        {
            cs.isClicked = true;
            yield return new WaitForSeconds(20);
            cs.MHashCode = cs.GetHashCode();
            EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, cs.MHashCode);
            EventManager.Instance.TriggerEvent(EventKey.DancingGameVictory, 0);
        }
        private void FixedUpdate()
        {
            timer += Time.deltaTime;
            if (timer >= timeInterve)
            {
                timer = 0;
                //CreateCs();
                CreateRoleAtOnlineTime();
            }

            UdpateCS();
        }

        private void UdpateCS()
        {
            for (int i = 0; i < customerSpes.Count; i++)
            {
                customerSpes[i].UpdateSC();
            }
        }
    }
}
