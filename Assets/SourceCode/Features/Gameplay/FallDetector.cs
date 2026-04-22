using Core.Messages.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;

namespace Features.Gameplay
{
    public class FallDetector : MonoBehaviour
    {
        [SerializeField]
        private float _fallDistance = 20f;

        private IPublisher<PlayerFellEvent> _publisher;
        private float _peakY;
        private bool _triggered;

        [Inject]
        public void Construct(IPublisher<PlayerFellEvent> publisher)
        {
            _publisher = publisher;
        }

        private void Update()
        {
            if (_triggered) return;

            float currentY = transform.position.y;

            if (currentY > _peakY)
            {
                _peakY = currentY;
            }

            if (_peakY - currentY > _fallDistance)
            {
                _triggered = true;
                _publisher.Publish(new PlayerFellEvent());
            }
        }
    }
}