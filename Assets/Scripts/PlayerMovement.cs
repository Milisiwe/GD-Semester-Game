using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap actionMap;
    private InputAction movement;

    [SerializeField]
    private Camera camera;
    private NavMeshAgent player;
    [SerializeField]
    [Range(0, 0.99f)]
    private float Smooth = 0.25f;
    [SerializeField]
    private float targetLerpSpeed = 1f;

    private Vector3 TargetDirection;
    private float LerpTime = 0;
    private Vector3 lastDirection;
    private Vector3 moveVector;

    AudioSource audioManager;
    private bool isMoving = false;

    private void Awake()
    {
        player = GetComponent<NavMeshAgent>();
        actionMap = inputActions.FindActionMap("Player");
        movement = actionMap.FindAction("Move");
        movement.started += HandleMovementAction;
        movement.canceled += HandleMovementAction;
        movement.performed += HandleMovementAction;
        movement.Enable();
        actionMap.Enable();
        inputActions.Enable();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
    }

    private void HandleMovementAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveVector = new Vector3(input.x, 0, input.y);

    }

    // Update is called once per frame
    void Update()
    {
        moveVector.Normalize();
        if (moveVector != lastDirection)
        {
            LerpTime = 0;
        }

        lastDirection = moveVector;
        TargetDirection = Vector3.Lerp(TargetDirection, moveVector, Mathf.Clamp01(LerpTime * targetLerpSpeed * (1 - Smooth)));
        Debug.Log(TargetDirection);
        player.Move(TargetDirection * player.speed * Time.deltaTime);
        isMoving = true;
        if (isMoving)
        {
            audioManager.Play();
        }
        

        
        Vector3 lookDirection = moveVector;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection), Mathf.Clamp01(LerpTime * targetLerpSpeed * (1 - Smooth)));
        }

        LerpTime += Time.deltaTime;

    }

    private void LateUpdate()
    {
        camera.transform.position = transform.position + Vector3.up * 40 + Vector3.back * 30;

    }
}
