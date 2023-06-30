using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    public DoorManager doorManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            doorManager.UnlockDoor(0);
            Debug.Log("Door Unlocked");
        }
    }

    public void TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float heal)
    {
        throw new System.NotImplementedException();
    }
}
