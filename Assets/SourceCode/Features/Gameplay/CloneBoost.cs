using Data.Configs;
using UnityEngine;

namespace Features.Gameplay
{
    public class CloneBoost
    {
        private readonly Rigidbody _rb;
        private readonly PlayerConfig _config;

        public CloneBoost(Rigidbody rb, PlayerConfig config)
        {
            _rb = rb;
            _config = config;
        }

        public void ApplyBoost(Vector3 direction)
        {
            Vector3 boostVelocity;

            if (direction == Vector3.zero)
            {
                boostVelocity = Vector3.up * (_config.upwardForce * 1.6f);
            }
            else
            {
                boostVelocity = (direction.normalized * _config.boostForce) + (Vector3.up * _config.upwardForce);
            }
            _rb.velocity = Vector3.zero; 
            _rb.AddForce(boostVelocity, ForceMode.VelocityChange);
        }
    }
}