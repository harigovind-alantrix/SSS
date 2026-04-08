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
            if (!jump.IsJumping()) return;

            currentZRotation += -moveInput * rotationSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
        }
    }

}
