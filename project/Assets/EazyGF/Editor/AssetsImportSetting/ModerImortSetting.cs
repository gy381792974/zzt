using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModerImortSetting : AssetPostprocessor
{
    //模型导入之前调用
    public void OnPreprocessModel()//
    {
        //ModelImporter modelImporter = assetImporter as ModelImporter;
        //if (modelImporter != null)
        //{
        //    modelImporter.animationType = ModelImporterAnimationType.Legacy;
        //    Debug.Log(111);
        //}
    }

    public void OnPostprocessModel(GameObject go)
    {
        //ModelImporter modelImporter = assetImporter as ModelImporter;
        //if (modelImporter != null)
        //{
        //    modelImporter.animationType = ModelImporterAnimationType.Legacy;
        //    Debug.Log(222);
        //}
    }
    //private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    //{
    //    //当移动资源的时候  也就是重新导入资源  
    //    for (int i = 0; i < importedAssets.Length; i++)
    //    {
    //        Debug.Log("importedAssets Asset: " + importedAssets[i]);
    //    }
    //}
}
