using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    private RaycastHit hit;
    public LayerMask _interactableLayer;
    private Transform _raycastOrigin;

    private HUD _playerHUD;

    private void Start()
    {
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
        _playerHUD = FindFirstObjectByType<HUD>();
    }
    private void Update()
    {
        CheckInput();
        InteractionRayCast();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (hit.transform!=null)
            {
                Debug.Log(hit.transform.name);
                hit.transform.GetComponent<IInteractable>().Interact(gameObject);
            }
        }
    }

    private void InteractionRayCast()
    {
        if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.TransformDirection(Vector3.forward), out hit, 5000f, _interactableLayer))
        {
            _playerHUD.UpdateInteractText("Press E to use " + hit.transform.GetComponent<IInteractable>().InteractName);
        }
        else
        {
            _playerHUD.UpdateInteractText("");
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player took ouchies!");
    }

    public void Heal(float heal)
    {
        Debug.Log("Player has healed");
    }
}
