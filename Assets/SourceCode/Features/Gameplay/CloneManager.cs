using System.Collections.Generic;
using Core.Models;
using Entities.Player;
using Features.Gameplay;
using UnityEngine;
using Cinemachine;
namespace Features.Gameplay
{
    public class CloneManager : MonoBehaviour
    {
        public GameObject currentPlayer;
        public GameObject clonePrefab;

        public void HandleClone()
        {
            if (currentPlayer == null)
            {
                Debug.Log("No current player!");
                return;
            }

            RecordingSystem recording = currentPlayer.GetComponent<RecordingSystem>();

            if (recording == null)
            {
                Debug.Log("RecordingSystem missing!");
                return;
            }

            List<RecordingData> data = recording.ConsumeRecording();

            if (data == null || data.Count == 0)
            {
                Debug.Log("No recording data");
                return;
            }

            GameObject clone = Instantiate(
                clonePrefab,
                data[0].Position,
                data[0].Rotation
            );

            clone.GetComponent<ClonePlayback>().Initialize(data);
        }

        public void SwitchToClone(GameObject newClone)
        {
            GameObject oldPlayer = currentPlayer;

            currentPlayer = newClone;

            if (oldPlayer != null)
            {
                Destroy(oldPlayer);
            }

            Debug.Log("Switched to new clone!");
        }
    }
}
