using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float bottomClamp = 1f;
    [SerializeField] private float topClamp = 1f;
    [SerializeField] private GameObject cameraFollow;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private GameObject controller;
    private Rigidbody rigidBody;
    private PlayerInputController playerInputController;
    private PlayerInput playerInput;
    private bool isCurrentDeviceMouse
    {
        get
        {
            return playerInput.currentControlScheme == "KeyboardMouse";
        }
    }

    private void Start()
    {
        playerInputController = controller.GetComponent<PlayerInputController>();
        playerInput = controller.GetComponent<PlayerInput>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CameraRotation();
        CameraMove();
    }

    private void OnEnable()
    {
        cinemachineCamera.enabled = true;
    }

    private void OnDisable()
    {
        cinemachineCamera.enabled = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CameraMove()
    {
        cameraFollow.transform.position = transform.position;
    }

    private void CameraRotation()
    {
        // if there is an input
        if (playerInputController.look.sqrMagnitude >= threshold)
        {
            //Don't multiply mouse input by Time.deltaTime
            float deltaTimeMultiplier = isCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            playerInputController.cameraTargetPitch.x += playerInputController.look.y * rotationSpeed * deltaTimeMultiplier;
            playerInputController.cameraTargetPitch.y += playerInputController.look.x * rotationSpeed * deltaTimeMultiplier;

            // clamp our pitch rotation
            playerInputController.cameraTargetPitch.x = ClampAngle(playerInputController.cameraTargetPitch.x, bottomClamp, topClamp);

            // Update Cinemachine camera target rotation
            cameraFollow.transform.rotation = Quaternion.Euler(playerInputController.cameraTargetPitch);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }



    private void Move()
    {
        if (playerInputController.move.sqrMagnitude >= threshold)
        {
            // Make Matrix with only cameras y axis
            Vector3 cameraRotation = new Vector3(0f, cameraFollow.transform.rotation.eulerAngles.y, 0f);
            Vector3 force = new Vector3(playerInputController.move.x, 0f, playerInputController.move.y);
            Matrix4x4 matrix4X4 = Matrix4x4.TRS(cameraFollow.transform.position, Quaternion.Euler(cameraRotation), cameraFollow.transform.lossyScale);
            force = matrix4X4.MultiplyVector(force);
            rigidBody.AddForce(force * speed);
        }
    }

}
