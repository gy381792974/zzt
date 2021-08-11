using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class ExcleBackUp : Editor
{
    private static readonly string ExcleBackUpFoderPath = EditorFilePath.Instance.GetBackUpFullPath() + "/ExcleBackUp/";
    private static string excleManualBackFolderName = "ExcleManualBackUp/";
    private static string excleAutoBackFolderName = "ExcleAutoBackUp/";
    [MenuItem("ExcleTools/打开Excle文件夹 &e", false, 910)]
    public static void OpenExcleFoder()
    {
        Application.OpenURL(EditorFilePath.Instance.GetLocalExclePath());
    }

    [MenuItem("ExcleTools/备份Excle", false, 911)]
    public static void BackUpExcle()
    {
        BackUpExcle(false);
    }
    //是否是自动备份，自动备份不需要提示
    public static void BackUpExcle(bool IsAuto)
    {
        DateTime dateTime = DateTime.Now;
        string timer = $"{dateTime.Year}.{dateTime.Month}.{dateTime.Day}.{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}";
        string backUpFolderName = IsAuto ? excleAutoBackFolderName : excleManualBackFolderName;

        string targetFoderName = ExcleBackUpFoderPath + backUpFolderName + "/ExcleBackUp-V" + DiffGameVersion.GetVersionCode(Application.version) + "-" + timer;
        targetFoderName.CreateDirIfNotExists();
        
        try
        {
            CopyDirectory(EditorFilePath.Instance.GetExcleRootFullPath(), targetFoderName);
        }
        catch (Exception)
        {
            if (EditorUtility.DisplayDialog("错误", "备份Eccle失败，请手动备份！", "确定"))
            {
                return;
            }
        }

        if (!IsAuto)//不是自动备份，给出提示
        {
            EditorUtility.DisplayDialog("", "备份Excle完成！", "确定");
        }
        else//如果是自动备份，检查一下当前各个版本的个数，超过一定个数需要就删除掉
        {
            DeleteExcle(50,20);
        }
    }

    public static void CopyDirectory(string srcPath, string destPath)
    {
        try
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)     //判断是否文件夹
                {
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    CopyDirectory(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
                }
                else
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }
        catch (Exception)
        {
            Debug.LogError("复制文件夹失败："+ destPath);
        }
    }

    [MenuItem("ExcleTools/恢复到最近一次的Excle", false, 912)]
    public static void ResumToNearExcleVersion()
    {
        if (EditorUtility.DisplayDialog("", "是否恢复到最近一次的备份！", "确定"))
        {
            DirectoryInfo directory = new DirectoryInfo(ExcleBackUpFoderPath+excleAutoBackFolderName);
            if (!directory.Exists)
            {
                return;
            }
            try
            {
                string lastFileName = directory.GetDirectories()[directory.GetDirectories().Length - 1].FullName;
                CopyDirectory(lastFileName, EditorFilePath.Instance.GetExcleRootFullPath());

                ExcleGeneratorBase.StartGeneratorData();
                Debug.Log("恢复到最后一次Excle数据！");
            }
            catch (Exception)
            {
                EditorUtility.DisplayDialog("错误", "恢复失败，请关闭Excle文件再尝试！", "确定");
            }
        }
    }

    //[MenuItem("GameTools/删除无用的Excle", false, 912)]
    //public static void DeleteUnuseExcle()
    //{
    //    if (EditorUtility.DisplayDialog("", "是否删除无用的Excle！每个版本仅保留第一个、中间一个和最后三个！", "确定"))
    //    {
    //        DeleteExcle(4,5);
    //    }
    //}

    /// <summary>
    /// 删除Excle
    /// </summary>
    /// <param name="keepAmount">保留多少个文件</param>
    /// <param name="MaxCount">超过多少个文件才删除</param>
    private static void DeleteExcle(int MaxCount,int keepAmount)
    {
        if (keepAmount >= MaxCount)
        {
            Debug.LogError("传入的参数错误!");
            return;
        }
        List<string> versionCheckList = new List<string>();
        List<RecodDelteExcleParm> RecodDelteExcleParmList = new List<RecodDelteExcleParm>();
        DirectoryInfo directory = new DirectoryInfo(ExcleBackUpFoderPath+ excleAutoBackFolderName);
        if (!directory.Exists)
        {
            directory.Create();
            return;
        }

        for (int i = 0; i < directory.GetDirectories().Length; i++)
        {
            DirectoryInfo directoryInfo = directory.GetDirectories()[i];
            string[] directNameSpliteNmae = directoryInfo.Name.Split('-');
            string versionName = directNameSpliteNmae[1];
            if (!versionCheckList.Contains(versionName))
            {
                versionCheckList.Add(versionName);
            }

            RecodDelteExcleParm recodDelteExcleParm = new RecodDelteExcleParm();
            recodDelteExcleParm.versionName = versionName;
            recodDelteExcleParm._DirectoryInfo = directoryInfo;
            RecodDelteExcleParmList.Add(recodDelteExcleParm);
        }

        for (int i = 0; i < versionCheckList.Count; i++)
        {
            List<RecodDelteExcleParm> versionList =
                RecodDelteExcleParmList.FindAll(x => x.versionName.Equals(versionCheckList[i]));
            if (versionList.Count > MaxCount)
            {
                for (int j = 0; j < versionList.Count; j++)
                {
                    if (j != 0 &&j != versionList.Count / 2 && j < versionList.Count - keepAmount)
                    {
                        versionList[j]._DirectoryInfo.Delete(true);
                    }
                }
            }
        }
    }

    public class RecodDelteExcleParm
    {
        public string versionName;
        public DirectoryInfo _DirectoryInfo;
    }
}
