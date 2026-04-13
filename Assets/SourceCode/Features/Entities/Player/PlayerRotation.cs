using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        public float rotationSpeed = 360f;

        private PlayerJump jump;
        private float currentZRotation = 0f;

        void Awake()
        {
            jump = GetComponent<PlayerJump>();
        }

        public void Rotate(float moveInput)
        {
            // 🔥 AIR ROTATION (UNCHANGED - KEEP YOUR FEEL)
            if (jump.IsJumping())
            {
                currentZRotation += -moveInput * rotationSpeed * Time.deltaTime;
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
            transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
        }
    }

}
