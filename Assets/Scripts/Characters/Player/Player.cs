using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ICharacter
{
    [Header("Interaction Variables")]
    public LayerMask _interactableLayer;
    private RaycastHit hit;
    private Transform _raycastOrigin;
    
    //Checkpoint System Setup
    private int _CurrentCheckPointID = 0;
    private CheckpointManager _checkpointManager;

    //HUD
    private HUD _playerHUD;
    
    [Header("HP")]
    public int _maxHP = 10;
    private int _currentHP=10;
    private bool _bInvincible = false;
    private float _gameTimer=0f;

    //Script Refs
    private CharacterController _characterController;
    private GunScript _gunScript;
    

    //Start sets up variables
    //May Create HUD at runtime instead of grabbing from scene
    private void Start()
    {
        SetupPlayerVariables();
        ReportPlayerToBlackboard();
    }

    private void SetupPlayerVariables()
    {
        //Once I change to make HUD at runtime I could also edit the prefab and pre-set the other variables too
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
        _playerHUD = FindFirstObjectByType<HUD>();
        _checkpointManager = FindFirstObjectByType<CheckpointManager>();
        _characterController = GetComponentInChildren<CharacterController>();

        //HP SETUP
        _currentHP = _maxHP;
        int maxAmmo = GetComponentInChildren<GunScript>()._maxAmmo;
        _playerHUD.SetupHUD(_maxHP, maxAmmo);

        //GRABS GUN COMP
        _gunScript = GetComponentInChildren<GunScript>();
    }
    
    private void ReportPlayerToBlackboard()
    {
        SharedGameObject selfGameObject = new SharedGameObject();
        selfGameObject.Value = gameObject;
        GlobalVariables.Instance.SetVariable("PlayerGameObject", selfGameObject);
    }

    //Update Checks Input & Interaction
    private void Update()
    {
        InteractionRayCast();
        UpdateTimer();

        if (Input.GetKey(KeyCode.V))
        {
            _playerHUD.ShowVictoryScreen((int)_gameTimer);
        }
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
    
    //Public Functions called by the Player Input Actions, see bindings there.
    public void Interact()
    {
        if (hit.transform != null)
        {
            hit.transform.GetComponent<IInteractable>().Interact(gameObject);
        }
    }

    public void ThrowGun()
    {
        var gunRef = GetComponentInChildren<GunScript>();
        if (gunRef)
        {
            gunRef.Throw();
        }
    }

    public void Pause()
    {
        _playerHUD.Pause();
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
        if(!_bInvincible)
        {
            _currentHP -= damage;
            UpdateHPUI();
        
            //Call Death
            if (_currentHP < 0)
            {
                Death();
            }
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
        UpdateHPUI();
    }

    public void Invincibility()
    {
        _bInvincible = true;
        StartCoroutine(InvincibilityTimer(2));
    }

    public void InfiniteAmmo()
    {
        _gunScript.InfiniteAmmo();
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
    IEnumerator InvincibilityTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _bInvincible = false;
    }
}