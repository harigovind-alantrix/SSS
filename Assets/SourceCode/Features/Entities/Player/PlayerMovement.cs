using UnityEngine;
using Data.Configs;

namespace Entities.Player
{
    public class PlayerMovement
    {
        private readonly Rigidbody _rb;
        private readonly PlayerConfig _config;

        public PlayerMovement(Rigidbody rb, PlayerConfig config)
        {
            _rb = rb;
            _config = config;
        }

        public void Move(float input)
        {
            _rb.velocity = new Vector3(input * _config.moveSpeed, _rb.velocity.y, _rb.velocity.z);
        }
    }
}