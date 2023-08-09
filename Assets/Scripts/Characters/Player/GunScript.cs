using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Mono.Cecil.Cil;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GunScript : MonoBehaviour
{
    //Gun Core Variables
    public GameObject bulletPrefab;
    public Transform gunBarrel;
    private Transform _bulletManager;
    private float _gunNoiseDistance = 25;
    private ObjectPool _bulletPool;
    
    //Ammo System
    public int _maxAmmo=10;
    private int _currentAmmo;
    private float _rechargeDelay=3;
    private float _rechargeTimer;

    //Player ref
    private Player _player;
    private bool isHeld = true;

    //Attach info
    private AttachData _attachData;
    private Rigidbody _rigidbody;

    //Infinite Ammo set by Player Script
    private bool _bInfiniteAmmo = false;

    // Start creates manager object and pools
    private void Start()
    {
        //Get Player Script
        _player = GetComponentInParent<Player>();
        _rigidbody = GetComponent<Rigidbody>();
        
        // Checks if a bullet manager exists and if not creates one
        if(GameObject.FindWithTag("BulletManager"))
        {
            _bulletManager = GameObject.FindWithTag("BulletManager").transform;
        }
        else
        {
            _bulletManager = new GameObject("BulletManager").transform;
            _bulletManager.tag = "BulletManager";

        }
        
        //Populates pool
        _bulletPool = new ObjectPool(bulletPrefab, 6, _bulletManager);

        _currentAmmo = _maxAmmo;
        StoreAttachData();
    }

    private void StoreAttachData()
    {
        _attachData = new AttachData(this.transform);
    }

    public void FireEvent(InputAction.CallbackContext ctx)
    {
        if (isHeld)
        {
            if (ctx.performed)
            {
                if (_currentAmmo > 0)
                {
                    ShootProjectile();
                }
                else
                {
                    FailShoot();
                }
            }
        }
    }

    private void CheckAmmo()
    {
        if (_currentAmmo < _maxAmmo)
        {
            _rechargeTimer -= Time.deltaTime;
            if (_rechargeTimer <= 0)
            {
                _currentAmmo++;
                _rechargeTimer = 1;
                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        _player.UpdateAmmoUI(_currentAmmo);
    }

    private void ShootProjectile()
    {
        // Grabs bullet from pool
        GameObject bullet = _bulletPool.GetObjectFromPool();
        
        // Sets transform of bullet active and positioning
        bullet.transform.position = gunBarrel.position;
        bullet.transform.rotation = gunBarrel.rotation;
        bullet.transform.SetParent(_bulletManager.transform);
        bullet.SetActive(true);
        
        //Applies force to bullet rigidbody
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = gunBarrel.forward * 50f;
        
        //Reports noise event to AI and starts recharge timer
        MakeNoise();

        //If Infinite Ammo active end function here
        if(_bInfiniteAmmo)
        {
            return;
        }

        //Updates Ammo Counts
        //If all ammo gone then recharge timer is longer
        _currentAmmo--;

        if (_currentAmmo <= 0)
        {
            _rechargeTimer = _rechargeDelay;
        }
        else
        {
            _rechargeTimer = _rechargeDelay / 2;
        }
        UpdateUI();
    }

    //Plays a bad animation / particle or something
    private void FailShoot()
    {
        
    }

    //Reports gunshot to the NPCs in radius
    private void MakeNoise()
    {
        var allEnemies = Physics.SphereCastAll(this.transform.position, _gunNoiseDistance, transform.forward);

        foreach (var enemy in allEnemies)
        {
            if (enemy.transform.CompareTag("Enemy"))
            {
                enemy.transform.GetComponent<AIPerception>().HearNoise(this.transform.position);
            }
        }
    }

    //Throw event called by player script
    public void Throw()
    {
        isHeld = false;
        transform.parent = null;
        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;
        Vector3 _toApply = transform.forward;
        _rigidbody.AddForce(_toApply*20f, ForceMode.Impulse);
        StartCoroutine(WaitForThrow(2f));
    }

    //Called when hit enemy / after 2 seconds of thrown
    private void ResetGun()
    {
        //Additional check here to prevent editor warning as this will get called twice once by trigger, secondly by coroutine if enemy hit
        if (!_rigidbody.isKinematic)
        {
            //Clears velocity
            _rigidbody.velocity = new Vector3(0, 0, 0);
            _rigidbody.angularVelocity = new Vector3(0, 0, 0);
        }
        
        //Disables Physics
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        
        //Reset Transform
        this.transform.parent = _attachData.GetParent();
        this.transform.localPosition = _attachData.GetPosition();
        this.transform.localRotation = _attachData.GetRotation();
        isHeld = true;
    }

    //Reset gun timer
    IEnumerator WaitForThrow(float waitDuration)
    {
        yield return new WaitForSeconds(waitDuration);
        ResetGun();
    }

    //Purely for colliding with enemy once thrown
    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld&other.CompareTag("Enemy"))
        {
            _currentAmmo = _maxAmmo;
            UpdateUI();
            ResetGun();
        }
    }

    public void InfiniteAmmo()
    {
        //Maxes ammo then sets infinite ammo
        _currentAmmo = _maxAmmo;
        UpdateUI();

        _bInfiniteAmmo = true;
        StartCoroutine(InfiniteAmmoTimer(3));
    }

    IEnumerator InfiniteAmmoTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        _bInfiniteAmmo = false;
    }
}

// Decided to make this a class for flexibility in being able to apply to more than just gun
class AttachData
{
    private Transform parent;
    private Vector3 position;
    private Quaternion rotation;

    public AttachData(Transform obj)
    {
        parent = obj.transform.parent;
        position = obj.transform.localPosition;
        rotation = obj.transform.localRotation;
    }

    //Get Functions
    public Transform GetParent()
    {
        return parent;
    }
    public Vector3 GetPosition()
    {
        return position;
    }
    public Quaternion GetRotation()
    {
        return rotation;
    }
}
