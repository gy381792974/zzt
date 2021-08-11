using System;
using System.IO;
using EazyGF;
using UnityEngine;

public class FilePath_DownLoadJsonData
{
    public static readonly string downloadJsonAB_RootDirPath = AB_ResFilePath.abRootDir + "/DownLoadJsonData";
    public static readonly string downloadJsonAB_DirPath = downloadJsonAB_RootDirPath + "/JsonDatas";
    public static readonly string downloadJsonAB_ExportPath= Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+
                                                             "/DownLoadJsonData";
}


/// <summary>
/// AB包相关路径
/// </summary>
public class AB_ResFilePath
{
    //AB资源的根目录
    public const string abRootDir = "Assets/AssetBundleRes";
    //Spine资源目录
    public const string abAllSpinesRootDir = abRootDir + "/" + abAllSpinesFolder;
    public const string abAllSpinesFolder = "SpineAssets"; 

    //预制体根目录
    public const string abPrefabPackRootDir = abRootDir + "/A_Prefabs/";

    //图片根目录
    public const string abTexturePackRootDir = abRootDir + "/" + abTexturePackRootFolderName + "/";
    public const string abTexturePackRootFolderName = "A_Textures";
    //图片单个打包目录
    public const string abTextureSinglePackRootDir = abTexturePackRootDir + abTexturePackSingleFolderName + "/";
    public const string abTexturePackSingleFolderName = "SinglePack";
    //图片文件打包根目录
    public const string abTextureFolderPackRootDir = abTexturePackRootDir + abTexturePackFolderName + "/";
    //图片文件夹打包 ab 包目录
    public const string abTexturePackFolderName =  "FolderPack";
    

    //共用资源的根目录
    public const string abAllShareAssetsRootDir = abRootDir + "/" + abAllShareAssetsFolderName + "/";
    public const string abAllShareAssetsFolderName = "AllShareAssets";
    //共用资源的字体根目录
    public const string abAllShareFontsRootDir = abAllShareAssetsRootDir + abAllShareFontsFolderName;
    public const string abAllShareFontsFolderName = "Fonts";

    //共用资源的Material目录
    public const string abAllShareMaterialsRootDir = abAllShareAssetsRootDir + abAllShareMaterialsFolderName;
    public const string abAllShareMaterialsFolderName = "Materials";

    //共用资源的Shader目录
    public const string abAllShareShaderRootDir = abAllShareAssetsRootDir + abAllShareShadersFolderName;
    public const string abAllShareShadersFolderName = "Shaders";
    //通用的图片ab包目录
    public const string abAllShareTextureABName = "sharetextures";


    //UI面板预制体文件夹路径
    public const string uiPanelPrefabsRootDir = abPrefabPackRootDir + uiPanelFolderName;

    
    //UI面板预制体文件夹名称
    public const string uiPanelFolderName = "A_UIPanelPrefabs/";

    public const string jsonDatasRootDir = abRootDir + "/JsonDatas/";
    public const string jsonGameDatasRootDir = jsonDatasRootDir + "GameDatas";
    public const string jsonLanguageDatasRootDir = jsonDatasRootDir + "LanguageDatas/";
    public const string LanguageSuffix = ".txt";

    //音效
    public const string SoundsRootDir = abRootDir + "/" + SoundsFolderName;
    public const string SoundsFolderName = "Sounds";

    
    //不同平台下StreamingAssets的路径。
    private static readonly string streamingAssetsPath =
#if   UNITY_ANDROID || UNITY_EDITOR|| UNITY_STANDALONE_WIN
        Application.streamingAssetsPath;
#elif UNITY_IPHONE
        Application.dataPath + "/Raw";
#else
       string.Empty;
#endif

    public static string GetStreamingAssetPath()
    {
        return streamingAssetsPath;
    }

    public static string GetRunTimePlatformName()
    {
#if UNITY_ANDROID
        return "Andriod";
#elif UNITY_IOS
          return "iOS";
#else
        return "PC";
#endif
    }

    public static string GetAssetBundleDirPath()
    {
        string assetbundleRootDir =
            $"{GetStreamingAssetPath()}/AllAssetBundles/{GetRunTimePlatformName()}/";
        return assetbundleRootDir;
    }
    

    private static string playerMainDataSavePath;
    private static string md5SavePath;
    //玩家数据存档位置
    public static string PlayerMainDataSavePath
    {
        get
        {
            if (string.IsNullOrEmpty(playerMainDataSavePath))
            {
#if !UNITY_EDITOR
                playerMainDataSavePath = Path.Combine(Application.persistentDataPath, "PlayerData.data");
#else
                playerMainDataSavePath = Path.Combine(Directory.GetCurrentDirectory(), "OtherAssets/PlayerData/PlayerData.data");
#endif
            }
            return playerMainDataSavePath;
        }
    }

    //玩家数据MD5码存档位置
    public static string MD5DataSavePath
    {
        get
        {
            if (string.IsNullOrEmpty(md5SavePath))
            {
#if !UNITY_EDITOR
                md5SavePath = Path.Combine(Application.persistentDataPath, "MD5_File.data");
#else
                md5SavePath = Path.Combine(Directory.GetCurrentDirectory(), "OtherAssets/PlayerData/MD5_File.data");
#endif
            }
            return md5SavePath;
        }
    }
}
