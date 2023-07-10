using System;
using UnityEngine;

namespace CheckpointSystem
{
    public class CheckPoint : MonoBehaviour
    {
        public int _checkPointID;
        private CheckpointManager _checkpointManager;
        
        private void Start()
        {
            _checkpointManager = FindFirstObjectByType<CheckpointManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.CompareTag("Player"))
            {
                //Debug.Log("Player walked on point");
                _checkpointManager.CompareCheckPoint(_checkPointID);
                gameObject.SetActive(false);
            }
        }
    }
}