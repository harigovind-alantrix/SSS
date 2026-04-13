using System.Collections.Generic;
using Core.Models;
using Entities.Player;
using Features.Gameplay;
using UnityEngine;
using Cinemachine;
namespace Features.Gameplay
{
    public class CloneManager : MonoBehaviour
    {
        public GameObject currentPlayer;
        public GameObject clonePrefab;

        private GameObject lastPlayer; // to remove previous one

        public void HandleClone()
        {
            if (currentPlayer == null)
            {
                Debug.Log("No current player!");
                return;
            }

            // 🔥 Remove previous old player
            RemoveOldPlayer();

            PlayerController playerController = currentPlayer.GetComponent<PlayerController>();

            if (playerController == null)
            {
                Debug.Log("PlayerController missing!");
                return;
            }

            Vector3 spawnPos = currentPlayer.transform.position;
            Quaternion spawnRot = currentPlayer.transform.rotation;


            float input = playerController.GetCurrentInput();

            Vector3 direction = Vector3.zero;

            if (Mathf.Abs(input) > 0.1f)
            {
                direction = new Vector3(input, 0, 0).normalized;
            }


            PlayerController pc = currentPlayer.GetComponent<PlayerController>();
            if (pc != null) pc.enabled = false;

            PlayerMovement movement = currentPlayer.GetComponent<PlayerMovement>();
            if (movement != null) movement.enabled = false;

            PlayerJump jump = currentPlayer.GetComponent<PlayerJump>();
            if (jump != null) jump.enabled = false;

            PlayerRotation rotation = currentPlayer.GetComponent<PlayerRotation>();
            if (rotation != null) rotation.enabled = false;

            Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.isKinematic = true; // 🔥 stops physics interaction
            }




            // 🔥 Spawn clone
            GameObject clone = Instantiate(clonePrefab, spawnPos, spawnRot);
            // 🔥 Apply boost to clone
            CloneBoost boost = clone.GetComponent<CloneBoost>();

            if (boost != null)
            {
                boost.ApplyBoost(direction);
            }
            else
            {
                Debug.Log("CloneBoost missing on prefab!");
            }

            // 🔥 Store old player for next cleanup
            lastPlayer = currentPlayer;

            // 🔥 Switch control to clone
            currentPlayer = clone;

            Debug.Log("Clone spawned with boost!");
        }

        // 🔥 Remove previous player (called before spawning next clone)
        void RemoveOldPlayer()
        {
            if (lastPlayer != null)
            {
                Destroy(lastPlayer);
                lastPlayer = null;
            }
        }
    }
}
