using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractable
{
    private DoorManager _DoorManager;

    public string InteractName { get; set; }

    private void Start()
    {
        _DoorManager = FindFirstObjectByType<DoorManager>();
        InteractName = "Door Switch";
    }
    public void Interact(GameObject instigator)
    {
        _DoorManager.UnlockDoor(0);
        Debug.Log("Door Opened");
    }
    
}
