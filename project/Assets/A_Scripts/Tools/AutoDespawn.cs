using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public float DespawnTime = 10;

    private void OnEnable()
    {
        PoolMgr.Instance._SpawnPool.Despawn(transform, DespawnTime); 
    }

}
