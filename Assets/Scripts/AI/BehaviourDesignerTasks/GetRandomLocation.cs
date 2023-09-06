using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
    [TaskCategory("Unity/Vector3")]
    [TaskDescription("Gets random location near transform.")]
    public class GetRandomLocation : Action
    {
        [Tooltip("The stored result")]
        [RequiredField]
        public SharedVector3 storeResult;
        [Tooltip("Transform to use as origin")]
        [RequiredField]
        public GameObject origin;

        private NavMeshHit hit;

        public override TaskStatus OnUpdate()
        {
            Vector3 newLocation = origin.transform.position + (Random.insideUnitSphere * 15);
            if (NavMesh.SamplePosition(newLocation, out hit, 1.0f, NavMesh.AllAreas))
            {
                storeResult.Value = newLocation;
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        public override void OnReset()
        {
            storeResult = Vector3.zero;
        }
    }
}