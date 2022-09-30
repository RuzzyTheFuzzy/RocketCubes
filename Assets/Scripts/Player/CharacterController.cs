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

    private void Update()
    {
        CurrentPlayer.SwapWeapons();
    }

    private void LateUpdate()
    {
        CurrentPlayer.CameraMove();
        CurrentPlayer.CameraRotation(rotationSpeed, controllerMultiplier);
    }

    private void FixedUpdate()
    {
        CurrentPlayer.Jump();
        CurrentPlayer.Move();
        CurrentPlayer.ThrowGrenade(maxThrowForce, throwAngle);
    }



}
