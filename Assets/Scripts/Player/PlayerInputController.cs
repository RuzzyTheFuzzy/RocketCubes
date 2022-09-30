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
    public bool weaponSwap;

    public bool isCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "Keyboard&Mouse";
        }
    }
    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnSwap(InputValue value)
    {
        swap = value.isPressed;
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnGrenade(InputValue value)
    {
        grenade = value.isPressed;
    }

    private void OnPause(InputValue value)
    {
        pause = value.isPressed;
    }

    private void OnRandomKill(InputValue value)
    {
        randomKill = value.isPressed;
    }
    private void OnWeaponSwap(InputValue value)
    {
        weaponSwap = value.isPressed;
    }

}
