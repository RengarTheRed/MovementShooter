using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public OpenDoor _roomDoor;

    //On Room Clear enables tied door to open
    protected void ClearRoom()
    {
        if (_roomDoor)
        {
            _roomDoor.UnlockDoor();
        }
    }
}