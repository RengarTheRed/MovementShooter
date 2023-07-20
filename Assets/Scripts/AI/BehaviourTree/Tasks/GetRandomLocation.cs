using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class GetRandomLocation : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        Vector3 moveLocation = context.transform.position + Random.insideUnitSphere * 64;
        moveLocation = new Vector3(moveLocation.x, context.agent.transform.position.y, moveLocation.z);
        if(context.agent.CalculatePath(moveLocation, context.agent.path))
        {
            blackboard.moveToPosition = moveLocation;
            return State.Success;
        }
        return State.Running;
    }
}
