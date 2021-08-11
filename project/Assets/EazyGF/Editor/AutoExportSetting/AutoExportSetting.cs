using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EazyGF;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExportType
{
    APK,
    Gradle,
    AAB
}


public class AutoExportSetting : ScriptableObject
{

    [Header("导出类型：APK & Andriod Studio工程 & Google aab")]
    public ExportType _ExportType = ExportType.APK;

    [Header("安卓和IOS的Excle数据是否分离，既有2套数据")]
    public bool Android_IOS_Spilt = false;

    [Header("打包时是否重新打AB包")]
    public bool ReBuild_AB_When_Export = true;
    
    [Header("代码裁剪级别,打包失败或者异常时请Disable")]
    public ManagedStrippingLevel _ManagedStrippingLevel= ManagedStrippingLevel.Low;

    [Header("当图片导入时是否自动压缩图片格式")]
    public bool IsAutoCompressionTexture = true;

    
    [Header("Android平台的图片导入时自动压缩的格式")]
    [Header("Android平台")]
    public TextureImporterFormat AndroidAutoImportTextureFormat= TextureImporterFormat.ETC2_RGBA8Crunched;
    [Header("Android平台的图片导入时自动压缩的质量")]
    public int AndroidAutoImportCompressionQuality = 50;

    
    public string KeyStoreFoldeName="KeyStore";
    public string KeyStorePassWord;
    public string AliasName;
    public string AliasPassWord;
 
    [Header("IOS平台的图片压缩格式")]
    [Header("IOS平台")]
    public TextureImporterFormat IOSAutoImportTextureFormat= TextureImporterFormat.ASTC_RGBA_4x4;
    [Header("IOS平台的图片导入时自动压缩的质量")]
    public int IOSAutoImportCompressionQuality = 50;

    [Header("APP名字本地化")]
    public bool Localization=false;

    [Header("需要从网络下载的数据文件名称")]
    public List<string> DownLoadJsonData;

    //用于记录生成UI代码时选择的物体
    [HideInInspector]
    public List<GameObject> SelectGameObjects=new List<GameObject>();

    private static AutoExportSetting instance;
    public static AutoExportSetting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GetAutoExportSettingInstance();
            }

            return instance;
        }
        set => instance = value;
    }

    [MenuItem("打包设置/设置", false, 0)]
    public static void ExportSetting()
    {
        Selection.activeObject = Instance;
        //创建必要的文件
        CreateAllNeedDirs.GenDir();
        EditorFilePath.Instance.InitFolder();
    }

    [MenuItem("打包设置/设置为Debug模式", false, 20)]
    public static void SetDebug()
    {
        Debug_Release_Setting.DebugSetting();
    }

    [MenuItem("打包设置/设置为Release模式", false, 21)]
    public static void SetRelease()
    {
        Debug_Release_Setting.ReleaseSetting();
    }
    

    [MenuItem("打包设置/测试包", false,32)]
    public static void ExportDebugSetting()
    {
        SetDebug();
        ExportPrj(true);
    }
    [MenuItem("打包设置/正式包", false, 33)]
    public static void ExportReleaseSetting()
    {
        SetRelease();
        ExportPrj(false);
    }
    
    private static AutoExportSetting GetAutoExportSettingInstance()
    {
        string autoExportSettingDataPath = "Assets/EazyGF/Editor/AutoExportSetting/AutoExportSettingData.asset";

        AutoExportSetting autoExportSetting = AssetDatabase.LoadAssetAtPath<AutoExportSetting>(autoExportSettingDataPath);
        if (autoExportSetting == null)
        {
            autoExportSettingDataPath.GetPathParentFolder().CreateDirIfNotExists();
            Path.Combine(EditorFilePath.Instance.GetOtherAssetsFullPath(), Instance.KeyStoreFoldeName).CreateDirIfNotExists();
            autoExportSetting = CreateInstance<AutoExportSetting>();
            AssetDatabase.CreateAsset(autoExportSetting, autoExportSettingDataPath);
            AssetDatabase.Refresh();
        }
        return autoExportSetting;
    }

    
    /// <summary>
    /// 打包
    /// </summary>
    /// <param name="IsDebug"></param>
    private static void ExportPrj(bool IsDebug)
    {
        
        //设置各个平台的参数
        string ExportPath =
#if UNITY_ANDROID
           SetAndriondParm(IsDebug);
#elif UNITY_IOS
           SetIOS_Parm(IsDebug);
#else
           SetPC_Parm(IsDebug);
#endif
        ExportPath.CreateDirIfNotExists();
        //添加需要打包的Scene
        List<string> levels = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled)
            {
                continue;
            }
            levels.Add(scene.path);
        }
        
        if (levels.Count <= 0)
        {
            Debug.LogError("请至少添加一个场景到Build Scene");
            return;
        }
       

        //打包时是否需要重新生成图集
        if (Instance.ReBuild_AB_When_Export)
        {
           // ExcleGeneratorBase.StartGeneratorData();
            AssetBundleHelp.BuildAssetBundle();
        }

        Time.timeScale = 1;
        //设置代码裁剪级别--过高的裁剪级别会导致打包失败
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, Instance._ManagedStrippingLevel);
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, Instance._ManagedStrippingLevel);
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Standalone, Instance._ManagedStrippingLevel);
        
        //打包
        try
        {
            var report = BuildPipeline.BuildPlayer(levels.ToArray(), ExportPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("打包成功!");
                string folderPath = ExportPath.Substring(0, ExportPath.LastIndexOf('/'));
                Application.OpenURL(folderPath);

            }
            else if (report.summary.result == BuildResult.Cancelled)
            {
                Debug.LogWarning("取消打包！");
            }
            else
            {
                Debug.LogError("打包失败!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }

        }
        catch (Exception e)
        {
            Debug.LogError("打包失败：" + e);
        }
    }


    private static string SetAndriondParm(bool IsDebug)
    {
        bool IsKeyReadly = ChecKeyReadly();
        if (!IsKeyReadly)
        {
            Debug.LogError("请正确设置KeyStore");
            return string.Empty;
        }

        string extion = string.Empty;
        switch (Instance._ExportType)
        {
            case ExportType.APK:
                extion = ".apk";
                break;
            case ExportType.AAB:
                extion = ".aab";
                break;
        }
       

        //构建方式
        EditorUserBuildSettings.exportAsGoogleAndroidProject = Instance._ExportType==ExportType.Gradle;
        //输出方式-直接生成APK或者导出成AS工程
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        //是否为development模式
        //EditorUserBuildSettings.development = IsDebug;
        if (EditorUserBuildSettings.development)
        {
            EditorUserBuildSettings.allowDebugging = true;
        }

        //是否生成符号表文件
        EditorUserBuildSettings.androidCreateSymbolsZip = !IsDebug;

        //设置安卓打包的一些参数
        EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality16Bit;
        
        EditorUserBuildSettings.buildAppBundle = Instance._ExportType == ExportType.AAB;

        //如果Debug包，使用Mono打包，节约时间
        if (IsDebug)
        {
            //PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            //PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            //设置编译平台
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            //设置目标库
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        }
        else
        {
            //设置编译平台
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            //设置目标库
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        }

        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android,ApiCompatibilityLevel.NET_4_6);
        

        int curVersion = DiffGameVersion.GetVersionCode(Application.version);
        PlayerSettings.Android.bundleVersionCode = curVersion;

        //设置输出路径
        string folderName =Path.Combine(EditorFilePath.Instance.GetExportFullPath(), $"Android/Version_{curVersion}/{GetDebugOrReleasName(IsDebug)}/").CreateDirIfNotExists();
      
        int MaxVersion = GetMaxVersion(folderName);
        MaxVersion += 1;
        string APK_FullName = GetCommonFileNameTitle(IsDebug) + "-" + MaxVersion + extion;
        return folderName + APK_FullName;
    }

    private static int GetMaxVersion(string folderName)
    {
        int MaxVersion = 0;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        //找到文件最大的版本号
        for (int i = 0; i < fileInfos.Length; i++)
        {
            if (Path.GetFileName(fileInfos[i].Name).EndsWith("zip"))
            {
                continue;
            }

            string fileName = Path.GetFileNameWithoutExtension(fileInfos[i].Name);
            string[] fileNameArray = fileName.Split('-');
            int version = int.Parse(fileNameArray[fileNameArray.Length - 1]);
            if (version > MaxVersion)
            {
                MaxVersion = version;
            }
        }
        //找到文件夹最大的版本号
        DirectoryInfo[] directorys = directoryInfo.GetDirectories();
        for (int i = 0; i < directorys.Length; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(directorys[i].Name);
            string[] fileNameArray = fileName.Split('-');
            int version = int.Parse(fileNameArray[fileNameArray.Length - 1]);
            if (version > MaxVersion)
            {
                MaxVersion = version;
            }
        }

        return MaxVersion;
    }

    private static string GetDebugOrReleasName(bool IsDebug)
    {
       return IsDebug ? "Debug" : "Release";
    }

    private static bool ChecKeyReadly()
    {
        string keyStorePath = Path.Combine(EditorFilePath.Instance.GetOtherAssetsFullPath(), Instance.KeyStoreFoldeName);
        DirectoryInfo keyStoreInfo=new DirectoryInfo(keyStorePath);
        FileInfo[] fileInfos = keyStoreInfo.GetFiles();
        string keyStoreName = string.Empty;
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string fileName = fileInfos[i].Name;
            if (fileName.EndsWith("keystore")|| fileName.EndsWith("jks"))
            {
                keyStoreName = fileName;
                break;
            }
        }

        if (string.IsNullOrEmpty(keyStoreName))
        {
            return false;
        }
        
        PlayerSettings.Android.keystoreName = Path.Combine(keyStorePath, keyStoreName);

        PlayerSettings.Android.keyaliasName = Instance.AliasName;
        PlayerSettings.Android.keystorePass = Instance.KeyStorePassWord;
        if (string.IsNullOrEmpty(Instance.AliasPassWord))
        {
            Instance.AliasPassWord = Instance.KeyStorePassWord;
        }
        PlayerSettings.Android.keyaliasPass = Instance.AliasPassWord;

        return File.Exists(PlayerSettings.Android.keystoreName) &&
               !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasName) &&
               !string.IsNullOrEmpty(PlayerSettings.Android.keystorePass) &&
               !string.IsNullOrEmpty(PlayerSettings.Android.keyaliasPass);
    }

    private static string SetIOS_Parm(bool IsDebug)
    {
        string FileName = $"{Application.productName}_XcodeProject" ;
        DirectoryInfo directoryInfo=new DirectoryInfo(FileName);
        if (directoryInfo.Exists)
        {
            directoryInfo.Delete(true);
        }
        PlayerSettings.iOS.buildNumber = DiffGameVersion.GetVersionCode(Application.version).ToString();
        return Path.Combine(EditorFilePath.Instance.GetExportFullPath(), "IOS" , FileName);
    }

    private static string SetPC_Parm(bool IsDebug)
    {
        string folderName = GetCommonFileNameTitle(IsDebug) +"-"+ GetNowTimeToWishFormat();
        DirectoryInfo directoryInfo = new DirectoryInfo(folderName);
        if (directoryInfo.Exists)
        {
            directoryInfo.Delete(true);
        }
        return Path.Combine(EditorFilePath.Instance.GetExportFullPath() + "/PC/" + folderName + "/" + Application.productName + "-"
                            + DiffGameVersion.GetVersionCode(Application.version) + ".EXE");
    }
    /// <summary>
    /// 通用前缀
    /// </summary>
    /// <param name="IsDebug"></param>
    /// <returns></returns>
    private static string GetCommonFileNameTitle(bool IsDebug)
    {
        return Application.productName + "-V" + DiffGameVersion.GetVersionCode(Application.version) + "-" +
               GetDebugOrReleasName(IsDebug);
    }

    private static string GetNowTimeToWishFormat()
    {
        DateTime dateTime = DateTime.Now;
        string timer = $"{dateTime.Year}.{dateTime.Month}.{dateTime.Day}-{dateTime.Hour}.{dateTime.Minute}.{dateTime.Second}";
        return timer;
    }
    
}
