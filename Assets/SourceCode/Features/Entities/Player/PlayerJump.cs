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
        private readonly IPublisher<CameraZoomEvent> _zoomPublisher;

        private bool _wasGrounded;
        private bool _currentGrounded;
            
        private float _lastJumpTime; 
        private const float JumpCooldown = .7f;

        public PlayerJump(
            Rigidbody rb,
            PlayerConfig config,
            IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _rb = rb;
            _config = config;
            _zoomPublisher = zoomPublisher;
        }

        public void Tick(bool isGrounded)
        {
            _currentGrounded = isGrounded;
            
            if (isGrounded && !_wasGrounded)
            {
                _zoomPublisher?.Publish(new CameraZoomEvent(false));
            }
            
            _rb.AddForce(Vector3.up * _config.gravity, ForceMode.Acceleration);

            _wasGrounded = isGrounded;
        }

        public void Jump(bool isGrounded)
        {
            if (isGrounded && Time.time >= _lastJumpTime + JumpCooldown)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _config.jumpForce, _rb.velocity.z);
                _lastJumpTime = Time.time;
                _zoomPublisher?.Publish(new CameraZoomEvent(true));
            }
        }
        public bool IsJumping() => !_currentGrounded;
    }
}