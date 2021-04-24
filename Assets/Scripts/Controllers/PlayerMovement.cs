using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float jumpForce = 100f;

    private Vector2 rawInputMovement = Vector2.zero;
    private bool jumpStarted = false;
    private bool isOnGround = false;

    public void OnMovement(Vector2 value)
    {
        rawInputMovement = new Vector3(value.x, value.y);
    }

    public void OnJump()
    {
        if (isOnGround)
        {
            jumpStarted = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isOnGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isOnGround = false;
        }
    }

    void FixedUpdate()
    {
        if (rawInputMovement.sqrMagnitude > Mathf.Epsilon)
        {
            var horizontalSpeed = new Vector2(playerRigidbody.velocity.x, 0);
            var speedLimitor = 1 - (horizontalSpeed.sqrMagnitude / Mathf.Pow(movementSpeed, 2));

            var movement = rawInputMovement * movementSpeed * speedLimitor * 50f;
            playerRigidbody.AddForce(movement, ForceMode2D.Force);
        }

        if (jumpStarted)
        {
            jumpStarted = false;

            var jumpVector = Vector2.up * jumpForce;
            playerRigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }
}
