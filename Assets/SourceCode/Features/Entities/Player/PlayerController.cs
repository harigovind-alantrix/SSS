using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerRotation rotation;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        rotation = GetComponent<PlayerRotation>();
    }
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        movement.Move(moveInput);
        rotation.Rotate(moveInput);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.Jump();
        }
    }
}
