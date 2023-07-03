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
    private bool _bJumping = false;
    private bool _bIsGrounded = false;
    
    //Movement multipliers
    public float moveSpeed = 12f;
    private float gravity = -9.81f;
    private float _jumpheight = 3f;

    private Collider wallRunningObject;
    private Coroutine _endRun;
    private float endRunTimer = .3f;
    

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
        //_bWallRunning = Physics.CheckSphere(transform.position, groundDistance*2f, wallMask);
        _bJumping = false;
        
        //Movement related functions
        CheckInput();
        CheckGravity();
        ApplyMovement();
    }

    //Split update into smaller functions
    private void CheckInput()
    {
        _bJumping = Input.GetButtonDown("Jump");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        _move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
    }
    private void Jump()
    {
        if (_bIsGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * gravity);
        }
        if (_bWallRunning)
        {
            //_verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * gravity);
            
            //_verticalVelocity = (transform.position - hit[0].ClosestPoint(transform.position)*4f);
            Vector3 wallJump = 200*(transform.position - wallRunningObject.ClosestPointOnBounds(transform.position));
            _move += wallJump;
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
                //_verticalVelocity.y = 0;
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
        _charController.Move(_verticalVelocity*Time.deltaTime);

    }

    private void ApplyMovement()
    {
        _charController.Move(_move * (moveSpeed * Time.deltaTime));
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
            _bWallRunning = true;
            wallRunningObject = other;
            Debug.Log("I'm wall running!");
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
    }
}