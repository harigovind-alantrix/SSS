using System.Collections;
using System.Collections.Generic;
using Core.Models;
using UnityEngine;
namespace Features.Gameplay
{
    public class RecordingSystem : MonoBehaviour
    {
        private List<RecordingData> recordedFrames = new List<RecordingData>();

        // Recording settings
        private float recordTimer = 0f;
        private float recordInterval = 0.02f; // 50 FPS recording

        private Vector3 lastPosition;

        // Memory control
        private int maxFrames = 500;

        void Start()
        {
            lastPosition = transform.position;
        }

        void Update()
        {
            RecordFrame();
        }

        void RecordFrame()
        {
            recordTimer += Time.deltaTime;

            if (recordTimer < recordInterval)
                return;

            recordTimer = 0f;

            // Skip if player hasn't moved
            if (Vector3.Distance(transform.position, lastPosition) < 0.001f)
                return;

            recordedFrames.Add(
                new RecordingData(transform.position, transform.rotation)
            );

            lastPosition = transform.position;

            // Limit memory (rolling buffer)
            if (recordedFrames.Count > maxFrames)
            {
                recordedFrames.RemoveAt(0);
            }
        }

        // Called when player presses CLONE
        public List<RecordingData> ConsumeRecording()
        {
            List<RecordingData> data = new List<RecordingData>(recordedFrames);

            recordedFrames.Clear(); // reset after clone

            return data;
        }

        // Optional debug
        public int GetFrameCount()
        {
            return recordedFrames.Count;
        }
    }

}
