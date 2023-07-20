using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject _bulletPrefab;
    public Transform _gunBarrel;
    private Transform _bulletManager;
    private float gunNoiseDistance = 25;

    private ObjectPool _bulletPool;

    // Start creates manager object and pools
    private void Start()
    {
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
        _bulletPool = new ObjectPool(_bulletPrefab, 6, _bulletManager);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        // Grabs bullet from pool
        GameObject bullet = _bulletPool.GetObjectFromPool();
        
        // Sets transform of bullet
        bullet.transform.position = _gunBarrel.position;
        bullet.transform.rotation = _gunBarrel.rotation;
        
        bullet.transform.SetParent(_bulletManager.transform);
        
        bullet.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = _gunBarrel.forward * 50f;
        MakeNoise();
    }

    private void MakeNoise()
    {
        var allEnemies = Physics.SphereCastAll(this.transform.position, gunNoiseDistance, transform.forward);

        foreach (var enemy in allEnemies)
        {
            if (enemy.transform.CompareTag("Enemy"))
            {
                enemy.transform.GetComponent<AIPerception>().HearNoise(this.transform.position);
            }
        }
    }
}
