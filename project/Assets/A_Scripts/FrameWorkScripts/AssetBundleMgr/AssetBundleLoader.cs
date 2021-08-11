using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// ab包模式加载
/// </summary>
public class AssetBundleLoader:IAssetLoader
{
    /// <summary>
    /// 加载资源时放入对象池
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="poolPolicy"></param>
    /// <returns></returns>
    public Transform LoadGameobjFromPool(string prefabName)
    {
        if (!PoolMgr.Instance._SpawnPool.prefabPools.ContainsKey(prefabName))
        {
            AssetBundle assetBundle = GetAssetBundle(prefabName);
            GameObject objAsset = assetBundle.LoadAsset<GameObject>(prefabName);
            //创建对象池
            PoolMgr.Instance.CreatePool(objAsset.transform);

            UnloadAsset(prefabName, false,false);
        }

        return PoolMgr.Instance._SpawnPool.Spawn(prefabName);
    }

    public Transform LoadGameobj(string prefabName)
    {
        AssetBundle assetBundle = GetAssetBundle(prefabName.ToLower());
        GameObject objAsset = assetBundle.LoadAsset<GameObject>(prefabName);
        if (objAsset != null)
        {
            return Object.Instantiate(objAsset).transform;
        }
        Debug.LogError($"加载:{prefabName}错误！！");
        return null;
    }

    //卸载资源
    public void UnloadAsset(string abName,bool unloadAllLoadedAssets, bool forceUnload)
    {
        abName = abName.ToLower();
        AssetBundleCache assetBundleCache = AssetMgr.Instance.GetAssetBundCache(abName);
        if (assetBundleCache != null)
        {
            if (assetBundleCache.Unload(unloadAllLoadedAssets, forceUnload))
            {
                AssetMgr.Instance.RemoveAssetBundCache(abName);
                assetBundleCache = null;
            }
        }
    }
    
    private Rect spriteRect=new Rect(0,0,0,0);
    private Vector2 spritePivot=new Vector2(0.5f,0.5f);
    public Sprite LoadTexture(string abName, string textureName)
    {
        AssetBundle assetBundle = GetAssetBundle(abName);
        Texture2D texture2D = assetBundle.LoadAsset<Texture2D>(textureName);
        spriteRect.width = texture2D.width;
        spriteRect.height = texture2D.height;
        return Sprite.Create(texture2D, spriteRect, spritePivot);
    }


    public Dictionary<string, Sprite> LoadAllTexture(string abName)
    {
        AssetBundle assetBundle = GetAssetBundle(abName);
        Texture2D[] objAssets = assetBundle.LoadAllAssets<Texture2D>();

        Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>(objAssets.Length);
        for (int i = 0; i < objAssets.Length; i++)
        {
            spriteRect.width = objAssets[i].width;
            spriteRect.height = objAssets[i].height;
            spriteDic.Add(objAssets[i].name,Sprite.Create(objAssets[i], spriteRect, spritePivot));
        }
        return spriteDic;
    }
    
    public T LoadAsset<T>(string abName, string assetName) where T : Object
    {
        AssetBundle assetBundle = GetAssetBundle(abName);
        T objAsset = assetBundle.LoadAsset<T>(assetName);
        if (objAsset != null)
        {
            return objAsset;
        }
        Debug.LogError($"加载:{abName}:{assetName}错误！！");
        return null;
    }
    
    public T[] LoadAllAssets<T>(string abName) where T : Object
    {
        AssetBundle assetBundle = GetAssetBundle(abName);
        if (assetBundle != null)
        {
            T[] objAssets = assetBundle.LoadAllAssets<T>();
            return objAssets;
        }
        Debug.LogError($"加载:{abName}错误！！");
        return null;
    }

    
    private Dictionary<string, Action<Sprite>> loadOneTextureActions=new Dictionary<string, Action<Sprite>>(10);
    private Dictionary<string, Action<Sprite[]>> loadAllTextureActions = new Dictionary<string, Action<Sprite[]>>(10);
    private Dictionary<string, Action<Object>> loadAssetsActions = new Dictionary<string, Action<Object>>(10);
    
    public void LoadTextureAsync(string abName, string textureName,  Action<Sprite> callBackAction)
    {
        string loadAssetKey = $"{abName}{textureName}";
        //相同资源仅加载一次
        if (loadOneTextureActions.ContainsKey(loadAssetKey))
        {
            return;
        }
        
        loadOneTextureActions.Add(loadAssetKey, callBackAction);

        AssetMgr.Instance.StartCoroutine(AssetMgr.Instance.LoadOnesAssetBundCacheAsync<Texture2D>(abName, textureName,OnLoadedOneTextureCallback));
    }

    private void OnLoadedOneTextureCallback(string abName, string textureName, Texture2D texture2D)
    {
        string loadAssetKey = $"{abName}{textureName}";
        if (loadOneTextureActions.TryGetValue(loadAssetKey, out var laodAction))
        {
            Sprite sprite = null;
            if (texture2D != null)
            {
                spriteRect.width = texture2D.width;
                spriteRect.height = texture2D.height;
                sprite = Sprite.Create(texture2D, spriteRect, spritePivot);
            }

            if (sprite != null)
            {
                laodAction?.Invoke(sprite);
            }
            else
            {
                Debug.LogError($"加载{abName}出错，不进入回调！");
            }
            loadOneTextureActions.Remove(loadAssetKey);
        }
    }
    

    public void LoadAllTextureAsync(string abName, Action<Sprite[]> callBackAction)
    {
        //相同资源仅加载一次
        if (loadAllTextureActions.ContainsKey(abName))
        {
            return;
        }

        loadAllTextureActions.Add(abName, callBackAction);

        AssetMgr.Instance.StartCoroutine(AssetMgr.Instance.LoadAllAssetBundCacheAsync(abName, OnLoadedAllTextureCallback));
    }


    private void OnLoadedAllTextureCallback(string abName, Texture2D []texture2D)
    {
        if (loadAllTextureActions.TryGetValue(abName, out var laodAction))
        {
            Sprite []spriteArray = null;
            if (texture2D != null)
            {
                int lenth = texture2D.Length;
                spriteArray =new Sprite[lenth];
                for (int i = 0; i < lenth; i++)
                {
                    Texture2D sigleTexture2D = texture2D[i];
                    spriteRect.width = sigleTexture2D.width;
                    spriteRect.height = sigleTexture2D.height;
                    spriteArray[i]  = Sprite.Create(sigleTexture2D, spriteRect, spritePivot);
                }
            }

            if (spriteArray != null)
            {
                laodAction?.Invoke(spriteArray);
            }
            else
            {
                Debug.LogError($"加载{abName}出错，不进入回调！");
            }
            loadAllTextureActions.Remove(abName);
        }
    }

    public void LoadAssetAsync(string abName, string assetName, Action<object> callBackAction)
    {
        if (loadAssetsActions.ContainsKey(abName))
        {
            return;
        }

        loadAssetsActions.Add(abName, callBackAction);

        AssetMgr.Instance.StartCoroutine(AssetMgr.Instance.LoadOnesAssetBundCacheAsync<Object>(abName, assetName, OnLoadedOneAssetCallback));
    }

    private void OnLoadedOneAssetCallback(string abName, string textureName, Object obj)
    {
        string loadAssetKey = $"{abName}{textureName}";
        if (loadAssetsActions.TryGetValue(loadAssetKey, out var laodAction))
        {
            if (obj != null)
            {
                laodAction?.Invoke(obj);
            }
            else
            {
                Debug.LogError($"加载{abName}出错，不进入回调！");
            }
            loadAssetsActions.Remove(loadAssetKey);
        }
    }

    /// <summary>
    /// 获取ab包
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="dontUnLoad">该参数表名该ab包是否是自动卸载，true:手动卸载，且会连同已经加载
    /// 出来的资源都一起卸载，false：自动卸载，仅卸载未加载的内容</param>
    /// <returns></returns>
    private AssetBundle GetAssetBundle(string abName)
    {
        return AssetMgr.Instance.LoadAssetBundle(abName.ToLower()).Bundle;
    }
}
