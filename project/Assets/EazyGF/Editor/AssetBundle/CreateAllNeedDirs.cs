using System.Collections;
using System.Collections.Generic;
using EazyGF;
using UnityEditor;

public class CreateAllNeedDirs : Editor
{
    public static void GenDir()
    {
        //ShareAssets
        AB_ResFilePath.abAllShareFontsRootDir.CreateDirIfNotExists();
        AB_ResFilePath.abAllSpinesRootDir.CreateDirIfNotExists();
        AB_ResFilePath.abAllShareMaterialsRootDir.CreateDirIfNotExists();
        AB_ResFilePath.abAllShareShaderRootDir.CreateDirIfNotExists();
        
        //Prefabs
        AB_ResFilePath.uiPanelPrefabsRootDir.CreateDirIfNotExists();

        //Textures
        AB_ResFilePath.abTextureSinglePackRootDir.CreateDirIfNotExists();
        AB_ResFilePath.abTextureFolderPackRootDir.CreateDirIfNotExists();

        //多语言文件夹
        AB_ResFilePath.jsonGameDatasRootDir.CreateDirIfNotExists();
        AB_ResFilePath.jsonLanguageDatasRootDir.CreateDirIfNotExists();

        AB_ResFilePath.SoundsRootDir.CreateDirIfNotExists();

        AB_ResFilePath.PlayerMainDataSavePath.GetPathParentFolder().CreateDirIfNotExists();
        
        AssetDatabase.Refresh();
    }
}
