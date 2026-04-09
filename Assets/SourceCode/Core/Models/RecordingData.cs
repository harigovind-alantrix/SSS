using UnityEngine;
namespace Core.Models
{
    public class RecordingData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public RecordingData(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }
}

