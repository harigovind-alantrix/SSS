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
        [SerializeField] private Rigidbody _rigidbody;
        
        private PlayerMovement _movement;
        private PlayerJump _jump;
        private PlayerRotation _rotation;
        private GroundChecker  _groundChecker; 

        private IDisposable _subscriptions;
        private float _lastMoveAxis;
        private bool _jumpRequested;
        private bool _frozen = false;

        [Inject]
        public void Construct(PlayerConfig config,
            ISubscriber<MoveInputEvent> moveSubscriber,
            ISubscriber<JumpInputEvent> jumpSubscriber,
            IPublisher<CameraZoomEvent> zoomPublisher)
        {
            _movement = new PlayerMovement(_rigidbody, config);
            _jump = new PlayerJump(_rigidbody, config, _groundChecker, zoomPublisher);
            _rotation = new PlayerRotation(transform, config, _jump);

            var d = DisposableBag.CreateBuilder();
            moveSubscriber.Subscribe(OnMove).AddTo(d);
            jumpSubscriber.Subscribe(OnJump).AddTo(d);
            _subscriptions = d.Build();
        }

        void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
            
            _rigidbody.useGravity = false; 
        }
        
        void Update()
        {
            if(_frozen) return;
            _rotation?.Rotate(_lastMoveAxis);
        }

        void FixedUpdate() 
        {
            if (_frozen) return;

            _movement?.Move(_lastMoveAxis);
            _jump?.Tick();

            if (_jumpRequested)
            {
                _jump?.Jump();
                _jumpRequested = false;
            }
        }

        private void OnMove(MoveInputEvent evt) => _lastMoveAxis = evt.Axis;
        
        private void OnJump(JumpInputEvent _) => _jumpRequested = true;

        public void Freeze()
        {
            _frozen = true;
            Dispose();
            _movement = null;
            _jump = null;
            _rotation = null;
            
            _rigidbody.velocity        = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.isKinematic     = true;
        }

        public void Dispose()
        {
            _subscriptions?.Dispose();
            _subscriptions = null;
        }

        void OnDestroy() => Dispose();
    }
}