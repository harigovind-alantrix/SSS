using System.Collections;
using System.Collections.Generic;
using Data.Configs;
using UnityEngine;
namespace Entities.Player
{
    public class PlayerMovement
    {
        private readonly Transform _playerTransform;
        private readonly PlayerConfig _config;

        public PlayerMovement(
            Transform playerTransform,
            PlayerConfig config)
        {
            _playerTransform = playerTransform;
            _config = config;
        }

        public void Move(float input)
        {
            Vector3 move = new Vector3(input, 0, 0);
            _playerTransform.position += move * _config.moveSpeed * Time.deltaTime;
        }
    }
}

