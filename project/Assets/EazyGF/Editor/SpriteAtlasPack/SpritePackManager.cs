//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using EazyGF;
//using Spine.Unity;
//using UnityEditor;
//using UnityEditor.U2D;
//using UnityEngine.U2D;
//using UnityEngine;
//using Object = UnityEngine.Object;

//public class EditorRes_ObjectInfo
//{
//    public Object depObj;
//    public int refCount;
//}

//public class SpritePackManager : Editor
//{

//    public static void BuildSpriteAtlas()
//    {
//        //删除所有图集，因为依赖会来自于图集，造成BUG
//        DeleteAllSpriteAtlas();
//        //设置图集为启用模式
//        EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
//        //搜索所有依赖项,剔除出重复使用的图片
//        SearchAllDepsAssets();
//        //创建公用图集
//        PackShareSpriteAtlas();
//        //创建面板的预制体图集
//        CreatePanelSpriteAtlas();
//        //创建动态加载的图集
//        CreateDynamicsSpriteAtlas();
//        //生成图集预览
//        SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);
//        //保存 刷新
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }
//    //删除所有的图集
//    private static void DeleteAllSpriteAtlas()
//    {
//        DirectoryInfo directoryInfo=new DirectoryInfo(AB_ResFilePath.allSpriteAtlasRootDir);
//        List< FileInfo > deleFileInfos=new List<FileInfo>();
//        if (directoryInfo.Exists)
//        {
//            DirectoryInfo[] chilDirectoryInfos = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
//            for (int i = 0; i < chilDirectoryInfos.Length; i++)
//            {
//                FileInfo[] fileInfos = chilDirectoryInfos[i].GetFiles();
//                for (int j = 0; j < fileInfos.Length; j++)
//                {
//                    deleFileInfos.Add(fileInfos[j]);
//                }
//            }
//        }

//        for (int i = 0; i < deleFileInfos.Count; i++)
//        {
//            deleFileInfos[i].Delete();
//        }
//    }


//    private static List<EditorRes_ObjectInfo> allDepsList = new List<EditorRes_ObjectInfo>();
//    private static List<Object> allShareList=new List<Object>();
//    //搜索所有依赖资源
//    private static void SearchAllDepsAssets()
//    {
//        //清空字典
//        allDepsList.Clear();
//        allShareList.Clear();
//        shareFilter.Clear();
//        //遍历指定文件夹下的所有Prefab的依赖文件
//        SearchDirDeps(AB_ResFilePath.abPrefabPackRootDir.CreateDirIfNotExists());
        
//        //将依赖数量超过1的添加进列表
//        for (int i = 0; i < allDepsList.Count; i++)
//        {
//            if ((allDepsList[i].refCount > 1 || IsUnityDefuatAssets(allDepsList[i].depObj)) && !allShareList.Contains(allDepsList[i].depObj))
//            {
//                allShareList.Add(allDepsList[i].depObj);
//            }
//        }
//    }
//    //遍历文件
//    private static void SearchDirDeps(string rootDirPath)
//    {
//        DirectoryInfo directoryInfo = new DirectoryInfo(rootDirPath);
//        List<DirectoryInfo> allChildDirArray = WriteFileHelp.FindBelowDirectory(rootDirPath, true);
//        for (int i = 0; i < allChildDirArray.Count; i++)
//        {
//            SearchFiles(allChildDirArray[i].GetFiles("*.prefab"));
//        }
//        SearchFiles(directoryInfo.GetFiles("*.prefab"));
//    }
//    /// <summary>
//    /// 搜索每一个文件夹
//    /// </summary>
//    /// <param name="fileInfos"></param>
//    private static void SearchFiles(FileInfo[] fileInfos)
//    {
//        for (int i = 0; i < fileInfos.Length; i++)
//        {
//            SearchSingleObjDeps(WriteFileHelp.ObsPathToRelativePath(fileInfos[i].FullName));
//        }
//    }
   
//    private static void SearchSingleObjDeps(string searchObjPath)
//    {
//        Object abObject = AssetDatabase.LoadAssetAtPath(searchObjPath, typeof(Object));

//        List<Object> depsList = GetPrefabDepe(abObject);
//        for (int i = 0; i < depsList.Count; i++)
//        {
//            EditorRes_ObjectInfo editorResObjectInfo = allDepsList.Find(x => x.depObj.GetHashCode() == depsList[i].GetHashCode()
//                                                                             && x.depObj.name==depsList[i].name);
//            if (editorResObjectInfo == null)
//            {
//                editorResObjectInfo=new EditorRes_ObjectInfo();
//                editorResObjectInfo.depObj = depsList[i];
//                editorResObjectInfo.refCount = 1;
//                allDepsList.Add(editorResObjectInfo);
//            }
//            else
//            {
//                editorResObjectInfo.refCount++;
//            }
//        }
//    }

//    //过滤掉公用图集里一些不需要的东西
//    private static List<Object> shareFilter = new List<Object>();
//    //提取公用资源并打包成一个AB包
//    private static void PackShareSpriteAtlas()
//    {
//        shareFilter.Clear();
//        List<Texture2D> texture2DList = new List<Texture2D>();
//        for (int i = 0; i < allShareList.Count; i++)
//        {
//            Type type = allShareList[i].GetType();

//            //根据类型来处理
//            if (type == typeof(Texture2D))//如果是图片
//            {
//                //为了避免动态加载的图片被打进公用图集，这里特殊处理一下
//                //Unity不知名问题，会加上一些额外引用，特殊处理一下
//                string[] nameArray = allShareList[i].name.Split('-');
//                if (!nameArray[0].Equals("sactx")  && !allShareList[i].name.Contains("Font Texture"))
//                {
//                    shareFilter.Add(allShareList[i]);
//                    texture2DList.Add(allShareList[i] as Texture2D);
//                }
//            }//如果是字体
//            else if (type == typeof(Font))
//            {
//                if (!IsUnityDefuatAssets(allShareList[i]))
//                {
//                    CopyAssets(AB_ResFilePath.abAllShareFontsRootDir, allShareList[i]);
//                }
//            }
//            //如果是材质
//            else if (type == typeof(Material))
//            {
//                if (!allShareList[i].name.Equals("Font Material"))//剔除字体材质
//                {
//                    //移动asset到share文件夹
//                     CopyAssets(AB_ResFilePath.abAllShareMaterialsRootDir, allShareList[i]);
//                }
//            }
//            //如果是材质
//            else if (type == typeof(Shader))
//            {
//                //移动asset到share文件夹
//                CopyAssets(AB_ResFilePath.abAllShareShaderRootDir, allShareList[i]);
//            }
//        }
//        //将共用图片打进图集
//        if (shareFilter.Count > 0)
//        {
//            string shareSpriteAtlasFullPath = AB_ResFilePath.abAllShareSpriteAtlasRootDir.CreateDirIfNotExists()+
//                                                           AB_ResFilePath.shareSpriteAtlasName + ".spriteatlas";
//            SpriteAtlas spriteAtlas = SpriteAtlasPack.CreateSpriteAtlas(texture2DList, shareSpriteAtlasFullPath);
//            spriteAtlas.SetIncludeInBuild(true);
//        }
//    }

    
//    //移动资源到指定文件夹
//    private static void CopyAssets(string newAssetPath, Object shareObj)
//    {
//        DirectoryInfo directoryInfo = new DirectoryInfo(newAssetPath);
//        if (!directoryInfo.Exists)
//        {
//            directoryInfo.Create();
//            AssetDatabase.Refresh();
//        }

//        string oldFilePath = AssetDatabase.GetAssetPath(shareObj);
        
//        string newFilePath = newAssetPath + "/" + Path.GetFileName(oldFilePath);

//        if (!File.Exists(oldFilePath) || File.Exists(newFilePath))
//        {
//            return;
//        }

//        AssetDatabase.CopyAsset(oldFilePath, newFilePath);
//    }

    
//    //获取物体的所有依赖资源
//    private static List<Object> GetPrefabDepe(Object go)
//    {
//        List<Object> results = new List<Object>();
//        Object[] roots = { go };
//        Object[] dependObjs = EditorUtility.CollectDependencies(roots);
//        foreach (Object dependObj in dependObjs)
//        {
//            if (dependObj == null)
//            {
//               continue;
//            }

//            Type dependObjType = dependObj.GetType();
//            //过滤
//            if (!IsInFilterFolder(dependObj) && IShareType(dependObjType))
//            {
//                results.Add(dependObj);
//            }
//        }
//        return results;
//    }

//    //获取指定路径下面的所有资源文件  
//    public static Object[] GetAllObjectFromFolder(string fullPath)
//    {
//        List<Object> objsList=new List<Object>();
       
//        if (Directory.Exists(fullPath))
//        {
//            DirectoryInfo direction = new DirectoryInfo(fullPath);
//            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

//            for (int i = 0; i < files.Length; i++)
//            {
//                if (files[i].Name.EndsWith(".meta"))
//                {
//                    continue;
//                }
//                Object tempObj = AssetDatabase.LoadAssetAtPath<Object>(WriteFileHelp.ObsPathToRelativePath(files[i].FullName));
//                objsList.Add(tempObj);
//            }
//        }
//        return objsList.ToArray();
//    }

    
//    /// <summary>
//    /// 是否是Unity的默认资源类型
//    /// </summary>
//    /// <returns></returns>
//    public static bool IsUnityDefuatAssets(Object depenObj)
//    {
//        string assetPath = AssetDatabase.GetAssetPath(depenObj);
//        if (!File.Exists(assetPath))
//        {
//            return true;
//        }

//        return false;
//    }

//    //需要从AB包中提取出来的类型，即需要单独打成一个共用AB包的类型
//    private static Type[] ShareTypeArray = { typeof(Material), typeof(Texture2D), typeof(Font), typeof(Shader), typeof(SpineAtlasAsset) };
//    //是否是共用类型
//    private static bool IShareType(Type type)
//    {
//        return ShareTypeArray.Contains(type);
//    }

//    //过滤的文件夹，不从以下文件夹中依赖资源
//    private static readonly string[] FolderFilter ={"DesignAssets","Resources", "Editor", "ThirdParty","Gizmos", AB_ResFilePath.abAllShareAssetsFolderName,
//                                                 AB_ResFilePath.abAllSpinesFolder,AB_ResFilePath.abTexturePackRootFolderName,AB_ResFilePath.allSpriteAtlasFolderName
//    };
//    private static bool IsInFilterFolder(Object obj)
//    {
//        string assetPath = AssetDatabase.GetAssetPath(obj);
//       // Debug.Log(assetPath);
//        string[] assetPathArray = assetPath.Split('/');
//        for (int i = 0; i < assetPathArray.Length; i++)
//        {
//            for (int j = 0; j < FolderFilter.Length; j++)
//            {
//               if (assetPathArray[i].Equals(FolderFilter[j]))
//                {
//                    return true;
//                }
//            }
//        }
//        return false;
//    }


//    //创建面板图集
//    public static void CreatePanelSpriteAtlas()
//    {
//        Object[] panelPrefabs = GetAllObjectFromFolder(AB_ResFilePath.uiPanelPrefabsRootDir);
//        for (int i = 0; i < panelPrefabs.Length; i++)
//        {
//            if (PrefabUtility.GetPrefabAssetType(panelPrefabs[i]) != PrefabAssetType.NotAPrefab) //如果是预制体
//            {
//                List<Texture2D> packList = GetPrefabDepe<Texture2D>(panelPrefabs[i]);
//                if (packList != null && packList.Count > 0)
//                {
//                    string panelAtlasPath = AB_ResFilePath.uiPanelsAtlasRootDir.CreateDirIfNotExists() + panelPrefabs[i].name+AB_ResFilePath.spriteAtlasPostfix+ ".spriteatlas";                    
//                    SpriteAtlasPack.CreateSpriteAtlas(packList, panelAtlasPath).SetIncludeInBuild(true); 
//                }
//            }
//        }
//    }

//    private static List<T> GetPrefabDepe<T>(Object go)
//    {
//        List<T> results = new List<T>();
//        Object[] roots = { go };
//        Object[] dependObjs = EditorUtility.CollectDependencies(roots);
//        foreach (Object dependObj in dependObjs)
//        {
//            if (dependObj == null)
//            {
//                continue;
//            }

//            string[] nameArray = dependObj.name.Split('-');
//            if (dependObj.name.Equals("Font Texture") )
//            {
//                continue;
//            }

//            if (IsInFilterFolder(dependObj))
//            {
//                continue;
//            }

//            if (dependObj.GetType() == typeof(T) && !nameArray[0].Equals("sactx")  && !IsTextureInShareList(dependObj) && !IsUnityDefuatAssets(dependObj))
//            {
//                results.Add((T)Convert.ChangeType(dependObj, typeof(T)));
//            }
//        }
//        AssetDatabase.SaveAssets();
//        return results;
//    }

//    //创建动态加载图集
//    public static void CreateDynamicsSpriteAtlas()
//    {
//        //最底层的文件夹
//        List<DirectoryInfo> belowDirectoryInfos = WriteFileHelp.FindBelowDirectory(AB_ResFilePath.abTextureFolderPack_atlas, true);
//        List<Texture2D> folderTextureList = new List<Texture2D>();
//        for (int i = 0; i < belowDirectoryInfos.Count; i++)
//        {
//            for (int j = 0; j < belowDirectoryInfos[i].GetFiles().Length; j++)
//            {
//                FileInfo fileInfo = belowDirectoryInfos[i].GetFiles()[j];
//                Texture2D texture2D =
//                    AssetDatabase.LoadAssetAtPath<Texture2D>(WriteFileHelp.ObsPathToRelativePath(fileInfo.FullName));
//                if (texture2D != null)
//                {
//                    folderTextureList.Add(texture2D);
//                }
//            }
//            string path = AB_ResFilePath.packedAtlasRootDir.CreateDirIfNotExists() + belowDirectoryInfos[i].Name+".pack"+ ".spriteatlas";
//            SpriteAtlasPack.CreateSpriteAtlas(folderTextureList, path).SetIncludeInBuild(false);
//        }
//    }

    
//    private static bool IsTextureInShareList(Object shareObj)
//    {
//        return shareFilter.Find(x => x.GetHashCode() == shareObj.GetHashCode()) != null;
//    }


//}
