using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

// Script purely for managing a pool of objects. Designed to be flexible albeit current use case is just gun projectiles
// Script was initially created by chatGPT but reworked by myself for my use case
// https://chat.openai.com/share/750e75a0-f7bc-4f4f-bb3e-1341713915d1
public class ObjectPool
{
    private GameObject _prefabToPool;
    private Transform _poolParent;
    private List<GameObject> _objectPool;
    
    // Class constructor sets up pool and creates initial objects
    public ObjectPool(GameObject prefabToPool, int initialPoolSize, Transform parentObject)
    {
        _prefabToPool = prefabToPool;
        _poolParent = parentObject;
        _objectPool = new List<GameObject>(initialPoolSize);

        if (_poolParent.childCount>0)
        {
            foreach (Transform child in _poolParent)
            {
                _objectPool.Add(child.gameObject);
            }
        }
        else
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewObject();
            }
        }
    }

    public GameObject GetObjectFromPool()
    {
        // Gets first inactive object, if none creates new
        GameObject obj = _objectPool.Find(o => !o.activeSelf);

        if (obj == null)
        {
            obj = CreateNewObject();
        }

        return obj;
    }
    
    // Function that creates objects and places in pool
    private GameObject CreateNewObject()
    {
        //Instantiates new object
        GameObject newObj = Object.Instantiate(_prefabToPool, _poolParent);
        
        //Deactivates
        newObj.SetActive(false);
        
        //Adds to pool
        _objectPool.Add(newObj);

        //Returns object
        return newObj;
    }
}
