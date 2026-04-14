using System;
using Core.Messages.Gameplay;
using Features.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Entities.Player
{
    public class PlayerController : MonoBehaviour ,IDisposable
    {
        private PlayerMovement _movement;
        private PlayerJump _jump;
        private PlayerRotation _rotation;
        
        private IDisposable _subscriptions;
        
        [Inject]
        public void Construct(
            ISubscriber<MoveInputEvent> moveSubscriber,
            ISubscriber<JumpInputEvent> jumpSubscriber)
        {
            var d = DisposableBag.CreateBuilder();
 
            moveSubscriber.Subscribe(OnMove).AddTo(d);
            jumpSubscriber.Subscribe(OnJump).AddTo(d);
            _subscriptions = d.Build();
        }
        void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _jump = GetComponent<PlayerJump>();
            _rotation = GetComponent<PlayerRotation>();
        }
        private void OnMove(MoveInputEvent evt)
        {
            _movement.Move(evt.Axis);
            _rotation.Rotate(evt.Axis);
        }
        private void OnJump(JumpInputEvent _)
        {
            _jump.Jump();
        }
        public void Freeze()
        {
            Dispose();
            
            if (_movement != null) _movement.enabled = false;
            if (_jump     != null) _jump.enabled     = false;
            if (_rotation != null) _rotation.enabled = false;
 
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity        = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic     = true;
            }
        }
 
        public void Dispose()
        {
            _subscriptions?.Dispose();
        }
 
        void OnDestroy() => Dispose();
    }
}