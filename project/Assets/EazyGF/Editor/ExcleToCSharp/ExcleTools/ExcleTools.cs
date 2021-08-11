using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ExcleTools : Editor
{
    [MenuItem("ExcleTools/1.更新Excle", false,1)]
    public static void UpdateExcle()
    {
        ProcessCommand("TortoiseProc.exe", "/command:update /path:" + EditorFilePath.Instance.GetExcleRootFullPath());
    }

    [MenuItem("ExcleTools/2.提交Excle", false, 2)]
    public static void ComminExcle()
    {
        if (EditorUtility.DisplayDialog("提交Excle", "是否提交Excle文件", "确定", "取消"))
        {
            ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + EditorFilePath.Instance.GetExcleRootFullPath());
        }
    }

    [MenuItem("ExcleTools/3.更新项目", false, 3)]
    public static void UpdateProject()
    {
        UpdateOrCommit(true);
    }

    [MenuItem("ExcleTools/4.提交项目", false, 4)]
    public static void CommitProject()
    {
        if (EditorUtility.DisplayDialog("提交项目", "是否提交项目", "确定", "取消"))
        {
            UpdateOrCommit(false);
        }
    }

    [MenuItem("ExcleTools/5.清理", false, 5)]
    public static void ClearExcle()
    {
        System.IO.DirectoryInfo parent = Directory.GetParent(Application.dataPath);
        ProcessCommand("TortoiseProc.exe", "/command:cleanup /path:" + parent);
    }


    private static void UpdateOrCommit(bool update)
    {
        List<string> pathList = new List<string>();
        DirectoryInfo projectBasePath = Directory.GetParent(Application.dataPath);
        string assetsPath = projectBasePath + "/Assets";
        string projectSetingPath = projectBasePath + "/ProjectSettings";
        string otherPath = Path.Combine(EditorFilePath.Instance.GetExcleRootFullPath(), "");
        pathList.Add(assetsPath);
        pathList.Add(projectSetingPath);
        pathList.Add(otherPath);

        string commitPath = string.Join("*", pathList.ToArray());
        string updateOrComit = update ? "update" : "commit";
        ProcessCommand("TortoiseProc.exe", $"/command:{updateOrComit} /path:" + commitPath);
    }
    

    public static void ProcessCommand(string command, string argument)
    {
        ProcessStartInfo info = new ProcessStartInfo(command);
        info.Arguments = argument;
        info.CreateNoWindow = false;
        info.ErrorDialog = true;
        info.UseShellExecute = true;

        if (info.UseShellExecute)
        {
            info.RedirectStandardOutput = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
        }
        else
        {
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.StandardOutputEncoding = System.Text.Encoding.UTF8;
            info.StandardErrorEncoding = System.Text.Encoding.UTF8;
        }
        Process process = Process.Start(info);

        if (!info.UseShellExecute)
        {
            UnityEngine.Debug.Log(process.StandardOutput);
            UnityEngine.Debug.Log(process.StandardError);
        }
        process.WaitForExit();
        process.Close();

    }
}
