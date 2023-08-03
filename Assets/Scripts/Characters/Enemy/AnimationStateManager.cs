using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationStateManager : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    private NavMeshAgent _agent;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //_animator.SetFloat("Speed", _agent.velocity.magnitude/_agent.speed);
        if (_agent.velocity.magnitude / _agent.speed > .1)
        {
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            _animator.SetTrigger("Attack");
        }

        if (Input.GetKey(KeyCode.D))
        {
            _animator.SetTrigger("Death");
        }
    }
}
