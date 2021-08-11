using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using EazyGF;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public class AssetBundleHelp : Editor
{
    private static readonly List<string> AllAssetBundleNameList = new List<string>();
    
    [MenuItem("打包设置/设置通用图片的AB包名", false, int.MaxValue - 1)]
    public static void SetShareTextureAssetBunldeName()
    {
        //生成图集
        ShareAssets.SetShareTextureABName();
        Debug.Log("设置通用图片AB包名完毕!");
    }

    [MenuItem("打包设置/打AB包", false,int.MaxValue)]
    public static void BuildAssetBundle()
    {
        //ClearAllAb_Names();
        //生成通用图片ab包名
        ShareAssets.SetShareTextureABName();
        //删除所有ab包
        DeleteAllAssetBundle();

        //设置所有ab包名
        SetAllAssetBundleName();
        
        BuildPipeline.BuildAssetBundles(AB_ResFilePath.GetAssetBundleDirPath().CreateDirIfNotExists(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        //写入AB包对应的枚举类型
      //  WriteAB_NameEnumFile();

        AssetDatabase.Refresh();
        //加密
        EncryptAssetBundles();
        
        AssetDatabase.Refresh();

        EditorUtility.UnloadUnusedAssetsImmediate();

        Debug.Log("生成AB包完毕!");
    }

    //清除所有ab 包名
    private static void ClearAllAb_Names()
    {
        string[] allAssetsPath = AssetDatabase.GetAllAssetPaths();
        for (int i = 0; i < allAssetsPath.Length; i++)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(allAssetsPath[i]);
            if (assetImporter != null)
            {
                assetImporter.assetBundleName = null;
            }
        }
    }

    private static void EncryptAssetBundles()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(AB_ResFilePath.GetAssetBundleDirPath());
        FileInfo[] fileInfo = directoryInfo.GetFiles();
        for (int i = 0; i < fileInfo.Length; i++)
        {
            if (fileInfo[i].Name.EndsWith(".manifest") || fileInfo[i].Name.EndsWith(".meta"))
            {
                continue;
            }

            byte []bytes = File.ReadAllBytes(fileInfo[i].FullName);
            Random random=new Random();
            byte[] randomBytes=new byte[SerializHelp.GetKey()];
            random.NextBytes(randomBytes);
            byte[] finalBytes = randomBytes.Concat(bytes).ToArray();
            File.WriteAllBytes(fileInfo[i].FullName, finalBytes);
        }
    }

    /// <summary>
    /// 设置指定的文件夹ab包名
    /// </summary>
    private static void SetAllAssetBundleName()
    {
        AllAssetBundleNameList.Clear();

        //移除并未使用的 assetbundle 名字
        AssetDatabase.RemoveUnusedAssetBundleNames();

        //文件夹作为包名
        string[] folderBundleArray =
        {
            AB_ResFilePath.abAllShareAssetsRootDir,
            AB_ResFilePath.abTextureFolderPackRootDir,
            AB_ResFilePath.abAllSpinesRootDir,
            AB_ResFilePath.SoundsRootDir
        };

        SetFolderBundleName(folderBundleArray);
        
        //单个文件作为包名
        string[] sigleBundleArray =
        {
            AB_ResFilePath.abPrefabPackRootDir,
            AB_ResFilePath.abTextureSinglePackRootDir,
            AB_ResFilePath.jsonLanguageDatasRootDir,
            AB_ResFilePath.jsonGameDatasRootDir,
        };
        SetSingleBundleName(sigleBundleArray);
    }

    /// <summary>
    /// 设置文件夹的AB包名
    /// </summary>
    private static void SetFolderBundleName(string [] folderArray)
    {
        for (int i = 0; i < folderArray.Length; i++)
        {

            string rootDirPath = folderArray[i];
            if (!Directory.Exists(rootDirPath))
            {
                continue;
            }

            //最底层的文件夹
            List<DirectoryInfo> allChildDirArray = WriteFileHelp.FindBelowDirectory(rootDirPath, true);
            //将最底层的文件夹设置AB包名
            for (int j = 0; j < allChildDirArray.Count; j++)
            {
                DirectoryInfo childDirectoryInfo = allChildDirArray[j];
                AssetImporter assetImporter =
                    AssetImporter.GetAtPath(WriteFileHelp.ObsPathToRelativePath(childDirectoryInfo.FullName));
                //设置AB包名
                SetAssetBundName(assetImporter, childDirectoryInfo.Name);
            }

            if (allChildDirArray.Count <= 0)
            {
                if (rootDirPath[rootDirPath.Length - 1].Equals('/'))
                {
                    rootDirPath = rootDirPath.Substring(0, rootDirPath.Length - 1);
                }

                AssetImporter assetImporter = AssetImporter.GetAtPath(rootDirPath);
                SetAssetBundName(assetImporter, Path.GetFileNameWithoutExtension(rootDirPath));
            }
        }
    }
    //设置单个AB包
    private static void SetSingleBundleName(string []sigleArray)
    {
        for (int i = 0; i < sigleArray.Length; i++)
        {
            string rootDirPath = sigleArray[i];
            //最底层的文件夹
            DirectoryInfo directoryInfo = new DirectoryInfo(rootDirPath);
            if (!directoryInfo.Exists)
            {
                continue;
            }
            List<DirectoryInfo> allChildDirArray = WriteFileHelp.FindBelowDirectory(rootDirPath, true);
            for (int j = 0; j < allChildDirArray.Count; j++)
            {
                SetBundleNameByDir(allChildDirArray[j].GetFiles());
            }

            SetBundleNameByDir(directoryInfo.GetFiles());
        }
    }

    private static void SetBundleNameByDir(FileInfo[] fileInfos)
    {
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (!fileInfos[i].Name.EndsWith(".meta"))
            {
                AssetImporter assetImporter = AssetImporter.GetAtPath(WriteFileHelp.ObsPathToRelativePath(fileInfos[i].FullName));
                string fileName = fileInfos[i].Name;
                string[] fileNameArray = fileName.Split('.');
                string abName = fileNameArray.Length > 1 ? fileNameArray[0] : fileName;
                SetAssetBundName(assetImporter, abName);
            }
        }
    }
    //设置AB包名
    private static void SetAssetBundName(AssetImporter assetImporter,string abName)
    {
        if (assetImporter != null)
        {
            abName = abName.Replace(" ", "");
            assetImporter.assetBundleName = abName;
            AllAssetBundleNameList.Add(abName);
        }
    }
    
    
    //[MenuItem("打包设置/删除所有AB包", false)]
    public static void DeleteAllAssetBundle()
    {
        string path= AB_ResFilePath.GetStreamingAssetPath()+"/AllAssetBundles";
        DirectoryInfo directoryInfo=new DirectoryInfo(path);
        if (!directoryInfo.Exists)
        {
            return;
        }
        DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

        for (int i = 0; i < directoryInfos.Length; i++)
        {
            directoryInfos[i].Delete(true);
        }
    }

    [MenuItem("打包设置/复制需要下载的ab包到桌面", false)]
    public static void CopyDownLoadJson_ab_ToDeskTop()
    {

    }
    //生成ab包名对应的枚举类型
    [MenuItem("GameTools/生成ab包名枚举 (alt+2) &2", false, 1)]
    public static void WriteAB_NameEnumFile()
    {
        //SetAllAssetBundleName();

        //StringBuilder stringBuilder = new StringBuilder();
        //stringBuilder.AppendLine("//自动生成,不要在该文件里写任何代码");
        //stringBuilder.AppendLine("public enum ABName");
        //stringBuilder.AppendLine("{");
        //for (int i = 0; i < AllAssetBundleNameList.Count; i++)
        //{
        //    stringBuilder.AppendLine("\t" + AllAssetBundleNameList[i].ToLower() + ",");
        //}
        //stringBuilder.AppendLine("}");
        //string abNameFullPath = EditorFilePath.Instance.GetGeneratedFolderRootPath();
        //abNameFullPath.CreateDirIfNotExists();
        //string fileName = "/ABName.cs";
        //File.WriteAllText(abNameFullPath + fileName, stringBuilder.ToString());

        //AssetDatabase.Refresh();
    }
}
