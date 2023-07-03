using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractable
{
    private bool _isUnlocked = false;
    private DoorManager _DoorManager;

    private void Start()
    {
        _DoorManager = FindFirstObjectByType<DoorManager>();
    }
    public void Interact(GameObject instigator)
    {
        _isUnlocked = true;
        _DoorManager.UnlockDoor(0);
    }
    
}
