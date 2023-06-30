using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator aniDoor;
    private bool _unlocked;
    
    private void Start()
    {
        aniDoor = GetComponentInChildren<Animator>();
    }

    //When triggered open door, close on exit
    private void OnTriggerEnter(Collider other)
    {
        if (_unlocked)
        {
            PlayDoorAnimation(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayDoorAnimation(false);
    }

    private void PlayDoorAnimation(bool openDoor)
    {
        aniDoor.SetBool("OpenDoor", openDoor);
    }

    //Functions accessed by DoorManager object
    public void UnlockDoor()
    {
        _unlocked = true;
    }

    public bool GetLockedState()
    {
        return _unlocked;
    }
}
