using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTingPathMgr : Singleton<CanTingPathMgr>
{
    // 餐厅寻路点容器
    public List<Vector3> m_canTingMoveList=new List<Vector3>();

    #region 获取餐厅寻路点
    public List<Vector3> GetCanTingMoveList()
    {
        if (m_canTingMoveList.Count <= 0)
        {
            Transform canTingTrans = GetChildTransByName("CanTing");
            m_canTingMoveList = GetChildTransPos(canTingTrans);
        }
        return m_canTingMoveList;
    }

    private Transform GetChildTransByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if(childName!=null)
        {
            return childTrans;
        }
        Debug.LogError($"找不到寻路点: {childName}");
        return null;
    }

    private List<Vector3> GetChildTransPos(Transform parentTrans)
    {
        Transform[] allChildTrans = parentTrans.GetComponentsInChildren<Transform>();
        int count = allChildTrans.Length;
        List<Vector3> tempList = new List<Vector3>(count - 1);
        for (int i=1;i< count;i++)
        {
            tempList.Add(allChildTrans[i].position);
        }
        return tempList;
    }
    #endregion

    public List<Vector3> m_canTingPathList = new List<Vector3>();

    public List<Vector3> GetCanTingPosList()
    {
        if (m_canTingPathList.Count <= 0)
        {
            Transform canTingTrans = GetPathChildTransByName("CanTingQueuePath");
            m_canTingPathList = GetPathChildTransPos(canTingTrans);
        }
        return m_canTingPathList;
    }

    private Transform GetPathChildTransByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if (childName != null)
        {
            return childTrans;
        }
        Debug.LogError($"找不到寻路点: {childName}");
        return null;
    }

    private List<Vector3> GetPathChildTransPos(Transform parentTrans)
    {
        Transform[] allChildTrans = parentTrans.GetComponentsInChildren<Transform>();
        int count = allChildTrans.Length;
        List<Vector3> tempList = new List<Vector3>(count - 1);
        for (int i = 1; i < count; i++)
        {
            tempList.Add(allChildTrans[i].position);
        }
        return tempList;
    }

    #region 餐厅路径点状态
    public List<bool> m_canTingPathStateList = new List<bool>();

    //获取餐厅路径状态
    public List<bool> GetCanTingPathSate()
    {
        if(m_canTingPathList.Count<=0)
        {
            Transform trans = GetPathStateChildByName("CanTingQueuePath");
            m_canTingPathStateList = GetPathState(trans);
        }
        return m_canTingPathStateList;
    }

    private Transform GetPathStateChildByName(string name)
    {
        Transform childTrans = transform.Find(name);
        if (childTrans != null)
        {
            return childTrans;
        }
        Debug.Log($"找不到子节点：{name}");
        return null;
    }

    private List<bool> GetPathState(Transform parent)
    {
        PathState[] allSitTrans = parent.GetComponentsInChildren<PathState>();
        int count = allSitTrans.Length;
        List<bool> tempList = new List<bool>();
        for (int i = 0; i < count; i++)
        {
            tempList.Add(allSitTrans[i].isAnyOne);
        }
        return tempList;
    }

    #endregion

    //座椅位置容器
    public List<Vector3> sitPosList = new List<Vector3>();

    #region 获取餐厅座椅
    public List<Vector3> GetSitPosList()
    {
        if (sitPosList.Count <= 0)
        {
            Transform sitPosTrans = GetChairTransPosByName("CanTingSitPoint");
            sitPosList = GetChairTransPos(sitPosTrans);
        }
        return sitPosList;
    }

    private Transform GetChairTransPosByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if (childName != null)
        {
            return childTrans;
        }
        Debug.LogError($"找不到座椅坐标: {childName}");
        return null;
    }

    private List<Vector3> GetChairTransPos(Transform parentTrans)
    {
        Transform[] allChildTrans = parentTrans.GetComponentsInChildren<Transform>();
        int count = allChildTrans.Length;
        List<Vector3> tempList = new List<Vector3>(count - 1);

        for (int i = 1; i < count; i++)
        {
            tempList.Add(allChildTrans[i].position);
        }
        return tempList;
    }
    #endregion

    //餐厅座椅状态容器（坐下/没坐）
    public List<bool> m_chairStateList = new List<bool>();

    #region 获取座椅状态(是否空位)
    public List<bool> GetSitStateList()
    {
        if (m_chairStateList.Count <= 0)
        {
            Transform trans = GetSitStateChildByName("CanTingSitPoint");
            m_chairStateList = GetSitState(trans);
        }
        return m_chairStateList;
    }

    private Transform GetSitStateChildByName(string name)
    {
        Transform childTrans = transform.Find(name);
        if (childTrans != null)
        {
            return childTrans;
        }
        Debug.Log($"找不到子节点：{name}");
        return null;
    }

    private List<bool> GetSitState(Transform parent)
    {
        ChairState[] allSitTrans = parent.GetComponentsInChildren<ChairState>();
        int count = allSitTrans.Length;
        List<bool> tempList = new List<bool>();
        for (int i = 0; i < count; i++)
        {
            tempList.Add(allSitTrans[i].isSit);
        }
        return tempList;
    }
    #endregion
}
