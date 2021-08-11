using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CubeEditTool : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform cridLayout;
    public List<CubeLayout> clList;
    public CubeEditItem cubeGameItem;

    [ContextMenu("InitLayoutData")]
    public void InitLayoutData()
    {
        clList.Clear();
        for (int i = 0; i < cridLayout.childCount; i++)
        {
            clList.Add(cridLayout.GetChild(i).GetComponent<CubeLayout>());
            cridLayout.GetChild(i).gameObject.name = "layout" + (i + 1);
        }
    }

   [ContextMenu("InInitItem")]
    public void CubeLevelEdit()
    {
        for (int i = 0; i < clList.Count; i++)
        {
            CubeLayout cubeLayout =  clList[i];
            cubeLayout.posTfs.Clear();
            for (int j = 0; j < cubeLayout.grid.childCount; j++)
            {
                Transform tf = cubeLayout.grid.GetChild(j);
                cubeLayout.posTfs.Add(tf);

                tf.name = "pos" + j;
                if (tf.childCount >= 0)
                {
                    RemoveAllChild(tf);
                }

                CubeEditItem item = GameObject.Instantiate(cubeGameItem, tf);
                item.gameObject.name = $"{i}cubeItem{j}";

                item.gameObject.SetActive(true);
                item.SetSelect(false);

                item.transform.localPosition = Vector3.zero;
            }
        }
    }

    private void RemoveAllChild(Transform tf)
    {
        for (int i = tf.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(tf.GetChild(i).gameObject);
        }
    }

    [ContextMenu("PrintLevelData")]
    public void PrintLevelData()
    {
        int level = 0;
        if(int.TryParse(levelIf.text, out int res))
        {
            level = res;
        }
        else
        {
            logInputField.text = "请输入正常关卡";
            return;
        }

        int cubeNum = 0;

        string resLog = "";

        for (int i = 0; i < clList.Count; i++)
        {
            CubeLayout cubeLayout = clList[i];

            int curCubeNum = 0;

            if (cubeLayout.gameObject.activeSelf)
            {
                string log = levelIf.text + "\t" + (i + 1) + "\t";

                for (int j = 0; j < cubeLayout.posTfs.Count; j++)
                {
                    if (cubeLayout.posTfs[j].GetChild(0).GetChild(1).gameObject.activeSelf)
                    {
                        log += j + "_";
                        cubeNum++;
                        curCubeNum++;
                    }
                }

                log = log.Remove(log.Length - 1);

                log += $"\t{cubeLayout.transform.localPosition.x}\t{cubeLayout.transform.localPosition.y}\t{curCubeNum}";

                Debug.LogError(log);

                resLog += log + "\n";
            }
        }

        if (cubeNum % 2 == 0)
        {
            WriteDictionaryConent($"cubeLeave{level}.txt", resLog);
            logInputField.text = $"写入成功 方块总数为 = {cubeNum}";
        }
        else
        {
            logInputField.text = $"写入失败 方块总数为 = {cubeNum}";
        }
    }

    public static void WriteDictionaryConent(string name, string sb)
    {
        string dir = GetOtherAssetsFullPath();

        //Debug.LogError($"Write path {dir}");

        StreamWriter streamWriter;
        FileInfo fileInfo = new FileInfo(dir + name);

        if (File.Exists(fileInfo.FullName))
        {
            File.Delete(fileInfo.FullName);
        }

        streamWriter = fileInfo.CreateText();

        streamWriter.Write(sb);

        streamWriter.Close();
        streamWriter.Dispose();
    }

    private static string buildNamePath = "OtherAssets/CubeData";
    //private static string cubeDirPath = Application.dataPath + "/Cube";

    public static string GetOtherAssetsFullPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), Application.dataPath + "/Resources/");
        //return Path.Combine(Directory.GetCurrentDirectory(), Application.dataPath + buildNamePath);
    }


    public List<Button> layoutSelBtns;
    public List<GameObject> selectTags;

    public Transform selectBtnGrid;
    public Transform selectTagsGrid;

    [ContextMenu("InitSelBtnAndTag")]
    public void InitSelBtnAndTag()
    {
        layoutSelBtns.Clear();
        for (int i = 0; i < selectBtnGrid.childCount; i++)
        {
            layoutSelBtns.Add(selectBtnGrid.GetChild(i).GetComponent<Button>());

            selectBtnGrid.GetChild(i).gameObject.name = "Btn" + (i);

            selectBtnGrid.GetChild(i).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
        }

        selectTags.Clear();
        for (int i = 0; i < selectTagsGrid.childCount; i++)
        {
            selectTags.Add(selectTagsGrid.GetChild(i).gameObject);

            selectTagsGrid.GetChild(i).gameObject.name = "tag" + (i);
        }
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

    public static string ReadFile2(string txtName)
    {
        string name = GetOtherAssetsFullPath() + txtName;

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

            //sb = sb.Remove(sb.Length - 2, 2);

            streamReader.Close();
            streamReader.Dispose();

            Debug.LogError(sb.ToString());

            return sb.ToString();
        }
        return null;
    }

    public Button clearCurLBtn;
    public Button copyLastLDataBtn;
    public Button EnterBtn;
    public Button EnterAllLevelBtn;

    public Button xOffBtnAdd;
    public Button yOffBtnAdd;

    public Button xOffBtnJ;
    public Button yOffBtnJ;


    public InputField logInputField;
    public InputField levelIf;
    int curLayout = 0;

    public InputField offxIF;
    public InputField offyIF;

    void Start()
    {
        clearCurLBtn.onClick.AddListener(ClearCurLDataBtn);
        copyLastLDataBtn.onClick.AddListener(CopyLastLayoutDataBtn);
        EnterBtn.onClick.AddListener(EnterBtnClick);

        EnterAllLevelBtn.onClick.AddListener(WriteAllLevelDataClick);

        yOffBtnAdd.onClick.AddListener(YOffBtnAClick);
        yOffBtnJ.onClick.AddListener(YOffBtnJClick);

        xOffBtnAdd.onClick.AddListener(XOffBtnAClick);
        xOffBtnJ.onClick.AddListener(XOffBtnJClick);

        for (int i = 0; i < layoutSelBtns.Count; i++)
        {
            int index = i;

            layoutSelBtns[index].onClick.AddListener(()=> {
                OnSelectLayoutOnClick(index);
            });
        }
        for (int i = 0; i < selectTags.Count; i++)
        {
            selectTags[i].gameObject.SetActive(false);
        }

        InitLayout();
        OnSelectLayoutOnClick(0);
        ClearLayoutData(curLayout);

        offxIF.onEndEdit.AddListener((var) =>
        {
            SetOffset();
        });


        offyIF.onEndEdit.AddListener((var) =>
        {
            SetOffset();
        });
    }


    private void YOffBtnAClick()
    {
        if (int.TryParse(offyIF.text, out int r))
        {
            int v = r + 45;

            offyIF.text = v.ToString();

            SetOffset();
        }
    }

    private void YOffBtnJClick()
    {
       
        if (int.TryParse(offyIF.text, out int r))
        {
            int v = r - 45;

            offyIF.text = v.ToString();

            SetOffset();
        }

    }

    private void XOffBtnAClick()
    {
        if (int.TryParse(offxIF.text, out int r))
        {
            int v = r + 40;

            offxIF.text = v.ToString();

            SetOffset();
        }
    }

    private void XOffBtnJClick()
    {
        if (int.TryParse(offxIF.text, out int r))
        {
            int v = r - 40;

            offxIF.text = v.ToString();
            SetOffset();
        }
    }

    private void SetOffset()
    {
        if (curLayout >= 0)
        {
            clList[curLayout].transform.localPosition = new Vector2(offxIF.text.ToInt(), offyIF.text.ToInt());
        }
    }

    private void EnterBtnClick()
    {
        PrintLevelData();
    }

    private void InitLayout()
    {
        for (int i = 0; i < clList.Count; i++)
        {
            clList[i].gameObject.SetActive(false);
        }
    }

    private void WriteAllLevelDataClick()
    {
        int level = levelIf.text.ToInt();

        string res = "";
        for (int i = 1; i <= level; i++)
        {
            res += ReadFile2($"cubeLeave{i}.txt");
        }
        res = res.Remove(res.Length - 2, 2);

        Debug.LogError("WriteAllLevelDataClick " + res);
        WriteDictionaryConent($"cubeLeaveAll.txt", res);

        logInputField.text = "写入成功";
    }

    private void OnSelectLayoutOnClick(int index)
    {
        curLayout = index;

        selectTags[index].gameObject.SetActive(!selectTags[index].gameObject.activeSelf);

        clList[index].gameObject.SetActive(selectTags[index].gameObject.activeSelf);

        int layout = -1;
        for (int i = clList.Count - 1; i >= 0; i--)
        {
            if (clList[i].gameObject.activeSelf)
            {
                layout = i;
                break;
            }
        }

        curLayout = layout;

        if (curLayout != -1)
        {
            offxIF.text = clList[curLayout].transform.localPosition.x.ToString();
            offyIF.text = clList[curLayout].transform.localPosition.y.ToString();

            SetOffset();
        }
    }

    private void ClearCurLDataBtn()
    {
        ClearLayoutData(curLayout);
        logInputField.text = "";
    }

    private void CreateAllClearData()
    {
        for (int i = 0; i < clList.Count; i++)
        {
            ClearLayoutData(i);
        }
    }

    private void ClearLayoutData(int layout)
    {
        if (layout > -1)
        {
            CubeLayout cubeLayout = clList[layout];

            if (cubeLayout != null)
            {
                for (int i = 0; i < cubeLayout.posTfs.Count; i++)
                {
                    //Debug.LogError($"i{i}");
                    cubeLayout.posTfs[i].GetChild(0).GetComponent<CubeEditItem>().ClearData();
                }
            }
        }
    }

    private void CopyLastLayoutDataBtn()
    {
        if (curLayout >= 1)
        {
            CubeLayout cubeLayout = clList[curLayout];
            CubeLayout lastLayoutData = clList[curLayout - 1];
            cubeLayout.transform.position = lastLayoutData.transform.position;

            if (cubeLayout != null)
            {
                for (int i = 0; i < cubeLayout.posTfs.Count; i++)
                {
                    CubeEditItem curDataItem = cubeLayout.posTfs[i].GetChild(0).GetComponent<CubeEditItem>();
                    CubeEditItem lastDataItem = lastLayoutData.posTfs[i].GetChild(0).GetComponent<CubeEditItem>();

                    curDataItem.SetSelect(lastDataItem.iconObj.gameObject.activeSelf);
                }
            }
        }
    }
}
