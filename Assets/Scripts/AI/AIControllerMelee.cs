using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIControllerMelee : AI
{
    protected override void Idle()
    {
        Debug.Log("I'm Idle");
        StartCoroutine(WaitForSeconds(3));
        _bCurrentlyInState = false;
    }

    protected override void Patrol()
    {
        Debug.Log("I'm patrolling");
        //_agent.Move((Random.insideUnitCircle * 5));
        _agent.destination = Random.insideUnitCircle * 1;
        _bCurrentlyInState = false;
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
        _state = State.Patrol;
    }
}
