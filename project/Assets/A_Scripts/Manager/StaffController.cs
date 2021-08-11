using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

[System.Serializable]
public class StaffData
{
    public Dictionary<int, int> staffDatas;
}
public class StaffController : MonoBehaviour
{
    static Dictionary<int, int> dataDic = new Dictionary<int, int>();
    #region
    static string fileName = "m_staffData.data";
    static string savePath;

    static string SavePath
    {
        get
        {
            if (savePath.IsNullOrEmpty())
            {
                savePath = UICommonUtil.GetSerSavePath(fileName);
            }
            return savePath;
        }
    }

    public static void SaveData()
    {
        StaffData data = new StaffData();
        data.staffDatas = dataDic;
        SerializHelp.SerializeFile(SavePath, data);
    }

    private bool ReadData()
    {
        StaffData data = SerializHelp.DeserializeFileToObj<StaffData>(SavePath, out bool isSuccess);
        if (isSuccess)
            dataDic = data.staffDatas;
        return isSuccess;
    }
    #endregion
    StaffBehaviour staffBehaviour;
    Dictionary<int, StaffBehaviour> staffsDic = new Dictionary<int, StaffBehaviour>();
    List<StaffModel> staffs;
    [SerializeField] Transform[] cookPath;
    void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        EventManager.Instance.RegisterEvent(EventKey.StaffDataUpdate, CreateStaff);
    }
    private void Init()
    {
        ReadData();
        staffs = StaffMgr.Instance.staffs;
        for (int i = 0; i < staffs.Count; i++)
        {
            if (staffs[i].level < 1)
                continue;
            Staff_Property property = StaffMgr.Instance.GetBaseStaffDataById(staffs[i].id);
            staffBehaviour = GetStaff(staffs[i].id, property.Path);
            Staff_Level_Property staff_Level = StaffMgr.Instance.GetStaffLevelDataByIdAndLevel(staffs[i].id, staffs[i].level);
            staffBehaviour.InitStaffSkill(staff_Level.SkllParam);
            InitStaffData(staffs[i].id);
        }
    }

    private void CreateStaff(object arg)
    {
        StaffModel staffModel = (StaffModel)arg;
        if (staffModel.level == 1)
        {
            Staff_Property property = StaffMgr.Instance.GetBaseStaffDataById(staffModel.id);
            GetStaff(staffModel.id, property.Path);
            InitStaffData(staffModel.id);
        }
        Staff_Level_Property level_Property = StaffMgr.Instance.GetStaffLevelDataByIdAndLevel(staffModel.id, staffModel.level);
        staffsDic[staffModel.id].InitStaffSkill(level_Property.SkllParam);
    }

    private StaffBehaviour GetStaff(int id, string path)
    {
        if (staffsDic.ContainsKey(id))
        {
            return staffsDic[id];
        }
        Transform staff = AssetMgr.Instance.LoadGameobj(path);
        staff.SetParent(transform);
        StaffBehaviour behaviour = staff.GetComponent<StaffBehaviour>();
        staffsDic.Add(id, behaviour);
        return behaviour;
    }

    private void InitStaffData(int id)
    {
        #region
        //switch (id)
        //{
        //    case 0:

        //        break;
        //    case 1:
        //        Waiter waiter = (Waiter)staffsDic[id];
        //        if (dataDic.ContainsKey(id))
        //        {
        //            waiter.InitData(dataDic[id], id);
        //        }
        //        else
        //        {
        //            waiter.InitData(0, id);
        //        }
        //        break;
        //    case 2:
        //        Cook cook = (Cook)staffsDic[id];
        //        cook.SetCookMoveTarget(cookPath[0]);
        //        break;
        //    case 3:
        //        Cook cook2 = (Cook)staffsDic[id];
        //        cook2.SetCookMoveTarget(cookPath[1]);
        //        break;
        //    case 4:
        //        Cook cook3 = (Cook)staffsDic[id];
        //        cook3.SetCookMoveTarget(cookPath[2]);
        //        break;
        //    case 5:

        //        break;
        //    case 6:

        //        break;
        //    case 7:

        //        break;

        //}
        #endregion
        if (staffsDic[id] is Waiter)
        {
            Waiter waiter = staffsDic[id] as Waiter;
            if (dataDic.ContainsKey(id))
            {
                waiter.InitData(dataDic[id], id);
            }
            else
            {
                waiter.InitData(0, id);
            }
        }
        else if (staffsDic[id] is Cook)
        {
            Cook cook = staffsDic[id] as Cook;
            cook.SetCookMoveTarget(cookPath[0]);
        }

    }

    public static void SaveStaffData(int id, int num)
    {
        if (!dataDic.ContainsKey(id))
        {
            dataDic.Add(id, num);
        }
        else
        {
            dataDic[id] = num;
        }
        SaveData();
    }


}
