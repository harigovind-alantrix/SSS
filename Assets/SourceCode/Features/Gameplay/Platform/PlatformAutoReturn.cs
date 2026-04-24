using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAutoReturn : MonoBehaviour
{
    private const float ReturnDistanceBelowCamera = 30f;

    private PlatformPool _pool;
    private Camera _mainCam;

    public void SetPool(PlatformPool pool)
    {
        _pool = pool;
        _mainCam = Camera.main;
    }

    public void OnReturnedToPool()
    {
        _pool = null;
        _mainCam = null;
    }

    void Update()
    {
        if (_pool == null || _mainCam == null) return;

        if (transform.position.y < _mainCam.transform.position.y - ReturnDistanceBelowCamera)
            _pool.ReturnPlatform(gameObject);
    }
}