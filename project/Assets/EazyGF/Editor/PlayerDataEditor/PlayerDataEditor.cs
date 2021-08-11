using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor;
using UnityEngine;

namespace EazyGF
{
    public class PlayerDataEditor : Editor
    {
        [MenuItem("GameTools/删档 &3", false, 3)]
        public static void EditorPlayerData_DeleData()
        {
            string playerDateSavePath = AB_ResFilePath.PlayerMainDataSavePath;
            if (File.Exists(playerDateSavePath))
            {
                File.Delete(playerDateSavePath);
                Debug.Log("删档成功");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.Log("找不到存档！");
            }
        }
    }
}

