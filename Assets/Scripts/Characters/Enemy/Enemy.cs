using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, ICharacter
{
    public int _maxHP=3;
    private int _currentHP;
    private bool _bCanHitPlayer = true;
    public Animator _animator;

    private void Start()
    {
        _currentHP = _maxHP;
    }

    private void Death()
    {
        Debug.Log("I " + gameObject.name + (" have died."));
        Destroy(gameObject);
    }
    
    // Functions must be defined as inherit from ICharacter
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0)
        {
            Death();
        }
    }

    public void Heal(int heal)
    {
    }

    public void Invincibility()
    {
    }

    public void InfiniteAmmo()
    {
    }
    
    //Melee Enemy Attack Logic
    private void OnTriggerEnter(Collider other)
    {
        // 3 conditions, must have player tag, invincibility frames not active & must be in attack state
        if (other.CompareTag("Player") && _bCanHitPlayer && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            StartCoroutine(AttackCoolDown(2));
            other.GetComponentInParent<ICharacter>().TakeDamage(1);
        }
    }

    //Placeholder alternative for Invincibility Frames
    IEnumerator AttackCoolDown(float duration)
    {
        _bCanHitPlayer = false;
        yield return new WaitForSeconds(duration);
        _bCanHitPlayer = true;
    }
}
