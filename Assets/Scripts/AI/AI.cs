using UnityEngine;
using UnityEngine.AI;

public abstract class AI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private float _moveSpeed=10f;
    public State _state = State.Idle;
    

    private float fieldOfView = 45;
    private float sightDistance = 100;
    private Transform _player;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateSense();
        switch (_state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.SeePlayer:
                SeePlayer();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    private void UpdateSense()
    {
        
    }

    protected abstract void Idle();

    protected abstract void Patrol();

    protected abstract void SeePlayer();

    protected abstract void Attack();
}


public enum State
{
    Idle, Patrol, SeePlayer, Attack
}