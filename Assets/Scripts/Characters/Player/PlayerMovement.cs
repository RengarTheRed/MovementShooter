using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Uses character controller over rigidbody
    private CharacterController _charController;
    //Ground checking variables
    public Transform groundCheck;
    public float groundDistance = .4f;
    //LayerMasks to use
    public LayerMask groundMask;
    public LayerMask wallMask;
    
    //Movement Vectors
    private Vector3 _move;
    private Vector3 _verticalVelocity;
    
    //Booleans set for wall / ground
    private bool _bWallRunning = false;
    private bool _bIsGrounded = false;
    
    //Movement multipliers
    public float moveSpeed = 12f;
    private float gravity = -9.81f;
    private float _jumpheight = 3f;
    

    // Get charactercontroller on start if null then print error
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        if (_charController == null)
        {
            Debug.Log("No Character Controller on Player");
        }
    }

    // Check if player is colliding with wall/floor then check movement input and apply
    void Update()
    {
        //Using this instead of Controller function since that's very buggy
        _bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        _bWallRunning = Physics.CheckSphere(transform.position, groundDistance*2f, wallMask);
        Collider[] hit = Physics.OverlapCapsule(transform.position, transform.position, groundDistance*2f, wallMask);

        //Gravity
        //Check if not grounded
        if (!_bIsGrounded)
        {
            //Wall running applies gravity at reduced rate
            if (_bWallRunning)
            {
                _verticalVelocity.y += .5f*(gravity * Time.deltaTime);
            }
            // Not grounded + Not Wall running apply gravity
            else
            {
                _verticalVelocity.y += (gravity*Time.deltaTime);
            }
        }
        else
        {
            if (_verticalVelocity.y < 0)
            {
                _verticalVelocity.y = 0;
            }
        }
        //Movement Input & Application of vertical velocity
        _move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            if (_bIsGrounded)
            {
                _verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * gravity);
            }
            if (_bWallRunning)
            {
                //_verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * gravity);
                
                //_verticalVelocity = (transform.position - hit[0].ClosestPoint(transform.position)*4f);
                Vector3 wallJump = 200*(transform.position - hit[0].ClosestPointOnBounds(transform.position));
                _move += wallJump;

            }
        }
        ApplyMovement();
    }

    //Split update into smaller functions
    private void CheckInput()
    {
        
    }
    private void Movement()
    {
        
    }

    private void ApplyMovement()
    {
        _charController.Move(_move * (moveSpeed * Time.deltaTime));
        _charController.Move(_verticalVelocity*Time.deltaTime);
    }

    //Use this for wall running etc
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }
}
