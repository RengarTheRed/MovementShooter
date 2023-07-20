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
        int previous = current;
        base.OnStart();
        var status = base.OnUpdate();
        if (previous != current) {
            if (children[previous].state == State.Running) {
                children[previous].Abort();
            }
        }

        return status;
    }
}
