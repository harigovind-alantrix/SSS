using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAutoReturn : MonoBehaviour
{
    public float destroyY = -10f;

    private PlatformPool pool;

    void Start()
    {
        pool = FindObjectOfType<PlatformPool>();
    }

    void Update()
    {
        if (transform.position.y < destroyY)
        {
            pool.ReturnPlatform(gameObject);
        }
    }

}
