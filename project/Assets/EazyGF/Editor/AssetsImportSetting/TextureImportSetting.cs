using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 自动将图片处理为精灵图片
/// </summary>
public class TextureImportSetting : AssetPostprocessor
{
    //图片导入前调用，可以设置一些压缩参数
    void OnPreprocessTexture()
    {
        if (AutoExportSetting.Instance.IsAutoCompressionTexture)
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter == null)
            {
                return;
            }

            //textureImporter.textureType = TextureImporterType.Sprite; // 设置为Sprite类型

            textureImporter.mipmapEnabled = false; // 禁用mipmap
            textureImporter.npotScale = TextureImporterNPOTScale.None;//关闭2的幂次方-避免变形
            textureImporter.filterMode = FilterMode.Bilinear;//设置采样率

           // return;
            ////安卓图片设置
            //textureImporter.SetPlatformTextureSettings(SpriteAtlasPack.GetAndroidTexturePlatformSetting(
            //    AutoExportSetting.Instance.AndroidAutoImportTextureFormat, AutoExportSetting.Instance.AndroidAutoImportCompressionQuality, 2048));
            ////IOS图片设置
            //textureImporter.SetPlatformTextureSettings(SpriteAtlasPack.GetIOSTexturePlatformSetting(
            //    AutoExportSetting.Instance.IOSAutoImportTextureFormat, AutoExportSetting.Instance.IOSAutoImportCompressionQuality, 2048));

            //Debug.Log(textureImporter.GetDefaultPlatformTextureSettings().name);
        }
    }
    
    

    [MenuItem("Assets/设置图片格式为 ASTC", false, int.MaxValue - 4)]
    public static void SetTextureFormat_ETC2_ASTC_MidQuality()
    {
        SetTextureFormat(TextureImporterFormat.ASTC_RGBA_4x4, TextureImporterFormat.ASTC_RGBA_4x4, 100);
    }
    //[MenuItem("Assets/设置图片格式/ETC2 & ASTC 高压缩质量", false, int.MaxValue - 3)]
    //public static void SetTextureFormat_ETC2_ASTC_HigQuality()
    //{
    //    SetTextureFormat(TextureImporterFormat.ETC2_RGBA8Crunched, TextureImporterFormat.ASTC_RGBA_4x4, 100);
    //}
    //[MenuItem("Assets/设置图片格式/RGBA_32", false, int.MaxValue - 2)]
    //public static void SetTextureFormat_RGBA32()
    //{
    //    SetTextureFormat(TextureImporterFormat.RGBA32, TextureImporterFormat.RGBA32, 100);
    //}


    private static void SetTextureFormat(TextureImporterFormat androidFormat, TextureImporterFormat iosFormat, int quality)
    {
        if (AutoExportSetting.Instance.IsAutoCompressionTexture)
        {
            Debug.LogError("请将 设置里面的 IsAutoCompressionTexture 取消勾选");
        }

        if (Selection.objects.Length > 0)
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                DirectoryInfo directory = new DirectoryInfo(AssetDatabase.GetAssetPath(Selection.objects[i]));
                if (!directory.Exists)
                {
                    Debug.Log("请选中文件夹后再操作！");
                    return;
                }
                FileInfo[] fileInfos = directory.GetFiles("*", SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    string localPath = WriteFileHelp.ObsPathToRelativePath(fileInfos[j].FullName);
                    TextureImporter textureImporter = AssetImporter.GetAtPath(localPath) as TextureImporter;
                    if (textureImporter != null)
                    {
                        textureImporter.SetPlatformTextureSettings(SpriteAtlasPack.GetAndroidTexturePlatformSetting(androidFormat, quality, 2048));
                        textureImporter.SetPlatformTextureSettings(SpriteAtlasPack.GetIOSTexturePlatformSetting(iosFormat, quality, 2048));

                        textureImporter.SaveAndReimport();
                    }
                }
            }
        }
    }
}