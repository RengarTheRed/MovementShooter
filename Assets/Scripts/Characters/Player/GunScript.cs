using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
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

    private Player _player;

    // Start creates manager object and pools
    private void Start()
    {
        //Get Player Script
        _player = GetComponentInParent<Player>();
        
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
    }
    
    private void Update()
    {
        CheckInput();
        CheckAmmo();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (_currentAmmo>0)
            {
                ShootProjectile();
            }
            else
            {
                FailShoot();
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
        
        //Updates Ammo Counts
        //If all ammo gone then recharge timer is longer
        if (_currentAmmo <= 0)
        {
            _rechargeTimer = _rechargeDelay;
        }
        else
        {
            _rechargeTimer = _rechargeDelay / 2;
        }
        _currentAmmo--;
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
}
