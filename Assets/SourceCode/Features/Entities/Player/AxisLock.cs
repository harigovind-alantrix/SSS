using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisLock : MonoBehaviour
{
    public bool lockZ = true;
    public float fixedZ = 0f;

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        if (lockZ)
            pos.z = fixedZ;

        transform.position = pos;
    }
}
