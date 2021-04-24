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

    public void OnMovementInput(InputAction.CallbackContext value)
    {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        var rawInputMovement = new Vector2(inputMovement.x, inputMovement.y);

        OnMove?.Invoke(rawInputMovement);
    }

    public void OnJumpInput(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            OnJump?.Invoke();
        }
    }
}
