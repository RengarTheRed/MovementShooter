using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControllerMelee : AI
{
    protected override void Idle()
    {
        Debug.Log("I'm Idle");
        StartCoroutine(WaitForSeconds(3f));
        _state = State.Idle;
    }

    protected override void Patrol()
    {
        Debug.Log("I'm patrolling");

    }

    protected override void SeePlayer()
    {
        Debug.Log("I see player");

    }

    protected override void Attack()
    {
        Debug.Log("I'm attacking");
    }

    IEnumerator WaitForSeconds(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
