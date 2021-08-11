using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeePathMgr : Singleton<CoffeePathMgr>
{
    //咖啡寻路点容器
    private List<Vector3> m_coffeeMoveList = new List<Vector3>();

    #region 获取咖啡寻路点
    public List<Vector3> GetCoffeeMoveList()
    {
        if (m_coffeeMoveList.Count <= 0)
        {
            Transform coffeeTrans = GetTransByName("Coffee");
            m_coffeeMoveList = GetChildTransPos(coffeeTrans);
        }
        return m_coffeeMoveList;
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


    #region 咖啡厅排队路径
    public List<Vector3> m_canTingPathList = new List<Vector3>();

    public List<Vector3> GetCanTingPosList()
    {
        if (m_canTingPathList.Count <= 0)
        {
            Transform canTingTrans = GetPathChildTransByName("CoffeeQueuePath");
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
    #endregion

    #region 咖啡路径点状态
    public List<bool> m_canTingPathStateList = new List<bool>();

    //获取餐厅路径状态
    public List<bool> GetCanTingPathSate()
    {
        if (m_canTingPathList.Count <= 0)
        {
            Transform trans = GetPathStateChildByName("CoffeeQueuePath");
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

    //咖啡厅座椅位置容器
    private List<Vector3> sitPosList = new List<Vector3>();

    #region 获取咖啡厅座椅坐标
    public List<Vector3> GetSitPosList()
    {
        if (sitPosList.Count <= 0)
        {
            Transform sitPosTrans = GetCoffeeChairTransPosByName("CoffeeSitPoint");
            sitPosList = GetCoffeeChairTransPos(sitPosTrans);
        }
        return sitPosList;
    }

    private Transform GetCoffeeChairTransPosByName(string childName)
    {
        Transform childTrans = transform.Find(childName);
        if (childName != null)
        {
            return childTrans;
        }
        Debug.LogError($"找不到咖啡厅座椅坐标: {childName}");
        return null;
    }

    public List<Vector3> GetCoffeeChairTransPos(Transform parentTrans)
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

    //咖啡座椅状态容器
    private List<bool> m_chairStateList = new List<bool>();

    #region 获取咖啡厅座椅状态
    public List<bool> GetSitStateList()
    {
        if (m_chairStateList.Count <= 0)
        {
            Transform trans = GetSitStateChildByName("CoffeeSitPoint");
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
        Debug.Log($"找不到咖啡厅座椅状态：{name}");
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
