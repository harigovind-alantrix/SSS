using System.Collections;
using System.Collections.Generic;
using Core.Models;
using Features.Gameplay;
using UnityEngine;

public class CloneFactory : MonoBehaviour
{
    public GameObject clonePrefab;

    public GameObject SpawnClone(List<RecordingData> data)
    {
        if (data == null || data.Count == 0)
            return null;

        GameObject clone = Instantiate(
            clonePrefab,
            data[0].Position,
            data[0].Rotation
        );

        ClonePlayback playback = clone.GetComponent<ClonePlayback>();
        playback.Initialize(data);

        return clone;
    }
}
