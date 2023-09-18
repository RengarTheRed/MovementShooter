using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Gets remaining distance from navmeshagent and sets to float")]
    public class GetRandomLocation : Action
    {
        [Tooltip("GameObject to get NavAgent From")]
        [RequiredField]
        public SharedGameObject gObject;
        
        [Tooltip("Float to store the remaining distance")]
        [RequiredField]
        public SharedFloat toStore;

        public override TaskStatus OnUpdate()
        {
            if (gObject == null)
            {
                return TaskStatus.Failure;
            }

            NavMeshAgent agent = gObject.Value.GetComponent<NavMeshAgent>();

            toStore = agent.remainingDistance;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            gObject = null;
            toStore = null;
        }
    }
}