using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 360f; // degrees per second

    private PlayerJump jump;

    void Awake()
    {
        jump = GetComponent<PlayerJump>();
    }

    public void Rotate(float moveInput)
    {
        // Rotate only when jumping
        if (!jump.IsJumping()) return;

        float direction = -moveInput; // flip for correct feel

        Vector3 rotation = new Vector3(0, 0, direction * rotationSpeed * Time.deltaTime);
        transform.Rotate(rotation);
    }
}
