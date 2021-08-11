using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class EditorFilePath : ScriptableObject
{
    [Header("编辑器一级文件夹位置,和Asset文件夹平级")]
    private string otherAssetFolder = "OtherAssets";
    public string backUpFolder = "BackUp";
    public string exportFolder = "Export";

    [Header("Excle文件夹位置")]
    public string excleRootFolder = "Excle";
    public string excleDefaultFolder = "A_Default";
    public string excleAndriodFolder = "Android";
    public string excleIosFolder = "iOS";

    [Header("自动生成的文件夹位置")]
    public string scriptsRootFolder = "A_Scripts";
    public string uiScriptsFolder = "A_UI";

    public string genClassFolder = "GeneratedCode";
    public string jsonClassesFolder = "JsonClassCode";
    
    
    private static EditorFilePath instance;
    public static EditorFilePath Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetInstance();
            }
            return instance;
        }
        set => instance = value;
    }

    private static EditorFilePath GetInstance()
    {
        if (instance == null)
        {
            string editorFilePath = "Assets/EazyGF/Editor/EditorFilePath/EditorFilePathData.asset";
            EditorFilePath loadInstance = AssetDatabase.LoadAssetAtPath<EditorFilePath>(editorFilePath);

            if (loadInstance == null)
            {
#if UNITY_EDITOR
                loadInstance = CreateInstance<EditorFilePath>();

                editorFilePath.GetPathParentFolder().CreateDirIfNotExists();

                AssetDatabase.CreateAsset(loadInstance, editorFilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }

            return loadInstance;
        }

        return instance;
    }

    public void InitFolder()
    {
        Instance.GetExcleDefaultFullPath().CreateDirIfNotExists();
        Instance.GetExcleAndroidFullPath().CreateDirIfNotExists();
        Instance.GetExcleIosFullPath().CreateDirIfNotExists();
        Instance.GetPlayerDataBackUpRootDirPath().CreateDirIfNotExists();
        Instance.GetJsonClassesFullPath().CreateDirIfNotExists();
        AssetDatabase.Refresh();
    }

    public string GetOtherAssetsFullPath()
    {
            return Path.Combine(Directory.GetCurrentDirectory(),Instance.otherAssetFolder);
    }
    public string GetBackUpFullPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), Instance.backUpFolder);
    }
    public string GetExportFullPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), Instance.exportFolder);
    }

    public string GetExcleRootFullPath()
    {
        return Path.Combine(Instance.GetOtherAssetsFullPath(), Instance.excleRootFolder);
    }

    public string GetExcleDefaultFullPath()
    {
        return Path.Combine(GetExcleRootFullPath(),Instance.excleDefaultFolder);
    }
    public string GetExcleAndroidFullPath()
    {
        return Path.Combine(GetExcleRootFullPath(), Instance.excleAndriodFolder);
    }
    public string GetExcleIosFullPath()
    {
        return Path.Combine(GetExcleRootFullPath(), Instance.excleIosFolder);
    }

    public string GetUIScriptsFullPath()
    {
        return Path.Combine("Assets", Instance.scriptsRootFolder, Instance.uiScriptsFolder);
    }

    public string GetGeneratedFolderRootPath()
    {
        return Path.Combine("Assets", Instance.scriptsRootFolder, Instance.genClassFolder);
    }

    public string GetJsonClassesFullPath()
    {
        return Path.Combine(Instance.GetGeneratedFolderRootPath(), Instance.jsonClassesFolder);
    }


    public string GetPlayerDataBackUpRootDirPath()
    {
        return Path.Combine(Instance.GetOtherAssetsFullPath(), "PlayerDataBackUp").CreateDirIfNotExists();
    }

    public string GetTempFileRootDirPath()
    {
        return Path.Combine(Instance.GetOtherAssetsFullPath(), "TempFiles").CreateDirIfNotExists();
    }
    
    
    public string GetLocalExclePath()
    {
        if (AutoExportSetting.Instance.Android_IOS_Spilt)//如果安卓和IOS数据是分开的
        {
            string localPath =
#if UNITY_ANDROID
            GetExcleAndroidFullPath();
#elif UNITY_IOS
            GetExcleIosFullPath();
#else
            GetExcleDefaultFullPath();
#endif

            DirectoryInfo directoryInfo = new DirectoryInfo(localPath);
            if (directoryInfo.GetFiles().Length > 0)
            {
                return localPath;
            }
        }
        else//如果不分开，直接读取默认的文件夹
        {
            string localPath = GetExcleDefaultFullPath();
            DirectoryInfo directoryInfo = new DirectoryInfo(localPath);
            if (directoryInfo.GetFiles().Length > 0)
            {
                return localPath;
            }
        }
       
        Debug.LogError("本地Excel文件夹出错！");
        return string.Empty;
    }
    
}
