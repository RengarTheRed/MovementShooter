using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator aniDoor;
    public bool _unlocked;
    
    private void Start()
    {
        aniDoor = GetComponentInChildren<Animator>();
    }

    //When triggered open door, close on exit, add type checks for this
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _unlocked)
        {
            ChangeDoorState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeDoorState(false);
        }
    }

    private void ChangeDoorState(bool openDoor)
    {
        aniDoor.SetBool("OpenDoor", openDoor);
    }

    //Functions accessed by DoorManager object
    public void UnlockDoor()
    {
        _unlocked = true;
    }
}
