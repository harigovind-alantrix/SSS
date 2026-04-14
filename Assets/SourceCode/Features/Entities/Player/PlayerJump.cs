using Core.Messages.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Entities.Player
{
    public class PlayerJump : MonoBehaviour
    {
        public float jumpForce = 10f;
        public float gravity = -25f;
        public float playerHeight = 1f;

        private float yVelocity;
        private bool isJumping;

        private GroundChecker groundChecker;
        
        private IPublisher<CameraZoomEvent> _zoomPublisher;

        [Inject]
        public void Construct(IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _zoomPublisher = zoomPublisher;
        }
        void Awake()
        {
            groundChecker = GetComponent<GroundChecker>();
        }

        void Update()
        {
            HandleJumpMotion();
        }

        public void Jump()
        {
            if (groundChecker.IsGrounded())
            {
                yVelocity = jumpForce;
                isJumping = true;
                _zoomPublisher?.Publish(new CameraZoomEvent(true));
            }
        }
        public bool IsJumping()
        {
            return isJumping;
        }

        void HandleJumpMotion()
        {
            Vector3 pos = transform.position;

            if (isJumping)
            {
                // Apply gravity only during jump
                yVelocity += gravity * Time.deltaTime;
                pos.y += yVelocity * Time.deltaTime;

                // Check ground
                RaycastHit hit;
                if (groundChecker.TryGetGround(out hit))
                {
                    float groundY = hit.point.y + (playerHeight / 2f);

                    // Land condition
                    if (pos.y <= groundY)
                    {
                        pos.y = groundY;
                        yVelocity = 0;
                        isJumping = false;
                        _zoomPublisher?.Publish(new CameraZoomEvent(false));
                    }
                }

                transform.position = pos;
            }
            else
            {
                // Always stick to ground when not jumping
                RaycastHit hit;
                if (groundChecker.TryGetGround(out hit))
                {
                    pos.y = hit.point.y + (playerHeight / 2f);
                    transform.position = pos;
                }
            }
        }
    }
}

