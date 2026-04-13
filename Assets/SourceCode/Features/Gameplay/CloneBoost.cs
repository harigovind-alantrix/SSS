using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;

namespace Features.Gameplay
{
    public class CloneBoost : MonoBehaviour
    {
        private Vector3 boostDirection;

        private bool isBoosting = false;

        [Header("Boost Settings")]
        public float boostForce = 10f;
        public float upwardForce = 6f;
        public float boostDuration = 0.25f;

        private PlayerMovement movement;
        private PlayerJump jump;

        void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            jump = GetComponent<PlayerJump>();
        }

        public void ApplyBoost(Vector3 direction)
        {
            boostDirection = direction; // 🔥 keep raw (important for zero check)

            StartCoroutine(BoostRoutine());
        }

        IEnumerator BoostRoutine()
        {
            isBoosting = true;

            // 🔥 Disable normal movement during boost
            if (movement != null) movement.enabled = false;
            if (jump != null) jump.enabled = false;

            float timer = 0f;

            while (timer < boostDuration)
            {
                Vector3 boostMove;

                // 🔥 FIX: If no movement → ONLY UPWARD BOOST
                if (boostDirection == Vector3.zero)
                {
                    boostMove = Vector3.up * (upwardForce * 1.6f) * Time.deltaTime;
                }
                else
                {
                    Vector3 dir = boostDirection.normalized;

                    boostMove =
                        (dir * boostForce + Vector3.up * upwardForce) * Time.deltaTime;
                }

                transform.position += boostMove;

                timer += Time.deltaTime;
                yield return null;
            }

            // 🔥 Re-enable controls
            if (movement != null) movement.enabled = true;
            if (jump != null) jump.enabled = true;

            isBoosting = false;
        }
    }
}