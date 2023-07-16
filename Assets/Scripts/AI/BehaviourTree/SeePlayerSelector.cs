using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SeePlayerSelector : InterruptSelector
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (blackboard.seePlayer)
        {
            if (current != 0)
            {
                children[current].Abort();
                current = 0;
            }
        }
        else
        {
            current = 1;
        }

        return State.Running;
    }
}
