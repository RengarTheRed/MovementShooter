using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Node
{
    public abstract NodeState Evaluate();
}

public enum NodeState
{
    Running,
    Success,
    Failure
}

public class Selector : Node
{
    private List<Node> childNodes;

    public Selector(List<Node> nodes)
    {
        childNodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in childNodes)
        {
            var result = node.Evaluate();
            if (result != NodeState.Failure)
                return result;
        }

        return NodeState.Failure;
    }
}

public class Sequence : Node
{
    private List<Node> childNodes;

    public Sequence(List<Node> nodes)
    {
        childNodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in childNodes)
        {
            var result = node.Evaluate();
            if (result != NodeState.Success)
                return result;
        }

        return NodeState.Success;
    }
}

public class PatrolAction : Node
{
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private Transform transform; // Enemy transform
    private float movementSpeed;

    public PatrolAction(Transform[] patrolWaypoints, Transform enemyTransform, float speed)
    {
        waypoints = patrolWaypoints;
        transform = enemyTransform;
        movementSpeed = speed;
    }

    public override NodeState Evaluate()
    {
        var targetWaypoint = waypoints[currentWaypointIndex];

        // Move towards the target waypoint
        MoveTowards(targetWaypoint.position);

        // Check if the enemy has reached the target waypoint
        if (HasReached(targetWaypoint.position))
        {
            // Switch to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Implement movement logic here using the enemy's transform
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        transform.gameObject.GetComponent<NavMeshAgent>().SetDestination(targetPosition);
    }

    private bool HasReached(Vector3 targetPosition)
    {
        // Implement check for reaching the target position using the enemy's transform
        return Vector3.Distance(transform.position, targetPosition) < 0.1f;
    }
}

public class AttackAction : Node
{
    private Transform player;
    private float attackRange;
    private Transform transform; // Enemy transform

    public AttackAction(Transform playerTransform, float range, Transform enemyTransform)
    {
        player = playerTransform;
        attackRange = range;
        transform = enemyTransform;
    }

    public override NodeState Evaluate()
    {
        // Check if the player is within attack range
        if (IsPlayerInRange())
        {
            AttackPlayer();
            return NodeState.Success;
        }

        return NodeState.Failure;
    }

    private bool IsPlayerInRange()
    {
        // Implement check for player within attack range using the enemy's transform
        return Vector3.Distance(transform.position, player.position) < attackRange;
    }

    private void AttackPlayer()
    {
        // Implement attack logic here
    }
}

