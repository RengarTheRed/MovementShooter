using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    public DoorManager doorManager;
    private RaycastHit hit;
    public LayerMask _interactableLayer;
    private Transform _raycastOrigin;

    private void Start()
    {
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
    }
    private void Update()
    {
        CheckInput();
        InteractionRayCast();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            
        }
    }

    private void InteractionRayCast()
    {
        if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.TransformDirection(Vector3.forward), out hit, 5000f, _interactableLayer))
        {
            Debug.Log("I hit " + hit.transform.name);
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
