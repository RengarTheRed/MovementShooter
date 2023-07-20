using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MoveToSound : ActionNode
{
    public float tolerance = 5.0f;

    protected override void OnStart() {
        context.agent.destination = blackboard.soundLocation;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.agent.destination = blackboard.soundLocation;
        if (context.agent.pathPending) {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            Debug.Log("cant get there");
            return State.Failure;
        }
        
        return State.Success;
    }
}
