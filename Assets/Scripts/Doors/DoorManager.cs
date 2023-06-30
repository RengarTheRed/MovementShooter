using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    //Unlock-able Doors
    public List<OpenDoor> doors;

    public void UnlockDoor(int doorIndex)
    {
        if (doorIndex < doors.Count)
        {
            doors[doorIndex].UnlockDoor();
            Debug.Log("Door Unlocked");
        }
    }
}
