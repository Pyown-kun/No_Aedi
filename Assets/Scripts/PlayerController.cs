using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform headBone;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 20f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = 0.5f;

    [SerializeField] private float turnSpeed = 0.05f;

    [SerializeField] private float maxXRot = 10f;
    [SerializeField] private float minXRot = -10f;

    private CharacterController characterController;
    private Vector3 moveDir;
    private Vector3 moveVelocity;
    private float ySpeed;

    Quaternion rotGoal;
    Vector3 direction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovementHanddle();
        Jump();
        WaeponTracing();
        LimitRot();
    }

    private void MovementHanddle()
    {
        float hInput = Input.GetAxis("Horizontal");

        moveDir = new Vector3(hInput, 0, 0);

        float magnitudo = Mathf.Clamp01(moveDir.magnitude) * moveSpeed;
        moveDir.Normalize();

        moveVelocity = moveDir * magnitudo;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        moveVelocity.y = ySpeed;

        characterController.Move(moveVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        

        Debug.Log(ySpeed);

        if (characterController.isGrounded)
        {
            ySpeed = 0;

            if (Input.GetKeyDown(KeyCode.W))
            {
                ySpeed = jumpForce;
            }
        }
    }

    private void LimitRot()
    {
        Vector3 headEulerAngles = headBone.localRotation.eulerAngles;

        headEulerAngles.x = (headEulerAngles.x > 180) ? headEulerAngles.x - 360 : headEulerAngles.x;
        headEulerAngles.x = Mathf.Clamp(headEulerAngles.x, minXRot, maxXRot);

        headBone.localRotation = Quaternion.Euler(headEulerAngles);
    }

    private void WaeponTracing()
    {
        //headBone.transform.LookAt(new Vector3(target.position.x, target.position.y, transform.position.z));
        Vector3 headTransform = new Vector3(target.position.x, target.position.y, transform.position.z);

        direction = (target.position - headBone.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        headBone.rotation = Quaternion.Slerp(headBone.rotation, rotGoal, turnSpeed);
    }
}
