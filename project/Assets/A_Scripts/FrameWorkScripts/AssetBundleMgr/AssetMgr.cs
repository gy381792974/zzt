using System;
using System.Collections;
using System.Collections.Generic;
using EazyGF;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

//加载模式
public enum AssetLoadMode
{
    EditorMode,    //编辑器模式(该模式直接加载资源)
    AssetBundleMode,    //ab包模式(该模式所有资源使用ab方式加载,需要提前打包AB包)
}

public class AssetMgr:Singleton<AssetMgr>,IAssetLoader
{
    //加载器
    private IAssetLoader assetLoader;
    
    public void Init(AssetLoadMode LoadMode)
    {
        switch (LoadMode)
        {
            case AssetLoadMode.EditorMode:
#if UNITY_EDITOR
                assetLoader = new AssetEditorLoader();
#endif
                break;
            case AssetLoadMode.AssetBundleMode:
                assetLoader = new AssetBundleLoader();

                //加载AB资源列表，通过该列表判断是否有通用资源
                LoadManifest();
                //加载通用AB资源
                LoadShareAssets();
                break;
        }
    }
    
    //总的资源Manifest
    private AssetBundleManifest assetBundleManifest;
    private AssetBundle manifestAssetbundle;
    //加载Manifest
    private void LoadManifest()
    {
        manifestAssetbundle = AssetBundle.LoadFromFile($"{AB_ResFilePath.GetAssetBundleDirPath()}{AB_ResFilePath.GetRunTimePlatformName()}", 0, (ulong)SerializHelp.GetKey());

        assetBundleManifest = manifestAssetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

    }

    /// <summary>
    /// 加载共用资源，共用资源常驻内存
    /// </summary>
    private void LoadShareAssets()
    {
        if (!string.IsNullOrEmpty(ShareAssetABName.fonts))
        {
            LoadAllAssets<Font>(ShareAssetABName.fonts);
        }
        if (!string.IsNullOrEmpty(ShareAssetABName.shaders))
        {
            LoadAllAssets<Shader>(ShareAssetABName.shaders);
        }
        //加载通用图片
        if (!string.IsNullOrEmpty(ShareAssetABName.sharetextures))
        {
            string path = AB_ResFilePath.GetAssetBundleDirPath() + ShareAssetABName.sharetextures;
            AssetBundle assetBundle= AssetBundle.LoadFromFile(path, 0, (ulong)SerializHelp.GetKey());
            assetBundle.LoadAllAssets();
        }
        //加载通用材质
        if (!string.IsNullOrEmpty(ShareAssetABName.materials))
        {
           Material[] allShareMaterials = LoadAllAssets<Material>(ShareAssetABName.materials);
        }
        
        assetBundleManifest = null;
        manifestAssetbundle.Unload(true);
        manifestAssetbundle = null;
    }


    #region 同步加载API

    /// <summary>
    /// 加载单个资源--Gameobj放入对象池--一般在对象池里的物体无需释放
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="poolPolicy"></param>
    /// <returns></returns>
    public Transform LoadGameobjFromPool(string prefabName)
    {
        return assetLoader.LoadGameobjFromPool(prefabName);
    }

    public Transform LoadGameobj(string prefabName)
    {
        return assetLoader.LoadGameobj(prefabName);
    }

    /// <summary>
    /// 加载单个资源--需手动释放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string abName, string assetName) where T : Object
    {
        return assetLoader.LoadAsset<T>(abName, assetName);
    }

    /// <summary>
    /// 加载ab包内所有资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="abName"></param>
    /// <returns></returns>
    public T[] LoadAllAssets<T>(string abName) where T : Object
    {
        return assetLoader.LoadAllAssets<T>(abName);
    }

    /// <summary>
    /// 从ab包内加载图片--用完需要手动释放,否则该资源会一直在内存中
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="textureName"></param>
    /// <returns></returns>
    public Sprite LoadTexture(string abName, string textureName)
    {
        return assetLoader.LoadTexture(abName, textureName);
    }

    /// <summary>
    /// 从ab包内加载所有图片--用完需要手动释放,否则该资源会一直在内存中
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public Dictionary<string, Sprite> LoadAllTexture(string abName)
    {
        return assetLoader.LoadAllTexture(abName);
    }

    #endregion
    #region 异步加载API

    public void LoadTextureAsync(string abName, string textureName, Action<Sprite> callBackAction)
    {
        assetLoader.LoadTextureAsync(abName, textureName, callBackAction);
    }
    
    public void LoadAllTextureAsync(string abName, Action<Sprite[]> callBackAction)
    {
        assetLoader.LoadAllTextureAsync(abName, callBackAction);
    }
    
    public void LoadAssetAsync(string abName, string assetName, Action<object> callBackAction)
    {
        assetLoader.LoadAssetAsync(abName, assetName, callBackAction);
    }

    #endregion

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="abName"></param>
    public void UnloadAsset(string abName,bool unloadAllLoadedAssets,bool forceUnload)
    {
        assetLoader.UnloadAsset(abName, unloadAllLoadedAssets, forceUnload);
    }


    //AB包缓存字典
    private readonly Dictionary<string, AssetBundleCache> assetBundleCachesDic = new Dictionary<string, AssetBundleCache>();
    //获取缓存-如果获取不到则加载AB包,将加载的AB包添加进缓存
    public AssetBundleCache LoadAssetBundle(string abName)
    {
        if (assetBundleCachesDic.TryGetValue(abName, out var assetBundleCache))
        {
            assetBundleCache.AddRefenceCount();
            return assetBundleCache;
        }
        string fullPath = $"{AB_ResFilePath.GetAssetBundleDirPath()}{abName}";
        AssetBundle assetBundle = AssetBundle.LoadFromFile(fullPath, 0, (ulong)SerializHelp.GetKey());
        
        if (assetBundle != null)
        {
            assetBundleCache = new AssetBundleCache(assetBundle);
            assetBundleCachesDic.Add(abName, assetBundleCache);
            return assetBundleCache;
        }
        Debug.LogError($"加载 {abName} 出错！");
        return null;
    }

    public IEnumerator LoadOnesAssetBundCacheAsync<T>(string abName, string textureName, Action<string, string, T> loadedCallback) where T : Object
    {
        abName = abName.ToLower();
        AssetBundleRequest assetBundleRequest = null;
        if (assetBundleCachesDic.TryGetValue(abName, out var assetBundleCache))
        {
            assetBundleCache.AddRefenceCount();
            assetBundleRequest = assetBundleCache.Bundle.LoadAssetAsync<T>(textureName);
        }
        else
        {
            string fullPath = $"{AB_ResFilePath.GetAssetBundleDirPath()}{abName}";
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fullPath, 0, (ulong)SerializHelp.GetKey());
            yield return assetBundleCreateRequest.isDone;
            if (assetBundleCreateRequest.assetBundle != null)
            {
                assetBundleCache = new AssetBundleCache(assetBundleCreateRequest.assetBundle);
                assetBundleCachesDic.Add(abName, assetBundleCache);
                assetBundleRequest = assetBundleCreateRequest.assetBundle.LoadAssetAsync<T>(textureName);
            }
        }

        if (assetBundleRequest == null)
        {
            Debug.LogError($"加载{abName}出错!!");
            yield break;
        }
        yield return assetBundleRequest.isDone;

        loadedCallback?.Invoke(abName, textureName, assetBundleRequest.asset as T);

        yield return null;
    }

    public IEnumerator LoadAllAssetBundCacheAsync(string abName, Action<string, Texture2D[]> loadedCallback)
    {
        AssetBundleRequest assetBundleRequest = null;
        if (assetBundleCachesDic.TryGetValue(abName, out var assetBundleCache))
        {
            assetBundleCache.AddRefenceCount();
            assetBundleRequest = assetBundleCache.Bundle.LoadAllAssetsAsync<Texture2D>();
        }
        else
        {
            string fullPath =$"{AB_ResFilePath.GetAssetBundleDirPath()}{abName}";
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(fullPath, 0, (ulong)SerializHelp.GetKey());
            yield return assetBundleCreateRequest.isDone;
            if (assetBundleCreateRequest.assetBundle != null)
            {
                assetBundleCache = new AssetBundleCache(assetBundleCreateRequest.assetBundle);
                assetBundleCachesDic.Add(abName, assetBundleCache);
                assetBundleRequest = assetBundleCreateRequest.assetBundle.LoadAllAssetsAsync<Texture2D>();
            }
        }

        if (assetBundleRequest == null)
        {
            Debug.LogError($"加载{abName}出错!!");
            yield break;
        }

        yield return assetBundleRequest.isDone;

        if (loadedCallback != null)
        {
            int assetLenth = assetBundleRequest.allAssets.Length;
            Texture2D [] tempTexture2Ds=new Texture2D[assetLenth];
            for (int i = 0; i < assetLenth; i++)
            {
                tempTexture2Ds[i] = assetBundleRequest.allAssets[i] as Texture2D;
            }
            loadedCallback.Invoke(abName, tempTexture2Ds);
        }

        yield return null;
    }

    
    //获取ab缓存
    public AssetBundleCache GetAssetBundCache(string abName)
    {
        if (assetBundleCachesDic.TryGetValue(abName, out var assetBundleCache))
        {
            return assetBundleCache;
        }
        Debug.LogError($"找不到ab包:{abName}的缓存");
        return null;
    }
    //移除ab缓存
    public void RemoveAssetBundCache(string abName)
    {
        if (assetBundleCachesDic.ContainsKey(abName))
        {
            assetBundleCachesDic.Remove(abName);
        }
        else
        {
            Debug.LogError($"找不到ab包:{abName}的缓存");
        }
    }
}
