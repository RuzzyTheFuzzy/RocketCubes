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
    private Player currentPlayer
    {
        get
        {
            return GameManager.instance.playerManager.currentPlayer;
        }
    }

    public float maxPower
    {
        get
        {
            return maxThrowForce;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        currentPlayer.CameraMove();
        currentPlayer.CameraRotation(rotationSpeed, controllerMultiplier);
    }

    private void FixedUpdate()
    {
        currentPlayer.Jump();
        currentPlayer.Move();
        currentPlayer.ThrowGrenade(maxThrowForce, throwAngle);
    }



}
