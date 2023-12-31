using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Bullet : MonoBehaviour
{
    private int _bulletDamage = 1;
    private float _lifeSpan = 4f;
    
    //When hit character deal damage and deactivate
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<ICharacter>() != null)
        {
            other.transform.GetComponent<ICharacter>().TakeDamage(_bulletDamage);
            gameObject.SetActive(false);
        }
    }

    // When activated stay active for short duration before deactivating
    private void OnEnable()
    {
        StartCoroutine(DisableTimer());
    }

    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(_lifeSpan);
        gameObject.SetActive(false);
    }
}
