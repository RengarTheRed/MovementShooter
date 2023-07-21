using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void TakeDamage(int damage);
    public void Heal(int heal);
}
