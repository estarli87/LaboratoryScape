using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables
    [SerializeField] private CharacterController _characterController;
    
    [Header("--- MOVEMENT ---")]
    [Space(10)]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float speed;
    [SerializeField] private float turnSmoothTime;
    
    private float turnSmoothVelocity;

    [Header("--- POSESSION ---")] 
    [Space(10)] 
    [SerializeField] private bool canPossess;
    [SerializeField] private bool imPossessing;

    public bool CanPossess => canPossess;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        playerCamera = GameObject.Find("MainCamera").GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        Movement();

        if (canPossess)
        {
            Possess();
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void Possess()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            imPossessing = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            canPossess = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            canPossess = false;
        }
    }
}