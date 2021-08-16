#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

/// <summary>
/// 编辑器模型加载
/// </summary>
public class AssetEditorLoader : IAssetLoader
{
    public Transform LoadGameobjFromPool(string prefabName)
    {
        if (!PoolMgr.Instance._SpawnPool.prefabPools.ContainsKey(prefabName))
        {
            GameObject objAsset = GetAsset<GameObject>(prefabName, prefabSearchFolder);
            if (objAsset != null)
            {
                //创建对象池
                PoolMgr.Instance.CreatePool(objAsset.transform);
            }
            else
            {
                Debug.LogError($"加载失败：{prefabName}");
            }
        }

        return PoolMgr.Instance._SpawnPool.Spawn(prefabName);
    }

    public Transform LoadGameobj(string prefabName)
    {
        GameObject objAsset = GetAsset<GameObject>(prefabName, prefabSearchFolder);
        if (objAsset != null)
        {
            return Object.Instantiate(objAsset).transform;
        }
        Debug.LogError($"加载失败：{prefabName}");
        return null;
    }


    public T LoadAsset<T>(string abName, string assetName) where T : Object
    {
        return GetAsset<T>(assetName, root_SearchFolder);
    }

    public T[] LoadAllAssets<T>(string abName) where T : Object
    {
        return GetAllAsset<T>(abName, root_SearchFolder);
    }


    public void LoadAssetAsync(string abName, string assetName, Action<object> callBackAction)
    {
        Object asset = GetAsset<Object>(assetName, root_SearchFolder);
        if (asset != null)
        {
            callBackAction?.Invoke(asset);
        }
        else
        {
            Debug.LogError($"加载{assetName}出错！！");
        }
    }

    public void LoadTextureAsync(string abName, string textureName, Action<Sprite> callBackAction)
    {
        Sprite sprite = LoadTexture(abName, textureName);
        if (sprite != null)
        {
            callBackAction?.Invoke(sprite);
        }
    }


    public void LoadAllTextureAsync(string abName, Action<Sprite[]> callBackAction)
    {
        Sprite[] sprites = GetAllAsset<Sprite>(abName, texture_ab_SearchFolder);
        if (sprites != null)
        {
            callBackAction?.Invoke(sprites);
        }
    }

    public Sprite LoadTexture(string abName, string textureName)
    {
        return GetAsset<Sprite>(textureName, texture_ab_SearchFolder);
    }

    public Dictionary<string, Sprite> LoadAllTexture(string abName)
    {
        Sprite[] sprites = GetAllAsset<Sprite>(abName, texture_ab_SearchFolder);
        Dictionary<string,Sprite> tempDic=new Dictionary<string, Sprite>(sprites.Length);
        for (int i = 0; i < sprites.Length; i++)
        {
            tempDic.Add(sprites[i].name,sprites[i]);
        }

        return tempDic;
    }


    public void UnloadAsset(string abName, bool unloadAllLoadedObj,bool forceUnload)
    {
        
        if (forceUnload)
        {
            Debug.Log($"强制卸载ab包：{abName}！释放已经加载的资源:{unloadAllLoadedObj}");
        }
        else
        {
            Debug.Log($"卸载ab包：{abName}！释放已经加载的资源:{unloadAllLoadedObj}");
        }
       
       // EditorUtility.UnloadUnusedAssetsImmediate();
        
    }
    
    readonly string[] root_SearchFolder = new[] { AB_ResFilePath.abRootDir };
    readonly string[] prefabSearchFolder = new[] { RemoveLastStr(AB_ResFilePath.abPrefabPackRootDir) };
    readonly string[] texture_ab_SearchFolder = new[] { RemoveLastStr(AB_ResFilePath.abTextureFolderPackRootDir) , RemoveLastStr(AB_ResFilePath.abTextureSinglePackRootDir)  };

    //移除最后一个“/”
    private static string RemoveLastStr(string value)
    {
        string charValue = value[value.Length - 1].ToString();
        if (charValue.Equals("/"))
        {
            value = value.Substring(0, value.Length - 1);
        }
        return value;
    }

    private T GetAsset<T>(string assetName,string [] searchFolder) where T : Object
    {
        string filiter = assetName;

        string[] findAssets = AssetDatabase.FindAssets(filiter, searchFolder);
        for (int i = 0; i < findAssets.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(findAssets[i]);
            string targetName = Path.GetFileNameWithoutExtension(assetPath);
            if (string.IsNullOrEmpty(targetName))
            {
                continue;
            }

            targetName = targetName.ToLower();
            if (targetName.Equals(assetName.ToLower()))
            {
                if (File.Exists(assetPath))//判断是否是资源而不是文件夹
                {
                    return AssetDatabase.LoadAssetAtPath<T>(assetPath);
                }
            }
        }
        Debug.LogError($"找不到资源:{assetName}");
        return null;
    }

    private T[] GetAllAsset<T>(string abName,string []searchFolder) where T : Object
    {
        List<T> tempList=new List<T>();
        string filiter = abName;

        string[] findAssets = AssetDatabase.FindAssets(filiter, searchFolder);
        for (int i = 0; i < findAssets.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(findAssets[i]);
            string targetName = Path.GetFileNameWithoutExtension(assetPath);
            if (string.IsNullOrEmpty(targetName))
            {
                continue;
            }
            
            targetName = targetName.ToLower();
            if (targetName.Equals(abName.ToLower()))
            {
                DirectoryInfo directoryInfo=new DirectoryInfo(assetPath);
                if (directoryInfo.Exists)
                {
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    for (int j = 0; j < fileInfos.Length; j++)
                    {
                        string obsPath = fileInfos[j].FullName;
                        if (!Path.GetExtension(obsPath).Equals(".meta"))
                        {
                            int index = obsPath.IndexOf("Assets", StringComparison.Ordinal);
                            string relativePath = obsPath.Substring(index, obsPath.Length - index);
                            tempList.Add(AssetDatabase.LoadAssetAtPath<T>(relativePath));
                        }
                    }
                }
            }
        }

        return tempList.ToArray();
    }
    
}
#endif