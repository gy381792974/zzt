using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace EazyGF
{

    [System.Serializable]
    public class StaffModel
    {
        public int id;
        public int level;
        public int skillType; // 0招募顾客 1收小费 2增加调味瓶
        public long createTime;

        public StaffModel(int id, int level, int skillType, long createTime = 0)
        {
            this.id = id;
            this.level = level;
            this.createTime = createTime;
            this.skillType = skillType;
        }
    }

    [System.Serializable]
    public class StaffSerData
    {
      public List<StaffModel> sStaffs;
    }

    public class StaffMgr : SingleClass<StaffMgr>
    {
        public List<StaffModel> staffs = new List<StaffModel>();

        private string savaDataName = "staff.data";
        private string savePath;

        public string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    savePath = UICommonUtil.GetSerSavePath(savaDataName);
                }

                return savePath;
            }
        }

        public bool ReadData()
        {
            StaffSerData serData = SerializHelp.DeserializeFileToObj<StaffSerData>(SavePath, out bool loadSuccess);

            if (loadSuccess)
            {
                staffs = serData.sStaffs;
            }

            return loadSuccess;
        }

        public void SaveData()
        {
            StaffSerData serData = new StaffSerData();
            serData.sStaffs = staffs;
            SerializHelp.SerializeFile(SavePath, serData);
        }


        public override void Init()
        {
            if (!ReadData())
            {
                staffs = new List<StaffModel>();

                Staff_Property[] sp = Staff_Data.DataArray;

                for (int i = 0; i < sp.Length; i++)
                {
                    staffs.Add(new StaffModel(sp[i].ID, 0, sp[i].SkillType));
                }

                SaveData();
            }
        }

        public void UpgradeStaff(int id)
        {
            int index = FindStaffIndex(id);
            if (index != -1)
            {
                staffs[index].level++;
                staffs[index].createTime = GameMgr.Instance.CurrentDateTime;

                EventManager.Instance.TriggerEvent(EventKey.StaffDataUpdate, staffs[index]);

                if (staffs[index].skillType == 2)
                {
                    int[] sps = GetStaffSkillParamById(id, staffs[index].level);

                    List<AwardData> ads =new List<AwardData>();

                    ads.Add(new AwardData(3, sps[0]));

                    UIMgr.ShowPanel<ComRewardPanel>(new ComRewardPanelData(ads));
                }

                SaveData();
            }
        }

        public int[] GetStaffSkillParamById(int id, int level)
        {
            Staff_Level_Property[] arrs = Staff_Level_Data.DataArray;

            for (int i = 0; i < arrs.Length; i++)
            {
                if (arrs[i].ID == id && arrs[i].Level == level)
                {
                    return arrs[i].SkllParam;
                }
            }

            return null;
        }

        public int FindStaffIndex(int id)
        {
            for (int i = 0; i < staffs.Count; i++)
            {
                if (staffs[i].id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public StaffModel FindStaffById(int id)
        {
            for (int i = 0; i < staffs.Count; i++)
            {
                if (staffs[i].id == id)
                {
                    return staffs[i];
                }
            }

            return null;
        }


        public Staff_Level_Property GetStaffLevelDataById(int id)
        {
            StaffModel staff = staffs[FindStaffIndex(id)];

            Staff_Level_Property[] arrs = Staff_Level_Data.DataArray;

            for (int i = 0; i < arrs.Length; i++)
            {
                if (arrs[i].ID == id && arrs[i].Level == staff.level)
                {
                    return arrs[i];
                }
            }

            Debug.LogError($"id = {id} level= {staff.level} 数据不存在");
            return null;
        }

        public Staff_Level_Property GetStaffLevelDataByIdAndLevel(int id, int level)
        {
            Staff_Level_Property[] arrs = Staff_Level_Data.DataArray;

            for (int i = 0; i < arrs.Length; i++)
            {
                if (arrs[i].ID == id && arrs[i].Level == level)
                {
                    return arrs[i];
                }
            }

            Debug.LogError($"id = {id} level= {level} 数据不存在");
            return null;
        }

        public Staff_Property GetBaseStaffDataById(int id)
        {
            Staff_Property staff_Property = Staff_Data.GetStaff_DataByID(id);

            if (staff_Property == null)
            {
                Debug.LogError($"id = {id} 数据不存在");
                return null;
            }

            return staff_Property;
        }
    }
}