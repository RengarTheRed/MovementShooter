using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    //Interaction Setup
    private RaycastHit hit;
    public LayerMask _interactableLayer;
    private Transform _raycastOrigin;
    
    //Checkpoint System Setup
    private int _CurrentCheckPointID = 0;
    private CheckpointManager _checkpointManager;

    //HUD
    private HUD _playerHUD;
    
    //HP
    public int _maxHP = 10;
    private int _currentHP;

    private CharacterController _characterController;

    private float _gameTimer=0f;

    //Start sets up variables
    //May Create HUD at runtime instead of grabbing from scene
    private void Start()
    {
        //Once I change to make HUD at runtime I could also edit the prefab and pre-set the other variables too
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
        _playerHUD = FindFirstObjectByType<HUD>();
        _checkpointManager = FindFirstObjectByType<CheckpointManager>();
        _characterController = GetComponent<CharacterController>();

        _currentHP = _maxHP;
        int maxAmmo = GetComponentInChildren<GunScript>()._maxAmmo;
        _playerHUD.SetupHUD(_maxHP, maxAmmo);
    }
    
    //Update Checks Input & Interaction
    private void FixedUpdate()
    {
        CheckInput();
        InteractionRayCast();
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _gameTimer += Time.deltaTime;
        _playerHUD.UpdateTimer(_gameTimer);
    }

    //Disables character controller as can't set position with it enabled then 'teleports' and re-enables
    public void LoadCheckPoint()
    {
        _characterController.enabled = false;
        gameObject.transform.position = _checkpointManager.GetPlayerCheckPoint(_CurrentCheckPointID).position;
        _characterController.enabled = true;
    }

    //Input Checking
    private void CheckInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (hit.transform!=null)
            {
                hit.transform.GetComponent<IInteractable>().Interact(gameObject);
            }
        }
        //Hard coded reset button, not sure if keeping in final build
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadCheckPoint();
        }
    }

    //Raycasts from raycast origin and checks for "Interactable" layer, if so pop-up on UI
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
    
    //UI Functions, Update Ammo & Update HP
    public void UpdateAmmoUI(int newCount)
    {
        _playerHUD.UpdateAmmo(newCount);
    }

    private void UpdateHPUI()
    {
        _playerHUD.UpdateHP(_currentHP);
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

    //Function that displays gameover HUD etc
    private void Death()
    {
        //Display GameOver etc
    }
    
    //Functions for Updating / Getting Current Checkpoint
    public int GetCheckPoint()
    {
        return _CurrentCheckPointID;
    }

    public void SetCheckPoint(int newCheckPointID)
    {
        _CurrentCheckPointID = newCheckPointID;
    }
}