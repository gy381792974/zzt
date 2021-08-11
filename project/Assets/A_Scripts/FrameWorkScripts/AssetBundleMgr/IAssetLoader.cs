using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAssetLoader
{
    //同步加载
    //加载Prefab并放入对象池
    Transform LoadGameobjFromPool(string prefabName);

    //加载Prefab
    Transform LoadGameobj(string prefabName);

    //从ab包中加载图片
    Sprite LoadTexture(string abName, string textureName);
    
    //从ab包中加载所有图片
    Dictionary<string, Sprite> LoadAllTexture(string abName);

    T LoadAsset<T>(string abName, string assetName) where T : Object;

    T[] LoadAllAssets<T>(string abName) where T : Object;

    //异步加载
    void LoadTextureAsync(string abName, string textureName, Action<Sprite> callBackAction);

    void LoadAllTextureAsync(string abName, Action<Sprite[]> callBackAction);

    void LoadAssetAsync(string abName, string assetName, Action<object> callBackAction);


    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="unloadAllLoadedAssets">是否卸载已经加载的资源</param>
    /// <param name="forceUnload">强制卸载，无论有多少次引用</param>
    void UnloadAsset(string abName,bool unloadAllLoadedAssets,bool forceUnload);


}