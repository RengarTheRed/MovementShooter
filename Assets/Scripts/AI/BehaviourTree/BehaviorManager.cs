using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    public Transform[] patrolWaypoints;
    public Transform player;
    public float attackRange = 2f;
    public float movementSpeed = 5f; // Movement speed of the enemy

    private Node behaviorTreeRoot;
    private Transform enemyTransform;

    private void Start()
    {
        enemyTransform = transform;

        var patrolAction = new PatrolAction(patrolWaypoints, enemyTransform, movementSpeed);
        var attackAction = new AttackAction(player, attackRange, enemyTransform);

        var rootSelector = new Selector(new List<Node>
        {
            patrolAction,
            attackAction
        });

        behaviorTreeRoot = rootSelector;
    }

    private void Update()
    {
        behaviorTreeRoot.Evaluate();
    }
}
