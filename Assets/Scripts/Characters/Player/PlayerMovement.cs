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
    
    //Movement Vectors
    private Vector3 _move;
    private Vector3 _verticalVelocity;
    private Vector3 _wallJumpVelocity;
    
    //Booleans set for wall / ground / sprinting
    private bool _bWallRunning = false;
    private bool _bHasJumped = false;
    private bool _bIsGrounded = false;
    private bool _bSprinting = false;
    
    //Movement multipliers
    private float moveSpeed = 6f;
    private float maxSpeed = 12f;
    private float gravity = -9.81f;
    private float _jumpheight = 3f;

    private Collider wallRunningObject;
    private Coroutine _endRun;
    private float endRunTimer = .1f;
    
    // Get character-controller on start if null then print error
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

        //Movement related functions
        CheckInput();
        CheckGravity();
        ApplyMovement();
    }

    //Split update into smaller functions
    private void CheckInput()
    {
        if (_bIsGrounded)
        {
            _bSprinting = Input.GetButton("Sprint");
            _move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        }
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (_bIsGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * gravity);
        }
        if (_bWallRunning &&!_bHasJumped)
        {
            _bHasJumped = true;
            
            _wallJumpVelocity = 50*(transform.position - wallRunningObject.ClosestPoint(transform.position));
            _wallJumpVelocity.y = 1;
        }
    }

    private void CheckGravity()
    {
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
            _wallJumpVelocity = new Vector3(0, 0, 0);
            if (_verticalVelocity.y < 0)
            {
                _verticalVelocity.y = 0;
            }
        }
        _charController.Move(_verticalVelocity*Time.deltaTime);
        _charController.Move(_wallJumpVelocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        if (_bSprinting)
        {
            _charController.Move(_move * (maxSpeed * Time.deltaTime));
        }
        else
        {
            _charController.Move(_move * (moveSpeed * Time.deltaTime));
        }
    }

    //When collide with wall sets bool and collider on exit remove
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (_endRun != null)
            {
                StopCoroutine(_endRun);
            }

            _move = _charController.velocity.normalized;
            _wallJumpVelocity = new Vector3(0, 0, 0);
            _bWallRunning = true;
            wallRunningObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (wallRunningObject == other)
        {
            _endRun = StartCoroutine(EndRun(endRunTimer));
        }
    }

    private IEnumerator EndRun(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _bWallRunning = false;
        wallRunningObject = null;
        _bHasJumped = false;
    }
}