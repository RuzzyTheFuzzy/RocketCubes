using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float maxThrowForce;
    [SerializeField] private Vector3 throwAngle = Vector3.forward;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float controllerMultiplier = 1f;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private WeaponManager weapons;
    [SerializeField] private PauseMenu pauseMenu;
    private Player currentPlayer
    {
        get
        {
            return playerManager.currentPlayer;
        }
    }
    public WeaponManager weaponManager
    {
        get
        {
            return weapons;
        }
    }

    private void Update()
    {
        currentPlayer.CameraMove();
    }

    private void LateUpdate()
    {
        if (!pauseMenu.paused)
            currentPlayer.CameraRotation(rotationSpeed, controllerMultiplier);
    }

    private void FixedUpdate()
    {
        currentPlayer.Jump();
        currentPlayer.Move();
        currentPlayer.ThrowGrenade(maxThrowForce, throwAngle);
    }



}
