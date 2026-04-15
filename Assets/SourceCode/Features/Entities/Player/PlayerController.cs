using System;
using Core.Messages.Gameplay;
using Data.Configs;
using Features.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Entities.Player
{
    public class PlayerController : MonoBehaviour, IDisposable
    {
        private PlayerMovement _movement;
        private PlayerJump _jump;
        private PlayerRotation _rotation;
        private GroundChecker  _groundChecker; 

        private IDisposable _subscriptions;
        private float _lastMoveAxis;

        [Inject]
        public void Construct(PlayerConfig config,
            ISubscriber<MoveInputEvent> moveSubscriber,
            ISubscriber<JumpInputEvent> jumpSubscriber,
            IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _movement = new PlayerMovement(transform, config);
            _jump = new PlayerJump(transform, config, _groundChecker, zoomPublisher);
            _rotation = new PlayerRotation(transform, config, _jump);

            var d = DisposableBag.CreateBuilder();
            moveSubscriber.Subscribe(OnMove).AddTo(d);
            jumpSubscriber.Subscribe(OnJump).AddTo(d);
            _subscriptions = d.Build();
        }
        void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
        }
        void Update()
        {
            _jump?.Tick();
            _rotation?.Rotate(_lastMoveAxis);
        }

        private void OnMove(MoveInputEvent evt)
        {
            _lastMoveAxis = evt.Axis;
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
            _movement = null;
            _jump = null;
            _rotation = null;
        }

        public void Dispose()
        {
            _subscriptions?.Dispose();
            _subscriptions = null;
        }

        void OnDestroy() => Dispose();
    }
}