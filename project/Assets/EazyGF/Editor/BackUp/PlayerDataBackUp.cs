using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class PlayerDataBackUp : Editor
{

    [MenuItem("GameTools/版本数据/备份当前版本玩家数据")]
    public static void BackUpPlayerData()
    {
        string playerDataSavePath = AB_ResFilePath.PlayerMainDataSavePath;
        string playerMd5SavePath = AB_ResFilePath.MD5DataSavePath;
        if (!File.Exists(playerDataSavePath))
        {
            Debug.LogError("没有存档，无法备份！");
            return;
        }

        string playerDataName = Path.GetFileName(playerDataSavePath);
        string playerMd5Name= Path.GetFileName(playerMd5SavePath);

        int version = DiffGameVersion.GetVersionCode(Application.version);
        string foldeName =$"{EditorFilePath.Instance.GetPlayerDataBackUpRootDirPath()}/Version_{version}";
        foldeName.CreateDirIfNotExists();

        string playerDataBackupPath = $"{foldeName}/{playerDataName}";
        string playerMD5DataBackupPath = $"{foldeName}/{playerMd5Name}";
        bool backUpSuccess = true;
        try
        {
            File.Copy(playerDataSavePath, playerDataBackupPath, true);
            File.Copy(playerMd5SavePath, playerMD5DataBackupPath, true);
        }
        catch (Exception)
        {
            backUpSuccess = false;
        }

        if (backUpSuccess)
        {
            Debug.Log("备份玩家数据成功！");
        }
        else
        {
            Debug.LogError("备份玩家数据失败！");
        }
        
    }

    [MenuItem("GameTools/版本数据/将存档替换为当前版本")]
    public static void ReplacePlayerDataToCurVersion()
    {
        ReplacePlayerData(false);
    }

    [MenuItem("GameTools/版本数据/将存档替换为上个版本")]
    public static void ReplacePlayerDataToLastVersion()
    {
        ReplacePlayerData(true);
    }

    private static void ReplacePlayerData(bool useLastVersion)
    {
        DirectoryInfo rootDir = new DirectoryInfo(EditorFilePath.Instance.GetPlayerDataBackUpRootDirPath());
        if (rootDir.Exists)
        {
            DirectoryInfo[] allVersionDir = rootDir.GetDirectories("*", SearchOption.AllDirectories);
            List<int> allVersionCodeList = new List<int>();
            for (int i = 0; i < allVersionDir.Length; i++)
            {
                string[] versionNameArray = allVersionDir[i].Name.Split('_');
                if (int.TryParse(versionNameArray[versionNameArray.Length - 1], out var oldVersion))
                {
                    if (!allVersionCodeList.Contains(oldVersion))
                    {
                        allVersionCodeList.Add(oldVersion);
                    }
                }
            }

            if (allVersionCodeList.Count < 2)
            {
                Debug.LogError("备份存档数据过少，请保证当前备份数据里有当前版本和上一个版本！");
                return;
            }

            allVersionCodeList.Sort();
            int curVersion = DiffGameVersion.GetVersionCode(Application.version);
            int lastVersion = -1;
            for (int i = 0; i < allVersionCodeList.Count; i++)
            {
                if (useLastVersion)
                {
                    if (allVersionCodeList[i] == curVersion && i != 0)
                    {
                        lastVersion = allVersionCodeList[i - 1];
                        break;
                    }
                }
                else
                {
                    if (allVersionCodeList[i] == curVersion )
                    {
                        lastVersion = allVersionCodeList[i];
                        break;
                    }
                }
            }

            if (-1 == lastVersion)
            {
                Debug.LogError("找不到上一个版本号存档！");
                return;
            }

            DirectoryInfo lastVersionInfo = new DirectoryInfo(Path.Combine(EditorFilePath.Instance.GetPlayerDataBackUpRootDirPath(), $"Version_{lastVersion}"));
            if (lastVersionInfo.Exists)
            {
                FileInfo[] lastVersionFileInfo = lastVersionInfo.GetFiles();

                string playerDataSavePath = AB_ResFilePath.PlayerMainDataSavePath;

                for (int i = 0; i < lastVersionFileInfo.Length; i++)
                {
                    File.Copy(lastVersionFileInfo[i].FullName, Path.Combine(playerDataSavePath.GetPathParentFolder(), lastVersionFileInfo[i].Name), true);
                }
                Debug.Log($"当前玩家数据版本:{lastVersion}");
            }
        }
        else
        {
            Debug.Log("没有备份存档！");
        }
    }
  
}
