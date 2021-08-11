using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EazyGF
{
    public class DiffGameVersion
    {
        /// <summary>
        /// 处理玩家版本不一致
        /// </summary>
        /// <param name="playerData">玩家数据</param>
        /// <param name="gameVersion">玩家当前的版本号</param>
        public static void HandleDiffGameVersion(PlayerData playerData)
        {
            int versionCode = GetVersionCode(playerData.gameVersion);
            if (versionCode < 311)//如果版本号小于某个版本时--
            {
                //todo
                
            }
        }

        public static int GetVersionCode(string gameVersion)
        {
            StringBuilder versionStr=new StringBuilder(3);
            for (int i = 0; i < gameVersion.Length; i++)
            {
                if (!gameVersion[i].Equals('.'))
                {
                    versionStr.Append(gameVersion[i]) ;
                }
            }

            if (int.TryParse(versionStr.ToString(), out int version))
            {
                return version;
            }
            Debug.LogError("错误的版本号！请检查！！！");
            return 100;
        }
    }
}

