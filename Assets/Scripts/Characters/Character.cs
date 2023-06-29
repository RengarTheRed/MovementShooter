using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{
    public abstract void TakeDamage(float damage);
    public abstract void Heal(float heal);
}
