using Core.Messages.Gameplay;
using MessagePipe;
using Data.Configs;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerJump
    {
        private readonly Rigidbody _rb;
        private readonly PlayerConfig _config;
        private readonly GroundChecker _groundChecker;
        private readonly IPublisher<CameraZoomEvent> _zoomPublisher;

        private bool _wasGrounded;

        public PlayerJump(
            Rigidbody rb,
            PlayerConfig config,
            GroundChecker groundChecker,
            IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _rb = rb;
            _config = config;
            _groundChecker = groundChecker;
            _zoomPublisher = zoomPublisher;
        }

        public void Tick()
        {
            bool isGrounded = _groundChecker.IsGrounded();
            
            if (isGrounded && !_wasGrounded)
            {
                _zoomPublisher?.Publish(new CameraZoomEvent(false));
            }
            
            _rb.AddForce(Vector3.up * _config.gravity, ForceMode.Acceleration);

            _wasGrounded = isGrounded;
        }

        public void Jump()
        {
            if (_groundChecker.IsGrounded())
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _config.jumpForce, _rb.velocity.z);
                _zoomPublisher?.Publish(new CameraZoomEvent(true));
            }
        }

        public bool IsJumping() => !_groundChecker.IsGrounded();
    }
}