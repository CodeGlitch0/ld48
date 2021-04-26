using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable] public class MovementInputEvent : UnityEvent<Vector2> { }
[Serializable] public class JumpInputEvent : UnityEvent { }

public class PlayerInputController : MonoBehaviour
{
    public MovementInputEvent OnMove;
    public JumpInputEvent OnJump;

    private bool isPlayerInputEnabled = false;

    public void EnablePlayerControls(bool active)
    {
        isPlayerInputEnabled = active;

        if (!isPlayerInputEnabled)
        {
            OnMove?.Invoke(Vector2.zero);
        }
    }

    public void OnMovementInput(InputAction.CallbackContext value)
    {
        if (!isPlayerInputEnabled)
        { 
            return;
        }

        Vector2 inputMovement = value.ReadValue<Vector2>();
        var rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);

        OnMove?.Invoke(rawInputMovement);
    }

    public void OnJumpInput(InputAction.CallbackContext value)
    {
        if (!isPlayerInputEnabled)
        { 
            return;
        }

        if (value.started)
        {
            OnJump?.Invoke();
        }
    }
}
