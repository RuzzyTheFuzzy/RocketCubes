using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float maxThrowForce;
    [SerializeField] private float throwAngle;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float controllerMultiplier = 1f;
    private Player CurrentPlayer
    {
        get
        {
            return GameManager.PlayerManager.CurrentPlayer;
        }
    }

    // Run player scripts from here to avoid eatch player having an update.
    // Scripts inside player to make accesability easier
    private void Update()
    {
        if (GameManager.gameState == GameState.Game)
        {
            CurrentPlayer.SwapWeapons();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.gameState == GameState.Game)
        {
            CurrentPlayer.Jump();
            CurrentPlayer.Move();
            CurrentPlayer.ThrowGrenade(maxThrowForce, throwAngle);
        }
    }

    private void LateUpdate()
    {
        // Always run to allow camera movement before players turn
        CurrentPlayer.CameraMove();
        CurrentPlayer.CameraRotation(rotationSpeed, controllerMultiplier);
    }




}
