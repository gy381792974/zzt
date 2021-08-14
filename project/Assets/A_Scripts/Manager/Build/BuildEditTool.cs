using EazyGF;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildEditTool : MonoBehaviour
{
    // Start is called before the first frame update
    public MainSpace mainSpace;

    public int level;

    [ContextMenu("EditorFillData")]
    public void EditorFillData()
    {
        mainSpace.stallList.Clear();
        for (int i = 0; i < mainSpace.stallGrid.childCount; i++)
        {
            mainSpace.stallGrid.GetChild(i).name = $"stall{i}";
            mainSpace.stallList.Add(mainSpace.stallGrid.GetChild(i).GetComponent<BuildStall>());
        }

        mainSpace.equipList.Clear();
        for (int i = 0; i < mainSpace.equipGrid.childCount; i++)
        {
            mainSpace.equipGrid.GetChild(i).name = $"equp{i}";
            mainSpace.equipList.Add(mainSpace.equipGrid.GetChild(i).GetComponent<BuildEquip>());
        }

        mainSpace.adornList.Clear();
        for (int i = 0; i < mainSpace.adornGrid.childCount; i++)
        {
            mainSpace.adornGrid.GetChild(i).name = $"adorn{i}";
            mainSpace.adornList.Add(mainSpace.adornGrid.GetChild(i).GetComponent<BuildAdorn>());
        }
    }

    [ContextMenu("ShowBuildByLevel")]
    public void ShowBuildByLevel()
    {
        if (level < 0 || level > 4)
        {
            Debug.LogError("设置的level能是1-4 当前leve =" + level);
            return;
        }

        for (int i = 0; i < mainSpace.stallList.Count; i++)
        {
            mainSpace.stallList[i].ShowBuild("", level);
        }

        for (int i = 0; i < mainSpace.equipList.Count; i++)
        {
            mainSpace.equipList[i].ShowBuild("", level);
        }

        for (int i = 0; i < mainSpace.adornList.Count; i++)
        {
            mainSpace.adornList[i].ShowBuild("", level);
        }
    }

    [ContextMenu("HideBuildShow")]
    public void HideBuildShow()
    {
        for (int i = 0; i < mainSpace.stallList.Count; i++)
        {
            mainSpace.stallList[i].HideShow();
        }

        for (int i = 0; i < mainSpace.equipList.Count; i++)
        {
            mainSpace.equipList[i].HideShow();
        }

        for (int i = 0; i < mainSpace.adornList.Count; i++)
        {
            mainSpace.adornList[i].HideShow();
        }
    }


    [ContextMenu("InitEffectPos")]
    public void InitEffectPos()
    {
        for (int i = 0; i < mainSpace.stallList.Count; i++)
        {
            mainSpace.stallList[i].InitEffectPos();
        }

        for (int i = 0; i < mainSpace.equipList.Count; i++)
        {
            mainSpace.equipList[i].InitEffectPos();
        }

        for (int i = 0; i < mainSpace.adornList.Count; i++)
        {
            mainSpace.adornList[i].InitEffectPos();
        }
    }

    [ContextMenu("SetBuildPos")]
    public void SetBuildPos()
    {
        string str = ReadFile("stall1");

        string[] mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindObj(name);

            if (pre == null)
            {
                //Debug.LogError(name + " " + i + "  no hava res");
                continue;
            }
            mainSpace.stallList[i / 4].buildPos[i % 4].localPosition = Vector3.zero;
            CreatePre(mainSpace.stallList[i / 4].buildPos[i % 4], pre, true, i % 4 == 0, (i / 4) == 11);
        }

        str = ReadFile("equip1");

        mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindObj(name);

            if (pre == null)
            {
                //Debug.LogError(name + " " + i + "  no hava res");
                continue;
            }

            bool isCreateBox = mainSpace.equipList[i / 4].GetComponent<BuildChair>() == null;

            mainSpace.equipList[i / 4].buildPos[i % 4].localPosition = Vector3.zero;
            CreatePre(mainSpace.equipList[i / 4].buildPos[i % 4], pre , isCreateBox, i % 4 == 0);
        }

        str = ReadFile("born1");

        mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            //bool isShowbox = (i/4 != 5);
            //bool isShowbox = false;

            string name = mName[i].Replace("\r", "").Replace("\t", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindObj(name);

            if (pre == null)
            {
                continue;
            }

            bool isCb = mainSpace.equipList[i / 4].GetComponent<BuildAdornCom>() == null;

            mainSpace.adornList[i / 4].buildPos[i % 4].localPosition = Vector3.zero;

            CreatePre(mainSpace.adornList[i / 4].buildPos[i % 4], pre , isCb, i % 4 == 0 && isCb);
        }

        //buildModelGrid.gameObject.SetActive(false);
    }

    [ContextMenu("SetShadow")]
    public void SetShadow()
    {

        //Transform ab = buildModel.FindChildRecursion("facility_20061 1");

        //Debug.LogError("abc" +ab.name);

        //return;

        //if (buildModelGrid == null)
        //{
        //    return;
        //}

        //buildModelGrid.gameObject.SetActive(true);

        string noHavashadown = "";

        string str = ReadFile("stall1");

        string[] mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "") + "_shadow";

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindShowObj(name);
            RemoveChild(mainSpace.stallList[i / 4].showPos[i % 4]);

            if (pre == null)
            {
                noHavashadown += name + "\n";
                continue;
            }

            CreatePre(mainSpace.stallList[i / 4].showPos[i % 4], pre);
        }


        str = ReadFile("equip1");

        mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "") + "_shadow";

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindShowObj(name);
            RemoveChild(mainSpace.equipList[i / 4].showPos[i % 4]);

            if (pre == null)
            {
                noHavashadown += name + "\n";
                continue;
            }
            CreatePre(mainSpace.equipList[i / 4].showPos[i % 4], pre);
        }


        str = ReadFile("born1");

        mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "") + "_shadow";

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindShowObj(name);
            RemoveChild(mainSpace.adornList[i / 4].showPos[i % 4]);

            if (pre == null)
            {
                noHavashadown += name + "\n";
                continue;
            }

            CreatePre(mainSpace.adornList[i / 4].showPos[i % 4], pre);
        }

        Debug.LogError(noHavashadown);
        //buildModelGrid.gameObject.SetActive(false);
    }


    public Transform mapGrid;
    [ContextMenu("HIDEMUBU")]
    private void HideMubu()
    {
        GameObject goM = FindObj("mubu", mapGrid, true);
        if (goM != null)
        {
            goM.gameObject.SetActive(false);
        }

        GameObject goJ = FindObj("jingshipai", mapGrid, true);
        if (goJ != null)
        {
            goJ.gameObject.SetActive(false);
        }
    }

    [ContextMenu("SetAllModeMuShadow")]
    private void SetAllModeMuShadow()
    {
        SetBuildPos();
        SetShadow();
        HideMubu();
    }

    public EazyGF.BuildArea ba;
    // public Transform buildAreaTf;
    //[ContextMenu("InitBuildArea")]
    //private void InitBuildArea()
    //{
    //    string str = ReadFile("stall1");

    //    string[] mName = str.Split('\n');

    //    for (int i = 0; i < 9; i++)
    //    {
    //        string name = mName[i * 4].Replace("\r", "");

    //        Debug.LogWarning(name + " " + i);

    //        GameObject pre = FindObj(name, buildAreaTf);

    //        if (pre == null)
    //        {
    //            continue;
    //        }

    //       ba.areaItems[i].transform.position = Vector3.zero;

    //       GameObject go = CreateNewPreToTargetTf(ba.areaItems[i].transform, pre, $"area");
    //       ba.areaItems[i].areaObj = go;
    //       CreateBoxByPre(ba.areaItems[i].transform, pre, "box").layer = 14;
    //    }
    //}


    public Transform stallUnLockTf;
    public Transform areaGrid;
    public Transform areaMaskGrid;

    [ContextMenu("SetAreaModel")]
    public void SetSingleBuildPos()
    {
        string str = ReadFile("stall2");

        string[] mName = str.Split('\n');

        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindObj(name, stallUnLockTf, true);

            if (pre == null)
            {
                continue;
            }

            CreateNewPreToTargetTf(mainSpace.stallList[i].lockObj.transform, pre, $"obj{i}");
            CreateBoxByPre(mainSpace.stallList[i].lockObj.transform, pre, $"box{i}").layer = 13;
        }

        str = ReadFile("area");
        mName = str.Split('\n');
        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "").Replace("\t", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = null;

            if (name == "-1")
            {
                //pre = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //pre.transform.localScale = Vector3.one * 0.01f;

                //pre.transform.position = new Vector3(-33, 11, -8);

                //pre.gameObject.SetActive(false);

                RemoveChild(ba.areaItems[i].transform);
                continue;
            }
            else
            {
                pre = FindObj(name, areaGrid, true);
            }

            if (pre == null)
            {
                continue;
            }

            ba.areaItems[i].transform.position = Vector3.zero;

            GameObject go = CreateNewPreToTargetTf(ba.areaItems[i].transform, pre, $"area");
            ba.areaItems[i].areaObj = go;

            CreateBoxByPre(ba.areaItems[i].transform, pre, "box").layer = 14;

            if (name == "-1")
            {
                GameObject.DestroyImmediate(pre.gameObject);
            }
        }

        int index = 1;
        if (index == 1)
        {
            return;
        }

        str = ReadFile("areaMask");
        mName = str.Split('\n');
        for (int i = 0; i < mName.Length; i++)
        {
            string name = mName[i].Replace("\r", "").Replace("\t", "");

            Debug.LogWarning(name + " " + i);

            GameObject pre = FindObj(name, areaMaskGrid, true);

            if (pre == null)
            {
                continue;
            }

            ba.buildMaskItems[i].transform.position = Vector3.zero;

            GameObject go = CreateNewPreToTargetTf(ba.buildMaskItems[i].transform, pre, $"areaMaks");
            ba.buildMaskItems[i].areaObj = go;

            CreateBoxByPre(ba.buildMaskItems[i].transform, pre, "box").layer = 14;
        }

    }

    #region 通用部分
    private void CreatePre(Transform tf, GameObject pre, bool isCreatBox = false, bool isCreaetUnlockBox = false, bool isUsrOrAng = false)
    {
        
        RemoveChild(tf);

        
        GameObject go = GameObject.Instantiate(pre);
        go.name = $"MBuild{pre.gameObject}";
        go.transform.SetParent(tf);

        go.transform.eulerAngles = pre.transform.eulerAngles;
        go.transform.localScale = pre.transform.localScale * 100;
        go.transform.position = pre.transform.position;

        Transform root = tf.parent.parent.parent;
        MeshRenderer mesh = pre.GetComponentInChildren<MeshRenderer>();

        Vector3 pos = pre.transform.position;
        if (mesh != null)
        {
            pos = mesh.bounds.center;
        }

        if (isCreatBox)
        {
            GameObject box = new GameObject("box");
            box.AddComponent<BoxCollider>();
            box.transform.SetParent(tf);

            box.transform.position = pos;

            BoxCollider boxDer = box.GetComponent<BoxCollider>();

            if (mesh != null)
            {
                Vector3 v = pre.GetComponentInChildren<MeshRenderer>().bounds.size;
                boxDer.size = v;
            }

            SetBoxInfo(box);
        }
    
        if (isCreaetUnlockBox)
        {
            return;

            Transform lockTf = root.GetChild(1);

            if (lockTf.gameObject.name != "Lock")
            {
                Debug.LogError("名字错误 " + lockTf.gameObject.name);
                return;
            }

            GameObject box2 = FindObj("unLockBox", lockTf, false);

            if (box2 != null)
            {
                GameObject.DestroyImmediate(box2);
            }

            if (box2 == null)
            {

                box2 = new GameObject("unLockBox");
                BoxCollider boxDer = box2.AddComponent<BoxCollider>();

                box2.transform.SetParent(lockTf);

                if (mesh != null)
                {
                    Vector3 v = pre.GetComponentInChildren<MeshRenderer>().bounds.size;
                    boxDer.size = v;
                }

                box2.transform.position = pos;
                box2.transform.eulerAngles = pre.transform.eulerAngles;

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube.transform.SetParent(box2.transform);
                cube.transform.localPosition = Vector3.zero;
                cube.transform.localEulerAngles = Vector3.zero;
            }

            SetBoxInfo(box2);
        }
    }


    public static void SetBoxInfo(GameObject box)
    {
        box.transform.localEulerAngles = new Vector3(0, 225f, 0);

        box.gameObject.tag = "Build";

        box.transform.localScale = Vector3.one * 0.7f;

        box.gameObject.layer = 13;
    }

    public Transform buildModel;
    public Transform buildShowModel;

    private GameObject FindShowObj(string name)
    {
        if (buildShowModel.FindChildRecursion(name) == null)
        {
            Debug.LogError(name + " " + "  no hava res");
            return null;
        }

        return buildShowModel.FindChildRecursion(name).gameObject;
    }

    private GameObject FindObj(string name)
    {
        if (buildModel.FindChildRecursion(name) == null)
        {
            Debug.LogError(name + " " + "  no hava res");
            return null;
        }

        return buildModel.FindChildRecursion(name).gameObject;
    }

    private GameObject FindObj(string name, Transform grid, bool isShowLog = true)
    {
        if (grid.FindChildRecursion(name) == null)
        {
            if (isShowLog)
            {
                Debug.LogError(name + " " + "  no hava res");
            }
            return null;
        }

        return grid.FindChildRecursion(name).gameObject;
    }


    public static void RemoveChild(Transform tf)
    {
        if (tf.childCount > 0)
        {  
            for (int i = tf.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(tf.GetChild(i).gameObject);
            }
        }
    }


    private static string buildNamePath = "OtherAssets/BuildConfig";
    public static string GetOtherAssetsFullPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), buildNamePath);
    }

    public static string ReadFile(string txtName)
    {
        string name = GetOtherAssetsFullPath() + "/" + txtName + ".txt";

        Debug.LogError("  " + name);

        StreamReader streamReader;
        if (File.Exists(name))
        {
            streamReader = File.OpenText(name);
            List<string> list = new List<string>();
            string lineFile;

            StringBuilder sb = new StringBuilder();

            while ((lineFile = streamReader.ReadLine()) != null)
            {
                sb.Append(lineFile);
                sb.AppendLine();
            }

            sb = sb.Remove(sb.Length - 2, 2);

            streamReader.Close();
            streamReader.Dispose();

            //Debug.LogError(sb.ToString());

            return sb.ToString();
        }
        return null;
    }

    public static GameObject CreateNewPreToTargetTf(Transform grid, GameObject pre, string name)
    {
        RemoveChild(grid);

        GameObject go = GameObject.Instantiate(pre);
        go.name = $"{name}";
        go.transform.SetParent(grid);

        go.transform.eulerAngles = pre.transform.eulerAngles;
        go.transform.localScale = pre.transform.localScale * 100;
        go.transform.position = pre.transform.position;

        return go;
    }

    //将pre的位置 旋转 缩放 给go
    public static GameObject CreateBoxByPre(Transform grid, GameObject pre, string name)
    {
        Vector3 pos = pre.transform.position;
        Vector3 eulerAngles = pre.transform.eulerAngles;
        Vector3 localScale = pre.transform.localScale * 100;

        MeshRenderer mesh = pre.GetComponentInChildren<MeshRenderer>();

        if (name == "-1")
        {
            Debug.LogError("name " + mesh);
        }

        if (mesh != null)
        {
            pos = mesh.bounds.center;
            eulerAngles = mesh.transform.eulerAngles;
            localScale = mesh.bounds.size;
        }

        GameObject go = new GameObject(name);
        go.transform.SetParent(grid);
        go.transform.position = pos;
        go.transform.eulerAngles = eulerAngles;
        go.transform.localScale = localScale;
        go.AddComponent<BoxCollider>();

        return go;
    }
    #endregion
}
