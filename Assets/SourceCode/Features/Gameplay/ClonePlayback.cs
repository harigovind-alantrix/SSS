using System.Collections;
using System.Collections.Generic;
using Core.Models;
using Entities.Player;
using UnityEngine;
namespace Features.Gameplay
{
    public class ClonePlayback : MonoBehaviour
    {
        private List<RecordingData> playbackData;
        private float timer = 0f;
        private int currentFrame = 0;

        public float playbackRate = 0.02f;

        private CloneManager manager;
        private bool isActivated = false;

        public void Initialize(List<RecordingData> data)
        {
            playbackData = data;

            // 🔥 AUTO FIND MANAGER
            manager = FindObjectOfType<CloneManager>();

            // CLONE MODE
            GetComponent<PlayerController>().enabled = false;
            GetComponent<RecordingSystem>().enabled = false;

            enabled = true;
        }

        void Update()
        {
            if (playbackData == null || isActivated) return;

            timer += Time.deltaTime;

            if (timer >= playbackRate)
            {
                timer = 0f;
                PlayMovement();
            }
        }

        void PlayMovement()
        {
            if (currentFrame >= playbackData.Count)
            {
                ActivateClone();
                return;
            }

            RecordingData frame = playbackData[currentFrame];

            transform.position = frame.Position;
            transform.rotation = frame.Rotation;

            currentFrame++;
        }

        void ActivateClone()
        {
            isActivated = true;

            Debug.Log("Clone is now ACTIVE PLAYER");

            // PLAYER MODE
            GetComponent<PlayerController>().enabled = true;
            GetComponent<RecordingSystem>().enabled = true;

            enabled = false;

            // 🔥 SWITCH CONTROL
            manager.SwitchToClone(gameObject);
        }
    }
}

