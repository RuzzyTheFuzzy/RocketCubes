using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool swap;
    public Vector2 cameraTargetPitch;
    public bool grenade;
    public bool pause;
    public bool randomKill;

    public bool isCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "Keyboard&Mouse";
        }
    }
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    private void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    private void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void OnSwap(InputValue value)
    {
        SwapInput(value.isPressed);
    }

    private void SwapInput(bool newSwapState)
    {
        swap = newSwapState;
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    private void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void OnGrenade(InputValue value)
    {
        GrenadeInput(value.isPressed);
    }

    private void GrenadeInput(bool newGrenadeState)
    {
        grenade = newGrenadeState;
    }

    private void OnPause(InputValue value)
    {
        PauseInput(value.isPressed);
    }

    private void PauseInput(bool newPauseState)
    {
        pause = newPauseState;
    }

    private void OnRandomKill(InputValue value)
    {
        RandomKillInput(value.isPressed);
    }

    private void RandomKillInput(bool newKillState)
    {
        randomKill = newKillState;
    }

}
