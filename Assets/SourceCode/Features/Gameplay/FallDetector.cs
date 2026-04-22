using Core.Messages.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Features.Gameplay
{
    public class FallDetector : MonoBehaviour
    {
        [SerializeField]
        private float _fallThreshold = -20f;

        private IPublisher<PlayerFellEvent> _publisher;
        private bool _triggered;

        [Inject]
        public void Construct(IPublisher<PlayerFellEvent> publisher)
        {
            _publisher = publisher;
        }

        private void Update()
        {
            if (_triggered) return;
            if (transform.position.y < _fallThreshold)
            {
                _triggered = true;
                _publisher.Publish(new PlayerFellEvent());
            }
        }
    }
}