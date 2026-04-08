using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 8f;

    public void Move(float input)
    {
        Vector3 move = new Vector3(input, 0, 0);
        transform.position += move * MoveSpeed * Time.deltaTime;
    }
}
