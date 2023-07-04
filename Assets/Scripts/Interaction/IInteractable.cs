using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    string InteractName { get; set; }

    public void Interact(GameObject instigator)
    {
        
    }
}
