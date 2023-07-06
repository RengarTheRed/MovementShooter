using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _bulletDamage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<ICharacter>() != null)
        {
            other.transform.GetComponent<ICharacter>().TakeDamage(_bulletDamage);
        }
    }
}
