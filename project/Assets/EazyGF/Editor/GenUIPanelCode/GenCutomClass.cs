using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class GenCutomClass : Editor
{
    [MenuItem("GameObject/生成UI自定义面板", false, 15)]
    public static void StartGenCutomClass()
    {
        if (Selection.gameObjects.Length > 0)
        {
            GameObject selectObj = Selection.gameObjects[0];

            if (!selectObj.name.Contains(GenUIPanelCode.customClassPostfix))
            {
                return;
            }
            if (PrefabUtility.IsAnyPrefabInstanceRoot(selectObj))
            {
                Debug.LogError("该命令只能使用在次级面板，主面板无法使用！");
                return;
            }

            Transform prefabTransform = GenUIPanelCode.FindUpPrefabParent(selectObj.transform);
            if (prefabTransform == null)
            {
                return;
            }

            DirectoryInfo directoryInfo=new DirectoryInfo(AB_ResFilePath.uiPanelPrefabsRootDir);
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.prefab");
                for (int i = 0; i < fileInfos.Length; i++)
                {
                    if (prefabTransform.name.ToLower().Equals(Path.GetFileNameWithoutExtension(fileInfos[i].FullName).ToLower()))
                    {
                        GameObject obj =AssetDatabase.LoadAssetAtPath<GameObject>(WriteFileHelp.ObsPathToRelativePath(fileInfos[i].FullName));
                        if (obj != null)
                        {
                            Transform selectPrefabTrans = obj.transform.Find(selectObj.name);
                            if (selectPrefabTrans != null)
                            {
                                AutoExportSetting.Instance.SelectGameObjects.Add(selectPrefabTrans.gameObject);
                                WriteFile(selectObj.transform);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public static void WriteFile(Transform selecTransform)
    {
        Transform parentPrefab = GenUIPanelCode. FindUpPrefabParent(selecTransform);
        if (parentPrefab == null)
        {
            return;
        }
        string folderPath = GenUIPanelCode.uiPanelCodeFolder +"/"+ parentPrefab.name;
        folderPath.CreateDirIfNotExists();

        //主文件路径
        string mainFilePath = folderPath + "/" + selecTransform.name + ".cs";
        if (!File.Exists(mainFilePath))
        {
            WriteMainFile(selecTransform.name, mainFilePath);
        }

        string designerFilePath = folderPath + "/" + selecTransform.name + ".Property.cs";
        WriteDesignerFile(selecTransform, designerFilePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //写主文件
    private static void WriteMainFile(string panelName, string filePath)
    {
        StringBuilder mainStringBuilder = new StringBuilder(50);
        mainStringBuilder.AppendLine("using System.Collections;");
        mainStringBuilder.AppendLine("using System.Collections.Generic;");
        mainStringBuilder.AppendLine("using UnityEngine;");
        mainStringBuilder.AppendLine("using UnityEngine.UI;");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("namespace EazyGF");
        mainStringBuilder.AppendLine("{");
        mainStringBuilder.AppendLine();

        mainStringBuilder.AppendLine($"\tpublic partial class {panelName} : UIBase");
        mainStringBuilder.AppendLine("\t{");
 
        mainStringBuilder.AppendLine("\t}");
        mainStringBuilder.AppendLine("}");

        File.WriteAllText(filePath, mainStringBuilder.ToString(), Encoding.UTF8);
    }

    //写属性文件
    private static void WriteDesignerFile(Transform selectTransform, string filePath)
    {
        string panelName = selectTransform.name;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        StringBuilder designStringBuilder = new StringBuilder(10);

        designStringBuilder.AppendLine("using UnityEngine.UI;");
        designStringBuilder.AppendLine("using UnityEngine;");
        designStringBuilder.AppendLine();
        designStringBuilder.AppendLine("namespace EazyGF");
        designStringBuilder.AppendLine("{");
        designStringBuilder.AppendLine($"\tpublic partial class {panelName}");
        designStringBuilder.AppendLine("\t{");
        designStringBuilder.AppendLine();
        //根据obj名字写入对应属性
        Transform[] AllChild = selectTransform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < AllChild.Length; i++)
        {
            string childName = AllChild[i].name;
            Transform secPanel = GenUIPanelCode.FindUpSecondPanelParent(AllChild[i]);
            //如果当前属于sec面板
            if (secPanel != null)
            {
                if (!secPanel.name.ToLower().Equals(selectTransform.name.ToLower()))
                {
                    continue;
                }
            }
            WriteChildProperty(designStringBuilder, childName);
        }
        designStringBuilder.AppendLine("\t}");
        designStringBuilder.AppendLine("}");
        File.WriteAllText(filePath, designStringBuilder.ToString(), Encoding.UTF8);
    }

    private static void WriteChildProperty(StringBuilder designStringBuilder, string childName)
    {
        string[] childNameArray = childName.Split('_');
        if (childNameArray.Length <= 1)
        {
            return;
        }
        //第一个是名字，后面的是属性名称
        string objName = childNameArray[0];
        for (int i = 1; i < childNameArray.Length; i++)
        {
            string fullTypeName = GenUIPanelCode.GetFullTypeName(childNameArray[i]);

            if (!string.IsNullOrEmpty(fullTypeName))
            {
                designStringBuilder.AppendLine($"\t\t[SerializeField] private {fullTypeName} {objName}_{childNameArray[i]};");
            }
        }
    }


}
