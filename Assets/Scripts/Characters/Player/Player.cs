using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    [Header("Interaction Variables")]
    public LayerMask interactableLayer;
    private RaycastHit _hit;
    private Transform _raycastOrigin;
    
    //Checkpoint System Setup
    private int _currentCheckPointID;
    private CheckpointManager _checkpointManager;

    //HUD
    private HUD _playerHUD;
    
    [Header("HP")]
    public int maxHp = 10;
    private int _currentHp;
    private bool _bInvincible;
    private float _gameTimer;

    //Script Refs
    private CharacterController _characterController;
    private GunScript _gunScript;
    

    //Start sets up variables
    //May Create HUD at runtime instead of grabbing from scene
    private void Start()
    {
        SetupPlayerVariables();
        // ReportPlayerToBlackboard();
    }

    private void SetupPlayerVariables()
    {
        //Once I change to make HUD at runtime I could also edit the prefab and pre-set the other variables too
        _raycastOrigin = gameObject.GetComponentInChildren<Camera>().transform;
        _playerHUD = FindFirstObjectByType<HUD>();
        _checkpointManager = FindFirstObjectByType<CheckpointManager>();
        _characterController = GetComponentInChildren<CharacterController>();

        //HP SETUP
        _currentHp = maxHp;
        int maxAmmo = GetComponentInChildren<GunScript>()._maxAmmo;
        _playerHUD.SetupHUD(maxHp, maxAmmo);

        //GRABS GUN COMP
        _gunScript = GetComponentInChildren<GunScript>();
    }
    
    // private void ReportPlayerToBlackboard()
    // {
    //     SharedGameObject selfGameObject = new SharedGameObject();
    //     selfGameObject.Value = gameObject;
    //     GlobalVariables.Instance.SetVariable("PlayerGameObject", selfGameObject);
    // }

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
        gameObject.transform.position = _checkpointManager.GetPlayerCheckPoint(_currentCheckPointID).position;
        _characterController.enabled = true;
    }
    
    //Public Functions called by the Player Input Actions, see bindings there.
    public void Interact()
    {
        if (_hit.transform != null)
        {
            _hit.transform.GetComponent<IInteractable>().Interact(gameObject);
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
            if (Physics.Raycast(_raycastOrigin.position, _raycastOrigin.TransformDirection(Vector3.forward), out _hit, 500f, interactableLayer))
            {
                _playerHUD.UpdateInteractText("Press E to use " + _hit.transform.GetComponent<IInteractable>().InteractName);
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

    private void UpdateHealthUI()
    {
        _playerHUD.UpdateHP(_currentHp);
    }


    //Damage Function Implementation
    public void TakeDamage(int damage)
    {
        if(!_bInvincible)
        {
            _currentHp -= damage;
            UpdateHealthUI();
        
            //Call Death
            if (_currentHp < 0)
            {
                Death();
            }
        }
    }

    //Heal Function maxes at maxHP
    public void Heal(int heal)
    {
        if (_currentHp + heal <= maxHp)
        {
            _currentHp += heal;
        }
        else
        {
            _currentHp = maxHp;
        }
        UpdateHealthUI();
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

    //Function that displays game over HUD etc
    private void Death()
    {
        //Display GameOver etc
    }
    
    //Functions for Updating / Getting Current Checkpoint
    public int GetCheckPoint()
    {
        return _currentCheckPointID;
    }

    public void SetCheckPoint(int newCheckPointID)
    {
        _currentCheckPointID = newCheckPointID;
    }
    IEnumerator InvincibilityTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _bInvincible = false;
    }
}