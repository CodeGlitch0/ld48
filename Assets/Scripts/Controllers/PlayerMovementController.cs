using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody2D playerRigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float jumpForce = 100f;

    private Vector2 rawInputMovement = Vector2.zero;
    private bool jumpStarted = false;
    private bool isOnGround = false;

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started && isOnGround)
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
