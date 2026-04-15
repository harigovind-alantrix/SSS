using System.Collections;
using System.Collections.Generic;
using Data.Configs;
using Entities.Player;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerRotation
    {
        private readonly Transform _transform;
        private readonly PlayerConfig _config;
        private readonly PlayerJump _jump;
        
        private float currentZRotation = 0f;

        public PlayerRotation(
            Transform transform,
            PlayerConfig config,
            PlayerJump jump)
        {
            _transform = transform;
            _config    = config;
            _jump      = jump;
        }


        public void Rotate(float moveInput)
        {
            if (_jump.IsJumping())
            {
                currentZRotation += -moveInput * _config.rotationSpeed * Time.deltaTime;
            }
            else
            {
                // 🔥 LANDING CORRECTION (SMART + FAST)
                float targetRotation = Mathf.Round(currentZRotation / 90f) * 90f;

                float angleDifference = Mathf.Abs(currentZRotation - targetRotation);

                // 🔥 Adaptive speed (big error = fast correction)
                float dynamicSnapSpeed = Mathf.Lerp(20f, 50f, angleDifference / 45f);

                currentZRotation = Mathf.Lerp(
                    currentZRotation,
                    targetRotation,
                    Time.deltaTime * dynamicSnapSpeed
                );

                // 🔥 Instant snap when very close (invisible)
                if (angleDifference < 0.5f)
                {
                    currentZRotation = targetRotation;
                }
            }

            // 🔥 APPLY ROTATION
            _transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
        }
    }
}