using VContainer.Unity;
using MessagePipe;
using Core.Messages.Gameplay;
using UnityEngine;

namespace Infrastructure.Services
{
    public class InputService :ITickable
    {
        private readonly IPublisher<MoveInputEvent> _movePublisher;
        private readonly IPublisher<JumpInputEvent> _jumpPublisher;
        private readonly IPublisher<CloneInputEvent> _clonePublisher;
        
        private float _currentAxis;
 
        public float CurrentAxis => _currentAxis;

        public InputService(
            IPublisher<MoveInputEvent> movePublisher,
            IPublisher<JumpInputEvent> jumpPublisher,
            IPublisher<CloneInputEvent> clonePublisher)
        {
            _movePublisher = movePublisher;
            _jumpPublisher = jumpPublisher;
            _clonePublisher = clonePublisher;
        }
        
        public void Tick()
        {
            _currentAxis = Input.GetAxis("Horizontal");
 
            _movePublisher.Publish(new MoveInputEvent(_currentAxis));
 
            if (Input.GetKeyDown(KeyCode.Space))
                _jumpPublisher.Publish(new JumpInputEvent());
 
            if (Input.GetKeyDown(KeyCode.C))
                _clonePublisher.Publish(new CloneInputEvent(_currentAxis));
        }
    }
}