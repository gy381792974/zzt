using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using UnityEngine;

public class PoolMgr:Singleton<PoolMgr>
{
    private  SpawnPool spawnPool;

    public  SpawnPool _SpawnPool
    {
        get
        {
            if (spawnPool == null)
            {
                spawnPool = GetComponent<SpawnPool>();
            }

            return spawnPool;
        }
    }

    public void CreatePool(Transform prefab)
    {
        PrefabPool prefabPool = GetPrefabPool(prefab);
        _SpawnPool._perPrefabPoolOptions.Add(prefabPool);
        _SpawnPool.CreatePrefabPool(prefabPool);
    }
    /// <summary>
    /// 创建一个对象池
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="poolPolicy"></param>
    /// <returns></returns>
    private  PrefabPool GetPrefabPool(Transform prefab)
    {
        PrefabPool prefabPool = new PrefabPool(prefab);
        //取消实例限制
        prefabPool.limitInstances = false;
        //开启自动清理池子
        prefabPool.cullDespawned = false;
        prefabPool.prefab = prefab;
        return prefabPool;
    }
    
    /// <summary>
    /// 回收一个对象
    /// </summary>
    /// <param name="trans"></param>
    public  void DespawnOne(Transform trans)
    {
        if (_SpawnPool.IsSpawned(trans))
        {
            _SpawnPool.Despawn(trans);
        }
    }

    /// <summary>
    /// 回收一个池子里的对象
    /// </summary>
    /// <param name="poolName"></param>
    public void DespawnOnePool(string poolName)
    {
        if (_SpawnPool.prefabPools.TryGetValue(poolName, out var prefabPool))
        {
            int spawnedCount = prefabPool.spawned.Count;

            for (int i = spawnedCount - 1; i >= 0; i--)
            {
                //DespawnOne(prefabPool.spawned[i]);
                _SpawnPool.Remove(prefabPool.spawned[i]);
            }
        }
    }

    /// <summary>
    /// 回收所有池子的所有对象
    /// </summary>
    public  void DespawnAll()
    {
        _SpawnPool.DespawnAll();
    }

}
