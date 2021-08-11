using System;
using UnityEngine;
using System.IO;

namespace EazyGF
{
    /// <summary>
    /// 初始化和保存玩家数据
    /// </summary>
    public partial class PlayerDataMgr
    {
        //游戏数据
        public static PlayerData g_playerData;
        
        /// <summary>
        /// 初始化玩家
        /// </summary>
        public void InitPlayerData()
        {
            //如果有存档
            if (File.Exists(AB_ResFilePath.PlayerMainDataSavePath))
            {
               
                //读取玩家存档
                g_playerData = SerializHelp.DeserializeFileToObj<PlayerData>(AB_ResFilePath.PlayerMainDataSavePath, out bool loadSuccess);
                if (!loadSuccess)
                {
                    Debug.LogError("读取玩家数据失败！创建新的玩家数据！");
                    CreateNewData();
                    return;
                }
                
                Debug.Log($"当前版本号为：{g_playerData.gameVersion},最新版本号为:{Application.version}");
                //如果版本不一致，在数据类型不一致的情况下的特殊处理
                if (!string.Equals(g_playerData.gameVersion, Application.version))
                {
                    //处理版本不一致
                    Debug.Log("版本号不一致");
                    DiffGameVersion.HandleDiffGameVersion(g_playerData);
                    SaveData();
                }

                Debug.Log("读取玩家存档成功！");
            }
            else
            {
                //创建新的玩家数据
                Debug.Log("没有存档，创建新的玩家数据！");
                CreateNewData();
            }
        }

        /// <summary>
        /// 创建玩家数据并保存
        /// </summary>
        private void CreateNewData()
        {
            g_playerData = new PlayerData();
            SaveData();
        }

        /// <summary>
        /// 保存玩家的数据
        /// </summary>
        public static void SaveData(bool isUpdatePLFTime = false)
        {
            if (isUpdatePLFTime)
            {
                g_playerData.playerOfflineTime = DateTime.Now;
            }

            g_playerData.gameVersion = Application.version;
            SerializHelp.SerializeFile(AB_ResFilePath.PlayerMainDataSavePath, g_playerData);
        }

    }
}

