using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EazyGF;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class EditorRes_ObjectInfo
{
    public Object depObj;
    public int refCount;
}

public class ShareAssets : Editor
{
    public static void SetShareTextureABName()
    {
        //搜索所有依赖项,剔除出重复使用的图片
        SearchAllDepsAssets();

        //处理公用资源
        HandleShareAssets();

        AssetDatabase.Refresh();
    }
    
    //所有依赖字典
    private static List<EditorRes_ObjectInfo> allDepsList = new List<EditorRes_ObjectInfo>();
    //所有通用资源
    private static List<Object> allShareAssetsList = new List<Object>();
    //通用资源中的图片资源
    private static List<Object> allShareTextureList = new List<Object>();
    //搜索所有依赖资源
    private static void SearchAllDepsAssets()
    {
        //清空字典
        allDepsList.Clear();
        allShareAssetsList.Clear();
        allShareTextureList.Clear();
        //遍历指定文件夹下的所有Prefab的依赖文件
        SearchDirDeps(AB_ResFilePath.abPrefabPackRootDir.CreateDirIfNotExists());

        //将依赖数量超过1的添加进列表
        for (int i = 0; i < allDepsList.Count; i++)
        {
            if ((allDepsList[i].refCount > 1 || IsUnityDefuatAssets(allDepsList[i].depObj)) && !allShareAssetsList.Contains(allDepsList[i].depObj))
            {
                allShareAssetsList.Add(allDepsList[i].depObj);
            }
        }
    }
    //遍历文件
    private static void SearchDirDeps(string rootDirPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(rootDirPath);
        List<DirectoryInfo> allChildDirArray = WriteFileHelp.FindBelowDirectory(rootDirPath, true);
        for (int i = 0; i < allChildDirArray.Count; i++)
        {
            SearchFiles(allChildDirArray[i].GetFiles("*.prefab"));
        }
        SearchFiles(directoryInfo.GetFiles("*.prefab"));
    }
    /// <summary>
    /// 搜索每一个文件夹
    /// </summary>
    /// <param name="fileInfos"></param>
    private static void SearchFiles(FileInfo[] fileInfos)
    {
        for (int i = 0; i < fileInfos.Length; i++)
        {
            SearchSingleObjDeps(WriteFileHelp.ObsPathToRelativePath(fileInfos[i].FullName));
        }
    }

    private static void SearchSingleObjDeps(string searchObjPath)
    {
        Object abObject = AssetDatabase.LoadAssetAtPath(searchObjPath, typeof(Object));

        List<Object> depsList = GetPrefabDepe(abObject);
        for (int i = 0; i < depsList.Count; i++)
        {
            EditorRes_ObjectInfo editorResObjectInfo = allDepsList.Find(x => x.depObj.GetHashCode() == depsList[i].GetHashCode()
                                                                             && x.depObj.name == depsList[i].name);
            if (editorResObjectInfo == null)
            {
                editorResObjectInfo = new EditorRes_ObjectInfo();
                editorResObjectInfo.depObj = depsList[i];
                editorResObjectInfo.refCount = 1;
                allDepsList.Add(editorResObjectInfo);
            }
            else
            {
                editorResObjectInfo.refCount++;
            }
        }
    }
    
    //提取公用资源并打包成一个AB包
    private static void HandleShareAssets()
    {
        for (int i = 0; i < allShareAssetsList.Count; i++)
        {
            Type type = allShareAssetsList[i].GetType();

            //根据类型来处理
            if (type == typeof(Texture2D))//如果是图片
            {
                //为了避免动态加载的图片被打进公用图集，这里特殊处理一下
                //Unity不知名问题，会加上一些额外引用，特殊处理一下
                string[] nameArray = allShareAssetsList[i].name.Split('-');
                if (!nameArray[0].Equals("sactx") && !allShareAssetsList[i].name.Contains("Font Texture"))
                {
                    AssetImporter assetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(allShareAssetsList[i]));
                    if (assetImporter != null)
                    {
                        assetImporter.assetBundleName = "sharetextures";
                        allShareTextureList.Add(allShareAssetsList[i]);
                    }
                }
            }//如果是字体
            else if (type == typeof(Font))
            {
                if (!IsUnityDefuatAssets(allShareAssetsList[i]))
                {
                    if (MoveAssets(AB_ResFilePath.abAllShareFontsRootDir, allShareAssetsList[i]))
                    {
                        Debug.Log($"移动字体文件:{allShareAssetsList[i]} 到通用字体文件夹中");
                    }
                }
            }
            //如果是材质
            else if (type == typeof(Material))
            {
                if (!allShareAssetsList[i].name.Equals("Font Material"))//剔除字体材质
                {
                    //移动asset到share文件夹
                    if (MoveAssets(AB_ResFilePath.abAllShareMaterialsRootDir, allShareAssetsList[i]))
                    {
                        Debug.Log($"移动材质:{allShareAssetsList[i]} 到通用材质文件夹中");
                    }
                }
            }
            //如果是材质
            else if (type == typeof(Shader))
            {
                //移动asset到share文件夹
                if (MoveAssets(AB_ResFilePath.abAllShareShaderRootDir, allShareAssetsList[i]))
                {
                    Debug.Log($"移动Shader:{allShareAssetsList[i]} 到通用Shader文件夹中");
                }
            }
        }
        //移除未使用的公用图片ab包名
        ClearUnUsedShareTextureAB_Name();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        CheckShareAssetFolder();
        GenShareAssetClass();
    }

    //移除未使用的公用图片ab包名
    private static void ClearUnUsedShareTextureAB_Name()
    {
        //当前ab包里包含的
        string[] shareTextureNames = AssetDatabase.GetAssetPathsFromAssetBundle(AB_ResFilePath.abAllShareTextureABName);

        for (int i = 0; i < shareTextureNames.Length; i++)
        {
            string shareTextureName = Path.GetFileNameWithoutExtension(shareTextureNames[i]);
            bool isUse = false;//该资源是否真的是通用资源，如果不是，需要移除该资源的ab包名
            for (int j = 0; j < allShareTextureList.Count; j++)
            {
                if (allShareTextureList[j].name.Equals(shareTextureName))
                {
                    isUse = true;
                    break;
                }
            }

            if (!isUse)
            {
                AssetImporter assetImporter= AssetImporter.GetAtPath(shareTextureNames[i]);
                if (assetImporter != null)
                {
                    assetImporter.assetBundleName = null;
                    Debug.Log($"将 {shareTextureName} 移除出公用ab包，该资源并未公用");
                }
                else
                {
                    Debug.LogError($"将{shareTextureName} 移除出通用ab包是发生未知错误！！！请检查相关资源！！！");
                }
            }
        }
    }

    //检查通用文件夹是否存在资源
    private static void CheckShareAssetFolder()
    {
        writeAbNameList.Clear();

        //当前所有的ab包名
        string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        
        GenShareAssetsABName(IsShareFolderHaveAssets(AB_ResFilePath.abAllShareFontsRootDir), AB_ResFilePath.abAllShareFontsFolderName.ToLower());
        GenShareAssetsABName(IsShareFolderHaveAssets(AB_ResFilePath.abAllShareMaterialsRootDir), AB_ResFilePath.abAllShareMaterialsFolderName.ToLower());
        GenShareAssetsABName(IsShareFolderHaveAssets(AB_ResFilePath.abAllShareShaderRootDir), AB_ResFilePath.abAllShareShadersFolderName.ToLower());
        GenShareAssetsABName(allAssetBundleNames.Contains(AB_ResFilePath.abAllShareTextureABName), AB_ResFilePath.abAllShareTextureABName);
    }

    private static bool IsShareFolderHaveAssets(string dirPath)
    {
        dirPath.CreateDirIfNotExists();
        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
        return directoryInfo.GetFiles().Length > 0;
    }
     
    private static List<string> writeAbNameList=new List<string>(4);
    private static void GenShareAssetsABName(bool isGen,string abName)
    {
        
        StringBuilder stringBuilder=new StringBuilder(20);
       
        if (isGen)
        {
            stringBuilder.AppendLine($"      public static string {abName}=\"{abName}\";");
        }
        else
        {
            stringBuilder.AppendLine($"      public static string {abName}=\"{string.Empty}\";");
        }
        writeAbNameList.Add(stringBuilder.ToString());
        
    }

    private static void GenShareAssetClass()
    {
        string filePath = EditorFilePath.Instance.GetGeneratedFolderRootPath() + "/ShareAssetABName.cs";
        StringBuilder stringBuilder = new StringBuilder(120);

        stringBuilder.AppendLine("  public class ShareAssetABName");
        stringBuilder.AppendLine(" {");
        for (int i = 0; i < writeAbNameList.Count; i++)
        {
            stringBuilder.AppendLine(writeAbNameList[i]);
        }
        stringBuilder.AppendLine("  }");

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(stringBuilder);
            }
        }
    }


    //移动资源到指定文件夹
    private static bool MoveAssets(string newAssetPath, Object shareObj)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(newAssetPath);
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
            AssetDatabase.Refresh();
        }

        string oldFilePath = AssetDatabase.GetAssetPath(shareObj);

        string newFilePath = newAssetPath + "/" + Path.GetFileName(oldFilePath);

        //if (!File.Exists(oldFilePath) || File.Exists(newFilePath))
        //{
        //    return false;
        //}

        return string.IsNullOrEmpty(AssetDatabase.MoveAsset(oldFilePath, newFilePath));
    }


    //获取物体的所有依赖资源
    private static List<Object> GetPrefabDepe(Object go)
    {
        List<Object> results = new List<Object>();
        Object[] roots = { go };
        Object[] dependObjs = EditorUtility.CollectDependencies(roots);
        foreach (Object dependObj in dependObjs)
        {
            if (dependObj == null)
            {
                continue;
            }

            Type dependObjType = dependObj.GetType();
            //过滤
            if (!IsInFilterFolder(dependObj) && IShareType(dependObjType))
            {
                results.Add(dependObj);
            }
        }
        return results;
    }

    

    /// <summary>
    /// 是否是Unity的默认资源类型
    /// </summary>
    /// <returns></returns>
    public static bool IsUnityDefuatAssets(Object depenObj)
    {
        string assetPath = AssetDatabase.GetAssetPath(depenObj);
        if (!File.Exists(assetPath))
        {
            return true;
        }

        return false;
    }

    //需要从AB包中提取出来的类型，即需要单独打成一个共用AB包的类型
    private static Type[] ShareTypeArray = { typeof(Material), typeof(Texture2D), typeof(Font), typeof(Shader) };
    //是否是共用类型
    private static bool IShareType(Type type)
    {
        return ShareTypeArray.Contains(type);
    }

    //过滤的文件夹，不从以下文件夹中依赖资源
    private static readonly string[] FolderFilter ={"DesignAssets","Resources", "Editor", "ThirdParty","Gizmos", AB_ResFilePath.abAllShareAssetsFolderName,
                                                 AB_ResFilePath.abAllSpinesFolder,AB_ResFilePath.abTexturePackRootFolderName,
    };

    private static bool IsInFilterFolder(Object obj)
    {
        string assetPath = AssetDatabase.GetAssetPath(obj);
        // Debug.Log(assetPath);
        string[] assetPathArray = assetPath.Split('/');
        for (int i = 0; i < assetPathArray.Length; i++)
        {
            for (int j = 0; j < FolderFilter.Length; j++)
            {
                if (assetPathArray[i].Equals(FolderFilter[j]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
   
}
