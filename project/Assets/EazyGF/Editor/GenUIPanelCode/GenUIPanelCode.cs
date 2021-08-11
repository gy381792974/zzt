using System;
using System.IO;
using System.Reflection;
using System.Text;
using EazyGF;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class GenUIPanelCode : Editor
{
    public static string uiPanelCodeFolder = EditorFilePath.Instance.GetUIScriptsFullPath();
    //其他自己生成面板类后缀
    public static string customClassPostfix = "_Cus";

    [MenuItem("GameTools/生成UI模板代码 (alt+1) &1",false,0)]
    public static void GenUICodeByGameTools()
    {
        GenUICode();
    }

    [MenuItem("Assets/生成UI模板代码", false, int.MaxValue)]
    public static void GenUICodeByAssets()
    {
        GenUICode();
    }

    private static void GenUICode()
    {
        uiPanelCodeFolder.CreateDirIfNotExists();
        AutoExportSetting.Instance.SelectGameObjects.Clear();
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GameObject selectGameObj = Selection.objects[i] as GameObject;
            if (selectGameObj == null)
            {
                continue;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(AB_ResFilePath.uiPanelPrefabsRootDir);
            bool isUIPanelPrefab = false;
            if (directoryInfo.Exists)
            {
                FileInfo[] fileInfos = directoryInfo.GetFiles("*.prefab");
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    if (selectGameObj.name.ToLower().Equals(Path.GetFileNameWithoutExtension(fileInfos[j].FullName).ToLower()))
                    {
                        isUIPanelPrefab = true;
                        break;
                    }
                }
            }

            if (!isUIPanelPrefab)
            {
                continue;
            }

            AutoExportSetting.Instance.SelectGameObjects.Add(selectGameObj);
        }

        for (int i = 0; i < AutoExportSetting.Instance.SelectGameObjects.Count; i++)
        {
            WriteFile(AutoExportSetting.Instance.SelectGameObjects[i].transform);
        }
    }

    public static Transform FindUpPrefabParent(Transform zi)
    {
        if (PrefabUtility.IsAnyPrefabInstanceRoot(zi.gameObject))
        {
            return zi;
        }

        Transform parent = zi.parent;
        if (parent != null)
        {
            return FindUpPrefabParent(parent);
        }

        return null;
    }

    public static Transform FindUpSecondPanelParent(Transform zi)
    {
        if (zi.name.Contains(customClassPostfix))
        {
            return zi;
        }

        Transform parent = zi.parent;
        if (parent != null)
        {
            return FindUpSecondPanelParent(parent);
        }

        return null;
    }


    public static void WriteFile(Transform selecTransform)
    {
        string folderPath = uiPanelCodeFolder + "/"+ selecTransform.name;
        
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
    private static void WriteMainFile(string panelName,string filePath)
    {
        StringBuilder mainStringBuilder=new StringBuilder(50);
        mainStringBuilder.AppendLine("using System.Collections;");
        mainStringBuilder.AppendLine("using System.Collections.Generic;");
        mainStringBuilder.AppendLine("using UnityEngine;");
        mainStringBuilder.AppendLine("using UnityEngine.UI;");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("namespace EazyGF");
        mainStringBuilder.AppendLine("{");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine($"\tpublic class {panelName}Data : UIDataBase");
        mainStringBuilder.AppendLine("\t{");
        mainStringBuilder.AppendLine($"\t\tpublic {panelName}Data()");
        mainStringBuilder.AppendLine("\t\t{");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("\t\t}");
        mainStringBuilder.AppendLine("\t}");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine($"\tpublic partial class {panelName} : UIBase");
        mainStringBuilder.AppendLine("\t{");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("\t\tprotected override void OnInit()");
        mainStringBuilder.AppendLine("\t\t{");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("\t\t}");
        mainStringBuilder.AppendLine();
        string dataName = $"{panelName.ToLower()}Data";
        mainStringBuilder.AppendLine($"        protected override void OnShow(UIDataBase {dataName} = null)");
        mainStringBuilder.AppendLine("\t\t{");
        mainStringBuilder.AppendLine($"\t\t\tif ({dataName} != null)");
        mainStringBuilder.AppendLine("\t\t\t{");
        mainStringBuilder.AppendLine($"                mPanelData = {dataName} as {panelName}Data;");
        mainStringBuilder.AppendLine("\t\t\t}");
        mainStringBuilder.AppendLine("\t\t}");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("\t\tprotected override void OnHide()");
        mainStringBuilder.AppendLine("\t\t{");
        mainStringBuilder.AppendLine();
        mainStringBuilder.AppendLine("\t\t}");
        mainStringBuilder.AppendLine("\t}");
        mainStringBuilder.AppendLine("}");

        File.WriteAllText(filePath, mainStringBuilder.ToString(),Encoding.UTF8); 
    }
    //写属性文件
    private static void WriteDesignerFile(Transform selectTransform,string filePath) 
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
        designStringBuilder.AppendLine($"\t\tprivate {panelName}Data mPanelData = null;");
        designStringBuilder.AppendLine();
        //根据obj名字写入对应属性
        Transform[] AllChild = selectTransform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < AllChild.Length; i++)
        {
            string childName = AllChild[i].name;
            Transform secPanel = FindUpSecondPanelParent(AllChild[i]);
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
            string fullTypeName = GetFullTypeName(childNameArray[i]);
            if (!string.IsNullOrEmpty(fullTypeName))
            {
                designStringBuilder.AppendLine($"\t\t[SerializeField] private {fullTypeName} {objName}_{childNameArray[i]};");
            }
        }
    }


    //每当编译完成反射属性给面板赋值
    [DidReloadScripts]
    private static void ReflectionProperty() 
    {
        try
        {
            for (int i = 0; i < AutoExportSetting.Instance.SelectGameObjects.Count; i++)
            {
                GameObject prefabGo = AutoExportSetting.Instance.SelectGameObjects[i];
                
                if (prefabGo == null)
                {
                    continue;
                }
                Type type = GetTypeByName(prefabGo.name);
                if (type != null)
                {
                    if (prefabGo.GetComponent(type) == null)
                    {
                        prefabGo.AddComponent(type);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    Transform[] AllChild = prefabGo.transform.GetComponentsInChildren<Transform>(true);
                    for (int j = 0; j < AllChild.Length; j++)
                    {
                        string childName = AllChild[j].name;
                        string[] childNameArray = childName.Split('_');
                        if (childNameArray.Length <= 1)
                        {
                            continue;
                        }
                        string objName = childNameArray[0];//第一个为名字
                        var component = prefabGo.GetComponent(type);
                        if (component == null)
                        {
                            continue;
                        }
                        MemberInfo[] memberInfos = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic);

                        for (int k = 0; k < memberInfos.Length; k++)
                        {
                            for (int x = 1; x < childNameArray.Length; x++)
                            {
                                string fullTypeName = GetFullTypeName(childNameArray[x]);
                                if (!string.IsNullOrEmpty(fullTypeName))
                                {
                                    string propertyName = $"{ objName}_{ childNameArray[x] }";
                                    if (memberInfos[k].Name.Equals(propertyName))
                                    {
                                        var fieldInfo = type.GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                                        if (fieldInfo != null)
                                        {
                                            if (fieldInfo.FieldType.ToString().Equals("UnityEngine.GameObject"))
                                            {
                                                fieldInfo.SetValue(component, AllChild[j].gameObject);
                                            }
                                            else if (fieldInfo.FieldType.ToString().Equals("UnityEngine.Transform"))
                                            {
                                                fieldInfo.SetValue(component, AllChild[j].transform);
                                            }
                                            else
                                            {
                                                var defaultType = AllChild[j].GetComponent(fieldInfo.FieldType);
                                                if (defaultType != null)
                                                {
                                                    fieldInfo.SetValue(component, defaultType);
                                                }
                                            }
                                            break;   
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!prefabGo.name.Contains(customClassPostfix))
                {
                    PrefabUtility.SavePrefabAsset(prefabGo);
                }
                else
                {
                    string assetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabGo);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                        if (prefab != null)
                        {
                            PrefabUtility.SavePrefabAsset(prefab);
                        }
                    }
                }
            }
            AutoExportSetting.Instance.SelectGameObjects.Clear();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        catch (Exception)
        {
            AutoExportSetting.Instance.SelectGameObjects.Clear();
        }
    }

    /// <summary>
    /// 从当前程序集中获取对应的类型
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    private static Type GetTypeByName(string typeName)
    {
        Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
        int assemblyArrayLength = assemblyArray.Length;
        for (int i = 0; i < assemblyArrayLength; ++i)
        {
            Type type = assemblyArray[i].GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }

        for (int i = 0; i < assemblyArrayLength; ++i)
        {
            Type[] typeArray = assemblyArray[i].GetTypes();
            int typeArrayLength = typeArray.Length;
            for (int j = 0; j < typeArrayLength; ++j)
            {
                if (typeArray[j].Name.Equals(typeName))
                {
                    return typeArray[j];
                }
            }
        }
        return null;
    }

    //检查是否是支持的类型并返回该类型的全称
    public static string GetFullTypeName(string objTypeName)
    {
        switch (objTypeName)
        {
            case "obj":
            case "Obj":
                return "GameObject";
            case "trans":
            case "Trans":
                return "Transform";
            case "btn":
            case "Btn":
                return "Button";
            case "text":
            case "Text":
                return "Text";
            case "img":
            case "Img":
            case "Image":
                return "Image";
            case "toggle":
            case "Toggle":
                return "Toggle";
            case "sRect":
            case "SRect":
                return "ScrollRect";
            case "slider":
            case "Slider":
                return "Slider";
            case "sbar":
            case "Sbar":
                return "Scrollbar";
            case "inputField":
            case "InputField":
                return "InputField";
            case "Dropdown":
                return "Dropdown";
        }

        return string.Empty;
    }
}
