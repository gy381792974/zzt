using EazyGF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTBuildStallPosEdit : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform buildGrid;

    public Transform originalGrid;

    public Transform targetGrid;

    [ContextMenu("InitOriginaModelPos")]
    public void InitOriginaModelPos()
    {
        for (int i = 0; i < originalGrid.childCount; i++)
        {
            Transform tf = originalGrid.GetChild(i);
            if (i < buildGrid.childCount)
            {
                tf.gameObject.SetActive(true);
                
                BuildItem buildStall = buildGrid.GetChild(i).GetComponent<BuildItem>();
                if (buildStall.buildPos[0].childCount > 0)
                {
                    originalGrid.GetChild(i).transform.position = buildStall.buildPos[0].GetChild(0).GetComponentInChildren<MeshRenderer>().bounds.center;
                    originalGrid.GetChild(i).transform.name = $"cube__Ready_pos_{i}";
                }
            }
            else
            {
                tf.gameObject.SetActive(false);
            }
        }
    }


    [ContextMenu("CopyPos")]
    public void CopyPos()
    {
        for (int i = 0; i < targetGrid.childCount; i++)
        {
            Transform tf = originalGrid.GetChild(i);

            targetGrid.GetChild(i).transform.position = tf.transform.position;
            targetGrid.GetChild(i).transform.name = $"pos{i}";
        }
    }


    public Transform mBuyFoodPosGrid;
    [ContextMenu("PrintStallTeamNum")]
    public void PrintStallTeamNum()
    {
        string str = "\n";
        for (int i = 0; i < mBuyFoodPosGrid.GetChild(0).childCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Transform tf = mBuyFoodPosGrid.GetChild(j).GetChild(i);

                str += tf.childCount + "\t\n";
            }
        }

        Debug.LogError(str);
    }

    [ContextMenu("PrintStallQueueNum")]
    public void PrintStallQueueNum()
    {
        string str = "\n";
        for (int i = 0; i < mBuyFoodPosGrid.GetChild(0).childCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Transform tf = mBuyFoodPosGrid.GetChild(j).GetChild(i).GetChild(0);

                str += (tf.childCount - 1) + "\t\n";
            }
        }

        Debug.LogError(str);
    }


    public List<Transform> yiGrid;
    [ContextMenu("PrintYZ")]
    public void PrintYZ()
    {
        string str = "\n";

        int yzNum = yiGrid[0].childCount;
        int level = yiGrid.Count;

        for (int i = 0; i < yzNum; i++)
        {
            for (int j = 0; j < level; j++)
            {
                Transform tf = yiGrid[j].GetChild(i);

                str += tf.name + "\t\n";
            }
        }

        Debug.LogError(str);
    }

    public int GetChildNum(Transform tf)
    {
        int childNum = 0;
        for (int i = 0; i < tf.childCount; i++)
        {
            Transform child = tf.GetChild(i);
            childNum += child.childCount;
        }

        return childNum;
    }


    [ContextMenu("CopyPreTargetTf")]
    public void CopyPre()
    {
        CreatePreByPre(copyToTf, targetPre.gameObject);
    }

    public Transform targetPre;
    public Transform copyToTf;
    private void CreatePreByPre(Transform tf, GameObject pre)
    {
        RemoveChild(tf);

        GameObject go = GameObject.Instantiate(pre, tf);
        go.name = $"2{pre.gameObject}";

        //go.transform.SetParent(tf);
        //go.transform.localEulerAngles = new Vector3(0, 225f, 0);
        //go.transform.localScale = Vector3.one * 100;

        go.transform.position = pre.transform.position;
        go.transform.eulerAngles = pre.transform.eulerAngles;
        go.transform.localScale = Vector3.one * 100;
    }

    private void RemoveChild(Transform tf)
    {
        if (tf.childCount > 0)
        {
            for (int i = tf.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(tf.GetChild(i).gameObject);
            }
        }
    }

}
