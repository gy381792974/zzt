using System;
using System.Collections.Generic;
using System.IO;
using EazyGF;
using ICSharpCode.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;

public class ProjectBackUp : Editor
{
    [MenuItem("GameTools/备份工程-普通", false, 888)]
    public static void ZipProjectNormal()
    {
        string outputPath= GetBackUpOutPutPath(false).CreateDirIfNotExists();
        Zip zip = new Zip();
      
        zip.ZipFile(GetPackFolders(), Path.Combine(outputPath, GetFileName(false)));
        Debug.Log("备份工程完毕！");
    }
    
    [MenuItem("GameTools/备份工程-封版", false, 889)]
    public static void ZipProjectRelease()
    {
        string outputPath= GetBackUpOutPutPath(true).CreateDirIfNotExists();
        Zip zip = new Zip();
        zip.ZipFile(GetPackFolders(), Path.Combine(outputPath, GetFileName(true)));
        Debug.Log("备份工程完毕！");
    }

    private static string[] GetPackFolders()
    {
        string projectFolder = Environment.CurrentDirectory;
        string assetPath = projectFolder + "/Assets";
        string otherAsset = EditorFilePath.Instance.GetOtherAssetsFullPath();

        string package = projectFolder + "/Packages";
        string projectSetting = projectFolder + "/ProjectSettings";

        string[] packFolderNameArray = new[] { assetPath, otherAsset, package, projectSetting };
        return packFolderNameArray;
    }

    [MenuItem("GameTools/删除普通备份", false, 889)]
    public static void DeleteNormalProject()
    {
        if (EditorUtility.DisplayDialog("", "是否删除无用的备份工程！每个版本仅保留第一个、中间一个、最后一个！", "确定"))
        {
            GetRootPath(out var rootPath, out var outputDir, false);
            DirectoryInfo directoryInfo=new DirectoryInfo(outputDir);
            int totalCount = directoryInfo.GetFiles().Length;
            int deleteCount = 0;
            List<FileInfo> deleteList=new List<FileInfo>();
            if (totalCount > 3)
            {
                for (int i = 0; i < totalCount; i++)
                {
                    if (i != 0 && i != totalCount / 2 && i != totalCount - 1)
                    {
                        deleteList.Add(directoryInfo.GetFiles()[i]);
                    }
                }
                EditorUtility.DisplayProgressBar("删除无用的工程中!", $"完成度：{deleteCount}/{deleteList.Count}", (float)deleteCount / deleteList.Count);
                for (int i = 0; i < deleteList.Count; i++)
                {
                    deleteList[i].Delete();
                    deleteCount++;
                }
                Debug.Log("删除工程完毕");
                EditorUtility.ClearProgressBar();
            }
        }
    }

    private static void GetRootPath(out string rootPath, out string outputDir, bool isRelease)
    {
        rootPath = Directory.GetCurrentDirectory();
        outputDir = GetBackUpOutPutPath(isRelease);
    }

    private static string GetBackUpOutPutPath(bool isRelease)
    {
        if (isRelease)
        {
            return Path.Combine(EditorFilePath.Instance.GetBackUpFullPath(), "ProjectBackUp/封包").CreateDirIfNotExists();
        }
       return Path.Combine(EditorFilePath.Instance.GetBackUpFullPath(), "ProjectBackUp/普通").CreateDirIfNotExists();
    }

    private static string GetFileName(bool IsRelease)
    {
        DateTime dateTime = DateTime.Now;
        string timer = $"{dateTime.Year}.{dateTime.Month}.{dateTime.Day}-{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}";
        string release = IsRelease ? "封包-" : string.Empty;
        string outputFileName = release+ Application.productName + "-V" + DiffGameVersion.GetVersionCode(Application.version) + "-" + timer + ".zip";
        return outputFileName;
    }

}

