using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("FireShoot Setting")] 
    public GameObject BulletPrefeb;
    public Transform FirePosition;
    public float FireSpeed;

    [Header("Canon Setting")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform topCannon;
    [SerializeField] private Transform bottomCannon;

    [Header("Move Setting")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = 0.5f;
    [SerializeField] private float turnSpeed = 0.05f;
    [SerializeField] private float maxXRot = 10f;
    [SerializeField] private float minXRot = -10f;
    [SerializeField] private float smoothTime = 0.05f;

    Rigidbody rb;

    private PlayerAnimation PlayerAnimation;
    private CharacterController characterController;

    private Vector3 moveDir;
    private Vector3 moveVelocity;
    private float ySpeed;
    private float currentVelocity;

    Quaternion rotGoal;
    Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
  
    }

    private void Update()
    {
        //Movement Control
        MoveRot();
        MovementHanddle();
        Jump();

        //WaeponControl
        WaeponTracing();
        LimitRot();

        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            GameObject bullet = Instantiate(BulletPrefeb, FirePosition.position, FirePosition.rotation);

            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward.normalized * FireSpeed);
        }
    }

    private void MovementHanddle()
    {
        float hInput = Input.GetAxis("Horizontal");

        if (hInput > 0 || hInput < 0)
        {
            PlayerAnimation.Walking();
        }
        else
        {
            PlayerAnimation.Idle();
        }

        moveDir = new Vector3(hInput, 0, 0);

        float magnitudo = Mathf.Clamp01(moveDir.magnitude) * moveSpeed;
        moveDir.Normalize();

        moveVelocity = moveDir * magnitudo;

        ySpeed += Physics.gravity.y * Time.deltaTime;

        moveVelocity.y = ySpeed;

        
        characterController.Move(moveVelocity * Time.deltaTime);
    }

    private void MoveRot()
    {
        if(moveVelocity.x == 0) return;
        float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime); ;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Jump()
    {
        

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
        Vector3 headEulerAngles = topCannon.localRotation.eulerAngles;

        headEulerAngles.x = (headEulerAngles.x > 180) ? headEulerAngles.x - 360 : headEulerAngles.x;
        headEulerAngles.x = Mathf.Clamp(headEulerAngles.x, minXRot, maxXRot);

        topCannon.localRotation = Quaternion.Euler(headEulerAngles);
    }

    private void WaeponTracing()
    {
        //botCanon.transform.LookAt(new Vector3(target.position.x, botCanon.position.y, target.position.z));
        bottomCannon.transform.LookAt(new Vector3(target.position.x, bottomCannon.position.y, target.position.z));

        
        direction = (target.position - topCannon.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);

        topCannon.rotation = Quaternion.Slerp(topCannon.rotation, rotGoal, turnSpeed);
        
    }
}
