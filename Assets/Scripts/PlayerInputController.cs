using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool swap;
    public Vector2 cameraTargetPitch;
    public bool grenade;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void OnSwap(InputValue value)
    {
        SwapInput(value.isPressed);
    }

    public void SwapInput(bool newSwapState)
    {
        swap = newSwapState;
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void OnGrenade(InputValue value)
    {
        GrenadeInput(value.isPressed);
    }

    public void GrenadeInput(bool newGrenadeState)
    {
        grenade = newGrenadeState;
    }

}
