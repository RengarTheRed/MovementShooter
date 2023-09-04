using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground Check Variables")]
    public Transform groundCheck;
    public float groundDistance = .4f;
    //Layermask to use as ground
    public LayerMask groundMask;
    
    //Uses character controller & rigidbody for moving
    [Header("Movement Components")]
    [SerializeField] private CharacterController _charController;
    [SerializeField] private Rigidbody _rigidbody;
    
    //Movement Vectors
    private Vector3 _move;
    private Vector3 _verticalVelocity;
    private Vector3 _wallJumpVelocity;
    private Vector2 moveInput;
    private Vector3 _wallMove;

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
    public Transform _cameraTransform;

    private Collider _wallRunningObject;
    private Coroutine _endRun;
    private float _endRunTimer = .4f;
    
    //Sprinting Variables
    private Coroutine _stpsprint;
    private bool _bSprintCoroutineRunning = false;
    
    [Header("Input Action Map")]
    public InputActionAsset actions;
    private InputAction _moveAction;
    
    [SerializeField] private float[] _cameraOffsets;
    [SerializeField] private Transform[] _colliderGroups;

    // Get character-controller on start if null then print error
    void Start()
    {
        //_charController = GetComponentInChildren<CharacterController>();
        if (_charController == null)
        {
            Debug.Log("No Character Controller on Player");
        }
        
        // find the "move" action, and keep the reference to it, for use in Update
        _moveAction = actions.FindActionMap("Player").FindAction("Move");
    }

    // Check if player is colliding with wall/floor then check movement input and apply
    void Update()
    {
        //Using this instead of Controller function since that's very buggy
        _bIsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log(_bWallRunning);
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

    public void MoveInput(InputAction.CallbackContext cbContext)
    {
        if (cbContext.performed && !_bWallRunning)
        {
            moveInput = _moveAction.ReadValue<Vector2>();
        }
        if (cbContext.canceled)
        {
            moveInput = new Vector2(0, 0);
        }
    }
    public void Crouch(InputAction.CallbackContext cbContext)
    {
        if (cbContext.started)
        {
            ToggleCrouch(true);
        }

        if (cbContext.canceled)
        {
            ToggleCrouch(false);
        }
    }

    private void ToggleCrouch(bool toCrouch)
    {
        if (toCrouch)
        {
            _cameraTransform.localPosition = new Vector3(0, _cameraOffsets[1], 0);
            _colliderGroups[1].gameObject.SetActive(true);
            _colliderGroups[0].gameObject.SetActive(false);
            
        }
        else
        {
            _cameraTransform.localPosition = new Vector3(0, _cameraOffsets[0], 0);
            _colliderGroups[0].gameObject.SetActive(true);
            _colliderGroups[1].gameObject.SetActive(false);
        }
    }
    
    private Vector3 GetMovementInput()
    {
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
        if (_bWallRunning && !_bHasJumped)
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
                _verticalVelocity.y += .3f*(_gravity * Time.deltaTime);
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
        _charController.Move(_verticalVelocity * Time.deltaTime);
        _charController.Move(_wallJumpVelocity * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        if (_bWallRunning)
        {
            _charController.Move(_wallMove * (_moveSpeed* Time.deltaTime));
        }
        else if(!_bWallRunning)
        {
            if (_bSprinting)
            {
                _charController.Move(_move * (_maxSpeed * Time.deltaTime));
            }
            else
            {
                _charController.Move(_move * (_moveSpeed * Time.deltaTime));
            }
        }
    }

    //When collide with wall sets bool and collider on exit remove
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _bWallRunning = true;
            if (_endRun != null)
            {
                StopCoroutine(_endRun);
            }

            _wallJumpVelocity = new Vector3(0, 0, 0);
            _wallMove = _charController.velocity.normalized;
            _wallMove.y = 0;
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