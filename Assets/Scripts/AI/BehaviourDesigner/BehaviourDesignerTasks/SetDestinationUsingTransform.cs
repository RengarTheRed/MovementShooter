﻿using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Sets the destination of the agent in world-space units using a Target's Transform. Returns Success if the destination is valid.")]
    public class SetDestinationUsingTransform: Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [SharedRequired]
        [Tooltip("The NavMeshAgent destination")]
        public SharedTransform destinationTransform;

        // cache the navmeshagent component
        private NavMeshAgent navMeshAgent;
        private GameObject prevGameObject;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
                prevGameObject = currentGameObject;
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null) {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }
            
            if(navMeshAgent.SetDestination(destinationTransform.Value.position))
            {
                Debug.Log("Set Destination to " + destinationTransform.Value.position);
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        private void Result(bool pass)
        {
            
        }

        public override void OnReset()
        {
            targetGameObject = null;
            destinationTransform = null;
        }
    }
}