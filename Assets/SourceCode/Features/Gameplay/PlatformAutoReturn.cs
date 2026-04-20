using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAutoReturn : MonoBehaviour
{

    private float returnDistanceBelowCamera = 30f;

    private PlatformPool pool;
    private Camera mainCam;

    void Start()
    {
        pool = FindObjectOfType<PlatformPool>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (pool == null || mainCam == null) return;

        float limitY = mainCam.transform.position.y - returnDistanceBelowCamera;

        if (transform.position.y < limitY)
        {
            pool.ReturnPlatform(gameObject);
        }
    }
    
}
