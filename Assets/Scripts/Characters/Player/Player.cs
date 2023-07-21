using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    private RaycastHit hit;
    public LayerMask _interactableLayer;
    private Transform _raycastOrigin;
    private int _CurrentCheckPointID = 0;

    private CheckpointManager _checkpointManager;
    private HUD _playerHUD;
    
    //HP
    public int _maxHP = 10;
    private int _currentHP;

    private void Start()
    {
        _currentHP = _maxHP;
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
        _playerHUD = FindFirstObjectByType<HUD>();
        _checkpointManager = FindFirstObjectByType<CheckpointManager>();
    }
    private void Update()
    {
        CheckInput();
        InteractionRayCast();
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadCheckPoint();
        }
    }

    //Currently inefficiently gets charcontroller each time will improve soon
    public void LoadCheckPoint()
    {
        GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = _checkpointManager.GetPlayerCheckPoint(_CurrentCheckPointID).position;
        GetComponent<CharacterController>().enabled = true;
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (hit.transform!=null)
            {
                hit.transform.GetComponent<IInteractable>().Interact(gameObject);
            }
        }
    }

    private void InteractionRayCast()
    {
        if (_playerHUD)
        {
            if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.TransformDirection(Vector3.forward), out hit, 500f, _interactableLayer))
            {
                _playerHUD.UpdateInteractText("Press E to use " + hit.transform.GetComponent<IInteractable>().InteractName);
            }
            else
            {
                _playerHUD.UpdateInteractText("");
            }
        }
    }

    //Damage Function Implementation
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0)
        {
            
        }
    }

    //Heal Function maxes at maxHP
    public void Heal(int heal)
    {
        if (_currentHP + heal <= _maxHP)
        {
            _currentHP += heal;
        }
        else
        {
            _currentHP = _maxHP;
        }
    }

    private void Death()
    {
        //Display GameOver etc
    }

    public int GetCheckPoint()
    {
        return _CurrentCheckPointID;
    }

    public void SetCheckPoint(int newCheckPointID)
    {
        _CurrentCheckPointID = newCheckPointID;
    }
}