using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check Variables")]
    public Transform groundCheck;
    public float groundDistance = .4f;
    //LayerMasks to use
    public LayerMask groundMask;
    
    //Uses character controller over rigidbody
    private CharacterController _charController;
    private Transform _playerTransform;
    
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
    private float _moveSpeed = 6f;
    private float _maxSpeed = 12f;
    private float _gravity = -9.81f;
    private float _jumpheight = 3f;
    
    //Crouching / Sliding variables
    private float _standScale;
    private float _crouchScale;
    public Transform _gunTransform;

    private Collider _wallRunningObject;
    private Coroutine _endRun;
    private float _endRunTimer = .1f;
    
    //Sprinting Variables
    private Coroutine _stpsprint;
    private bool _bSprintCoroutineRunning = false;
    
    [Header("Input Action Map")]
    public InputActionAsset actions;
    private InputAction _moveAction;

    // Get character-controller on start if null then print error
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        if (_charController == null)
        {
            Debug.Log("No Character Controller on Player");
        }
        
        // find the "move" action, and keep the reference to it, for use in Update
        _moveAction = actions.FindActionMap("Player").FindAction("Move");
        _playerTransform = transform;
        _standScale = _playerTransform.localScale.y;
        _crouchScale = _standScale / 2;
    }

    // Check if player is colliding with wall/floor then check movement input and apply
    void Update()
    {
        //Using this instead of Controller function since that's very buggy
        _bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Movement related functions
        CheckGravity();
        _move = GetMovementInput();
        ApplyMovement();
    }

    public void Sprint(InputAction.CallbackContext cbContext)
    {
        // Implemented as a toggle for now
        // May change based on playtest
        if (cbContext.performed)
        {
            _bSprinting = !_bSprinting;
        }
    }
    
    public void Crouch(InputAction.CallbackContext cbContext)
    {
        if (cbContext.started)
        {
            Debug.Log("Started Crouch");
            ToggleCrouch(true);
        }

        if (cbContext.canceled)
        {
            Debug.Log("Stopped Crouching");
            ToggleCrouch(false);
        }
    }

    private void ToggleCrouch(bool toCrouch)
    {
        if (toCrouch)
        {
            _playerTransform.localScale = new Vector3(_playerTransform.localScale.x, _crouchScale, _playerTransform.localScale.z);

        }
        else
        {
            _playerTransform.localScale = new Vector3(_playerTransform.localScale.x, _standScale, _playerTransform.localScale.z);
        }
    }
    private Vector3 GetMovementInput()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 tmpMove = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        if (Mathf.Abs(moveInput.magnitude)<.1f)
        {
            if (!_bSprintCoroutineRunning)
            {
                _stpsprint = StartCoroutine(StopSprint(.3f));
            }
        }
        else
        {
            if (_bSprintCoroutineRunning)
            {
                StopCoroutine(_stpsprint);
                _bSprintCoroutineRunning = false;
            }
        }
        return tmpMove;
    }

    IEnumerator StopSprint(float duration)
    {
        _bSprintCoroutineRunning = true;
        yield return new WaitForSeconds(duration);
        _bSprinting = false;
        _bSprintCoroutineRunning = false;
    }

    public void Jump()
    {
        if (_bIsGrounded)
        {
            _verticalVelocity.y = Mathf.Sqrt(_jumpheight * -2f * _gravity);
        }
        if (_bWallRunning &&!_bHasJumped)
        {
            _bHasJumped = true;
            
            _wallJumpVelocity = 50*(transform.position - _wallRunningObject.ClosestPoint(transform.position));
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
                _verticalVelocity.y += .5f*(_gravity * Time.deltaTime);
            }
            // Not grounded + Not Wall running apply gravity
            else
            {
                _verticalVelocity.y += (_gravity*Time.deltaTime);
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
            _charController.Move(_move * (_maxSpeed * Time.deltaTime));
            //GetComponent<Rigidbody>().AddForce();
        }
        else
        {
            _charController.Move(_move * (_moveSpeed * Time.deltaTime));
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
            _wallRunningObject = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_wallRunningObject == other)
        {
            _endRun = StartCoroutine(EndRun(_endRunTimer));
        }
    }

    private IEnumerator EndRun(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _bWallRunning = false;
        _wallRunningObject = null;
        _bHasJumped = false;
    }
}