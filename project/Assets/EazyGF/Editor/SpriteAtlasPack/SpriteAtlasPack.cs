using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class SpriteAtlasPack : Editor
{
    /// <summary>
    /// 创建一个新的图集并添加图片进去
    /// </summary>
    /// <param name="spriteObjects"></param>
    /// <param name="filePath"></param>
    public static SpriteAtlas CreateSpriteAtlas(List<Texture2D> spriteObjects,string filePath)
    {
        //创建一个新的图集
        SpriteAtlas spriteAtlas = CreateNewSpriteAtlas(filePath);
        spriteAtlas.Remove(spriteAtlas.GetPackables());
        List<Object> tempObjects=new List<Object>();
        tempObjects.AddRange(spriteObjects);
        spriteAtlas.Add(tempObjects.ToArray());
        return spriteAtlas;
    }

    /// <summary>
    /// 创建一个新的图集
    /// </summary>
    /// <param name="path"></param>
    /// <param name="AtlasName"></param>
    /// <returns></returns>
    private static SpriteAtlas CreateNewSpriteAtlas(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        SpriteAtlas spriteAtlas = new SpriteAtlas();

        SetSpriteAtlasProperty(spriteAtlas);

        AssetDatabase.CreateAsset(spriteAtlas, filePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return spriteAtlas;
    }

    //设置图集属性
    private static void SetSpriteAtlasProperty(SpriteAtlas spriteAtlas)
    {
        //设置打包属性
        SpriteAtlasPackingSettings spriteAtlasPackingSettings = new SpriteAtlasPackingSettings
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2
        };
        spriteAtlas.SetPackingSettings(spriteAtlasPackingSettings);
        //设置图片属性
        SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        spriteAtlas.SetTextureSettings(textureSetting);

        //// 设置Android平台属性
        //spriteAtlas.SetPlatformSettings(
        //    GetAndroidTexturePlatformSetting(AutoExportSetting.Instance.AndroidSpriteAtlasTextureFormat,AutoExportSetting.Instance.AndroidSpriteAtlasCompressionQuality, 2048)
        //    );

        //// 设置Ios平台属性
        //spriteAtlas.SetPlatformSettings(
        //    GetIOSTexturePlatformSetting(AutoExportSetting.Instance.IOSpriteAtlasTextureFormat, AutoExportSetting.Instance.IOSpriteAtlasCompressionQuality,2048)
        //    );
    }

    public static TextureImporterPlatformSettings GetAndroidTexturePlatformSetting(TextureImporterFormat textureImporterFormat, int compressionQuality,int maxSize)
    {
        TextureImporterPlatformSettings AndroidPlatformSetting = new TextureImporterPlatformSettings()
        {
            maxTextureSize = maxSize,
            format = textureImporterFormat,
            crunchedCompression = true,
            textureCompression = TextureImporterCompression.Compressed,
            compressionQuality = compressionQuality,
            overridden = true,
            androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality16Bit,
            
            name = "Android"
        };
        return AndroidPlatformSetting;
    }
    public static TextureImporterPlatformSettings GetIOSTexturePlatformSetting(TextureImporterFormat textureImporterFormat, int compressionQuality, int maxSize)
    {
        TextureImporterPlatformSettings IOSPlatformSetting = new TextureImporterPlatformSettings()
        {
            maxTextureSize = maxSize,
            format = textureImporterFormat,
            crunchedCompression = true,
            textureCompression = TextureImporterCompression.Compressed,
            compressionQuality = compressionQuality,
            overridden = true,

            name = "iPhone"
           // name = "iOS"
        }; 
        return IOSPlatformSetting;
    }

}
