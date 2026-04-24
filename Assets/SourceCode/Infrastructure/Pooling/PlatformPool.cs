using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using VContainer;

public class PlatformPool : MonoBehaviour
{
    [SerializeField]
    private PlatformAutoReturn platformPrefab;

    [SerializeField]
    private int initialSize = 20;

    [SerializeField]
    private int maxSize = 60;

    [SerializeField]
    private Transform poolContainer;

    private ObjectPool<PlatformAutoReturn> _pool;

    [Inject]
    public void Construct(IObjectResolver container)
    {
        _pool = new ObjectPool<PlatformAutoReturn>(
            container:   container,
            prefab:      platformPrefab,
            parent:      poolContainer != null ? poolContainer : transform,
            initialSize: initialSize,
            poolName:    "[PlatformPool]",
            maxSize:     maxSize);
    }

    public GameObject GetPlatform()
    {
        var platform = _pool.Get();
        platform.SetPool(this);
        return platform.gameObject;
    }

    public void ReturnPlatform(GameObject obj)
    {
        var platform = obj.GetComponent<PlatformAutoReturn>();
        if (platform == null) return;
 
        platform.OnReturnedToPool();
        _pool.Return(platform);
    }
 
    void OnDestroy() => _pool?.Dispose();
}