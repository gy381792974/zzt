using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBarPathMgr : Singleton<QBarPathMgr>
{
    //酒吧寻路点容器
    private List<Vector3> m_barMoveList = new List<Vector3>();

    #region 获取酒吧寻路点
    public List<Vector3> GetBarMoveList()
    {
        if (m_barMoveList.Count <= 0)
        {
            Transform marketTrans = GetTransByName("Bar");
            m_barMoveList = GetChildTransPos(marketTrans);
        }
        return m_barMoveList;
    }

    private Transform GetTransByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if (childName != null)
        {
            Debug.Log($"找到名字:{childName}");
            return childTrans;

        }
        Debug.LogError($"找不到子节点: {childName}");
        return null;
    }

    private List<Vector3> GetChildTransPos(Transform parentTrans)
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

    #region 酒吧排队路径
    public List<Vector3> m_qbarPathList = new List<Vector3>();

    public List<Vector3> GetQBarPosList()
    {
        if (m_qbarPathList.Count <= 0)
        {
            Transform canTingTrans = GetPathChildTransByName("BarQueuePath");
            m_qbarPathList = GetPathChildTransPos(canTingTrans);
        }
        return m_qbarPathList;
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
    #endregion

    #region 酒吧排队路径状态
    public List<bool> m_qbarPathStateList = new List<bool>();

    //获取酒吧路径状态
    public List<bool> GetQBarPathSate()
    {
        if (m_qbarPathStateList.Count <= 0)
        {
            Transform trans = GetPathStateChildByName("BarQueuePath");
            m_qbarPathStateList = GetPathState(trans);
        }
        return m_qbarPathStateList;
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

    //酒吧座椅位置容器
    private List<Vector3> sitPosList = new List<Vector3>();

    #region 获取酒吧座椅坐标
    public List<Vector3> GetSitPosList()
    {
        if (sitPosList.Count <= 0)
        {
            Transform sitPosTrans = GetBarChairTransPosByName("BarSitPoint");
            sitPosList = GetBarChairTransPos(sitPosTrans);
        }
        return sitPosList;
    }

    private Transform GetBarChairTransPosByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if (childName != null)
        {
            return childTrans;
        }
        Debug.LogError($"找不到酒吧座椅坐标: {childName}");
        return null;
    }

    public List<Vector3> GetBarChairTransPos(Transform parentTrans)
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

    //超市座椅状态容器
    private List<bool> m_chairStateList = new List<bool>();

    #region 获取超市座椅状态
    public List<bool> GetSitStateList()
    {
        if (m_chairStateList.Count <= 0)
        {
            Transform trans = GetSitStateChildByName("BarSitPoint");
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
        Debug.Log($"找不到酒吧座椅状态：{name}");
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
