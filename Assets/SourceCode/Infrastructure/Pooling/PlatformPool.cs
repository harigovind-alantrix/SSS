using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public GameObject platformPrefab;
    public int poolSize = 20;
    public Transform Container;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab, Container);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetPlatform()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(platformPrefab, Container);
        return newObj;
    }

    public void ReturnPlatform(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
