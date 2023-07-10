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
            if (other.CompareTag("Player"))
            {
                _checkpointManager.CompareCheckPoint(_checkPointID);
            }
        }
    }
}