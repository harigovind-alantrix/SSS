using UnityEngine;

namespace Data.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 8f;

        [Header("Jump")]
        public float jumpForce = 10f;
        public float gravity = -25f;
        public float playerHeight = 1f;

        [Header("Rotation")]
        public float rotationSpeed = 360f;

        [Header("Boost")]
        public float boostForce = 10f;
        public float upwardForce = 6f;
        public float boostDuration = 0.25f;

        [Header("Camera")]
        public float zoomSpeed = 5f;
        public float zoomInFOV = 60f;
        public float zoomOutFOV = 48f;
    }
}