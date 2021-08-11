using System.Collections.Generic;
using EazyGF;
using UnityEngine;
using UnityEngine.U2D;

// AssetBundle缓存
public class AssetBundleCache
{
    private int refenceCount = 0;//该ab包引用次数

    public AssetBundleCache(AssetBundle ab)
    {
        Bundle = ab;
        refenceCount = 1;
    }

    public AssetBundle Bundle
    {
        get;
        private set;
    }

    /// <summary>
    /// 增加一次引用
    /// </summary>
    public void AddRefenceCount()
    {
        refenceCount += 1;
    }

    /// <summary>
    /// 卸载ab包
    /// </summary>
    /// <param name="unloadAllLoadedObj">是否卸载已经加载过的资源</param>
    /// <param name="forceUnload">强制卸载，无论有多少次引用</param>
    /// <returns></returns>
    public bool Unload(bool unloadAllLoadedObj,bool forceUnload)
    {
        if (Bundle != null)
        {
            if (forceUnload)
            {
                Debug.Log($"强制卸载ab包:{Bundle.name}");
                Bundle.Unload(unloadAllLoadedObj);
                Bundle = null;
                return true;
            }
            else
            {
                refenceCount--;

                if (refenceCount == 0)
                {
                    Debug.Log($"卸载ab包：{Bundle.name}！卸载已经加载的资源：{unloadAllLoadedObj}");
                    Bundle.Unload(unloadAllLoadedObj);
                    Bundle = null;
                    return true;
                }
                Debug.Log($"卸载ab包:{Bundle.name}失败，还有{refenceCount}次引用");
            }
        }

        return false;
    }
}