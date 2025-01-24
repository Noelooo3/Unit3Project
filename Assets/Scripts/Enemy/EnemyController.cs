using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent Agent => agent;
    public List<Transform> Waypoints => waypoints;
    public float MaximumDistanceForFollowing => maximumDistanceForFollowing;
    public float MaximumDistanceForAttacking => maximumDistanceForAttacking;
    public float MinimumDistanceToWaypoint => minimumDistanceToWaypoint;
    public float CheckRadius => checkRadius;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float maximumDistanceForFollowing;
    [SerializeField] private float maximumDistanceForAttacking;
    [SerializeField] private float minimumDistanceToWaypoint;
    [SerializeField] private float checkRadius;
    
    private IEnemyState _currentState;

    private void Start()
    {
        ChangeState(new EnemyIdleState());
    }

    private void Update()
    {
        if (_currentState == null)
        {
            return;
        }
        _currentState.UpdateState();
    }

    public void ChangeState(IEnemyState state)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }
        _currentState = state;
        _currentState.EnterState(this);
    }
}
