using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS_Mgr 
{
    private static List<BFSNode> findedList = new List<BFSNode>();
    private static Vector3 endPos;
    private static int nodeLenth;
    private static BFSNode[] m_BFSNodePosArray;

    public static List<Vector3> GetMoveList(BFSNode[] BFSNodePosArray, Vector3 startPos, Vector3 targetPos)
    {
        m_BFSNodePosArray = BFSNodePosArray;
        nodeLenth = m_BFSNodePosArray.Length;
        GetStartAndEndCircleIndex(startPos, targetPos, out int startIndex, out int endIndex);
        return CaculateMoveList(startIndex, endIndex);
    }

    public static List<Vector3> GetMoveList(BFSNode[] BFSNodePosArray, Transform startPos, Transform targetPos)
    {
        m_BFSNodePosArray = BFSNodePosArray;
        nodeLenth = m_BFSNodePosArray.Length;
        GetStartAndEndCircleIndex(startPos.transform.position, targetPos.transform.position, out int startIndex, out int endIndex);
        return CaculateMoveList(startIndex, endIndex);
    }

    public static List<Vector3> GetMoveList(BFSNode[] BFSNodePosArray, int startIndex, int endIndex)
    {
        m_BFSNodePosArray = BFSNodePosArray;
        nodeLenth = m_BFSNodePosArray.Length;
        return CaculateMoveList(startIndex, endIndex);
    }
    public static List<Vector3> GetRandomMoveList(BFSNode[] BFSNodePosArray, Vector3 startPos)
    {
        m_BFSNodePosArray = BFSNodePosArray;
        nodeLenth = m_BFSNodePosArray.Length;
        int randomEndIndex = Random.Range(0, nodeLenth);
        GetStartAndEndCircleIndex(startPos, BFSNodePosArray[randomEndIndex].Pos, out int startIndex, out int endIndex);
        return CaculateMoveList(startIndex, randomEndIndex);
    }

    private static List<Vector3> CaculateMoveList(int startIndex,int endIndex)
    {
        BFSNode firstNode = m_BFSNodePosArray[startIndex];
        BFSNode targetNode = m_BFSNodePosArray[endIndex];
        if (firstNode == targetNode)
        {
            List<Vector3> oneList = new List<Vector3>();
            oneList.Add(firstNode.Pos);
            return oneList;
        }
        findedList.Clear();
        for (int i = 0; i < m_BFSNodePosArray.Length; i++)
        {
            m_BFSNodePosArray[i].finded = false;
        }

        firstNode.finded = true;
        findedList.Add(firstNode);
        endPos = targetNode.Pos;
        FindPath(firstNode.Pos);
        List<Vector3> tempList = new List<Vector3>(findedList.Count);
        for (int i = 0; i < findedList.Count; i++)
        {
            tempList.Add(findedList[i].Pos);
        }
        return tempList;
    }

    private static void FindPath(Vector3 startPos)
    {
        BFSNode bfsNode = GetCloseTransArray(startPos);

        if (bfsNode != null)
        {
            findedList.Add(bfsNode);
            //是否是终点
            if (Vector3.Distance(bfsNode.Pos, endPos) < 0.5f)
            {
                return;
            }
            FindPath(bfsNode.Pos);
        }
    }

    private static BFSNode GetCloseTransArray(Vector3 startPos)
    {
        List<BFSNode> tempList = new List<BFSNode>();
        //找到所有单位距离内的点
        for (int i = 0; i < nodeLenth; i++)
        {
            float dis = Vector3.Distance(m_BFSNodePosArray[i].Pos, startPos);
            if (!m_BFSNodePosArray[i].finded && dis < 2)
            {
                tempList.Add(m_BFSNodePosArray[i]);
            }
        }

        //在符合的范围内找到最靠近目的地的那一个点
        if (tempList.Count >= 1)
        {
            BFSNode bfsNode = tempList[0];
            if (tempList.Count == 1)
            {
                bfsNode.finded = true;
                return bfsNode;
            }
            float minDis1 = Vector3.Distance(bfsNode.Pos, endPos);//当前节点到终点的距离
            for (int i = 1; i < tempList.Count; i++)
            {
                float nextDis1 = Vector3.Distance(tempList[i].Pos, endPos);
                if (nextDis1 < minDis1)
                {
                    minDis1 = nextDis1;
                    bfsNode = tempList[i];
                }
            }
            bfsNode.finded = true;
            return bfsNode;
        }
        return null;
    }

    private static void GetStartAndEndCircleIndex(Vector3 startPos, Vector3 targetPos, out int startIndex, out int endIndex)
    {
        startIndex = 0;
        endIndex = 0;

        float startMinDis = Vector3.Distance(startPos, m_BFSNodePosArray[0].Pos);
        float endMinDis = Vector3.Distance(targetPos, m_BFSNodePosArray[0].Pos);
        int lenth = m_BFSNodePosArray.Length;
        //从圈圈中找出离开始点、结束点最近的点的下标
        for (int i = 1; i < lenth; i++)
        {
            float nextStartMinDis = Vector3.Distance(startPos, m_BFSNodePosArray[i].Pos);
            if (nextStartMinDis < startMinDis)
            {
                startMinDis = nextStartMinDis;
                startIndex = i;
            }

            float nextEndMinDis = Vector3.Distance(targetPos, m_BFSNodePosArray[i].Pos);
            if (nextEndMinDis < endMinDis)
            {
                endMinDis = nextEndMinDis;
                endIndex = i;
            }
        }
    }
}
