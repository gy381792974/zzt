using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

public class CameraViewMove
{
    public Vector3 point;
    public bool type;
    public CameraViewMove(Vector3 point, bool type)
    {
        this.point = point;
        this.type = type;
    }

}

[System.Serializable]
public class AreaData
{
    public Dictionary<int, BuildStatus> buildStatusDic;
    public Dictionary<int, int[]> areaDic;
}

/// <summary>
/// 弹窗管理类
/// </summary>
public class BuildUpgradeMgr : SingleClass<BuildUpgradeMgr>
{
    Dictionary<int, BuildStatus> buildStatusDic = new Dictionary<int, BuildStatus>();
    Dictionary<int, int[]> areaDic = new Dictionary<int, int[]>();
    static string fileName = "StallData.data";
    static string savePath;
    public static string SavePath
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

    private bool ReadData()
    {
        AreaData area = SerializHelp.DeserializeFileToObj<AreaData>(SavePath, out bool isSuccess);
        if (isSuccess)
        {
            this.buildStatusDic = area.buildStatusDic;
            this.areaDic = area.areaDic;
        }
        return isSuccess;
    }

    public void SaveData()
    {
        AreaData area = new AreaData();
        area.buildStatusDic = buildStatusDic;
        area.areaDic = areaDic;
        SerializHelp.SerializeFile(SavePath, area);
    }

    /// <summary>
    /// 储存当前index区域中每个item的当前等级
    /// </summary>
    /// <param name="index">区域 index</param>
    /// <param name="arr">里面所有item的当前等级</param>
    public void SaveAreaData(int index, int[] arr)
    {
        if (areaDic.ContainsKey(index))
        {
            areaDic[index] = arr;
        }
        else
        {
            areaDic.Add(index, arr);
        }
        SaveData();
    }

    public override void Init()
    {
        ReadData();
    }

    public void BuildUnLockAndUpgrade(BuildDataModel model, int index = -1)
    {
        if (model.Level < 1)
        {
            UIMgr.ShowPanel<AreaUnlockPanel>(new AreaUnlockPanelData(model));
            return;
        }

        if (model.AreaIndex == 0)
        {
            UIMgr.ShowPanel<BuildUpgradePanel>(new BuildUpgradePanelData(model));
            Vector3 point = MainSpace.Instance.stallList[model.Pos - 1].GetShowBuildBoxTf().position;
            EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2,
                new CameraViewMove(point, true));
            ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.stallList[model.Pos - 1].GetShowBuildMR());
        }
        else if (model.AreaIndex == 3 || model.AreaIndex == 1 || model.AreaIndex == 7)
        {
            List<BuildDataModel> buildDataModels = BuildMgr.GetBuildDatasByArea(model.AreaIndex);
            Debug.Log("index " + index);
            UIMgr.ShowPanel<ChairPanel>(new ChairPanelData(buildDataModels, model.Id, index));
        }
    }

    public int[] GetCurAreaItem(int areaIndex, int length)
    {
        if (areaDic.ContainsKey(areaIndex))
        {
            return areaDic[areaIndex];
        }
        else
        {
            int[] arr = new int[length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = 1;
            }
            areaDic.Add(areaIndex, arr);
            return arr;
        }
    }

    /// <summary>
    /// 获取到当前stall的状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public BuildStatus GetBuildStatusById(int id)
    {
        if (buildStatusDic.ContainsKey(id))
        {
            return buildStatusDic[id];
        }
        else
        {
            BuildStatus bs = new BuildStatus(1, 1, 1);
            buildStatusDic.Add(id, bs);
            SaveData();
            return bs;
        }
    }

    /// <summary>
    /// 获取到当前的食物售价
    /// </summary>
    /// <param name="id"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetCurLevelFoodSell(int id, int level)
    {
        StallLevel_Property stall = BuildMgr.GetStallLevelPropertyByIdAndLevel(id, level);
        BuildStatus bs = GetBuildStatusById(id);
        int foodSell = stall.FoodSell[0] * (int)Mathf.Pow(stall.FoodSell[1], bs.foodLevel - 1);
        return foodSell;
    }
    /// <summary>
    /// 获取到当前的食物等级
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCurFoodLevel(int id)
    {
        BuildStatus bs = GetBuildStatusById(id);
        return bs.foodLevel;
    }
    /// <summary>
    /// 获取当前的取餐位数量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCurTakeMealNum(int id)
    {
        BuildStatus bs = GetBuildStatusById(id);
        return bs.mealNum;
    }
    /// <summary>
    /// 获取当前的排队位数量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCurQueueNum(int id)
    {
        BuildStatus bs = GetBuildStatusById(id);
        return bs.queueNum;
    }

}
