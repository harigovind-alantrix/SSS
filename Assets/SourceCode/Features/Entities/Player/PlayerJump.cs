using Core.Messages.Gameplay;
using MessagePipe;
using Data.Configs;
using UnityEngine;
using VContainer;

namespace Entities.Player
{
    public class PlayerJump
    {
        private readonly Transform _transform;
        private readonly PlayerConfig _config;
        private readonly GroundChecker _groundChecker;
        private readonly IPublisher<CameraZoomEvent> _zoomPublisher;

        private float _yVelocity;
        private bool _isJumping;

        public PlayerJump(
            Transform transform,
            PlayerConfig config,
            GroundChecker groundChecker,
            IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _config = config;
            _transform = transform;
            _groundChecker = groundChecker;
            _zoomPublisher = zoomPublisher;
        }


        public void Tick()
        {
            HandleJumpMotion();
        }

        public void Jump()
        {
            if (_groundChecker.IsGrounded())
            {
                _yVelocity = _config.jumpForce;
                _isJumping = true;
                _zoomPublisher?.Publish(new CameraZoomEvent(true));
            }
        }

        public bool IsJumping()
        {
            return _isJumping;
        }

       private void HandleJumpMotion()
        {
            Vector3 pos = _transform.position;

            if (_isJumping)
            {
                // Apply gravity only during jump
                _yVelocity += _config.gravity * Time.deltaTime;
                pos.y += _yVelocity * Time.deltaTime;

                // Check ground
                RaycastHit hit;
                if (_groundChecker.TryGetGround(out hit))
                {
                    float groundY = hit.point.y + (_config.playerHeight / 2f);

                    // Land condition
                    if (pos.y <= groundY)
                    {
                        pos.y = groundY;
                        _yVelocity = 0;
                        _isJumping = false;
                        _zoomPublisher?.Publish(new CameraZoomEvent(false));
                    }
                }

                _transform.position = pos;
            }
            else
            {
                // Always stick to ground when not jumping
                RaycastHit hit;
                if (_groundChecker.TryGetGround(out hit))
                {
                    pos.y = hit.point.y + (_config.playerHeight / 2f);
                    _transform.position = pos;
                }
            }
        }
    }
}