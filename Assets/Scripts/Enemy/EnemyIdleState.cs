using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : IEnemyState
{
    private EnemyController _controller;
    private int _currentWaypointIndex;
    
    public void EnterState(EnemyController controller)
    {
        Debug.Log("Enter Idle State");
        _controller = controller;
        _currentWaypointIndex = Random.Range(0, _controller.Waypoints.Count);
    }

    public void UpdateState()
    {
        Transform currentWaypoint = _controller.Waypoints[_currentWaypointIndex];
        NavMeshAgent agent = _controller.Agent;
        float minimumDistanceToWaypoint = _controller.MinimumDistanceToWaypoint;
        List<Transform> waypoints = _controller.Waypoints;
        
        // Check if the enemy is close enough to its current destination
        // If it's close enough, then go to the next one
        if (agent.remainingDistance <= minimumDistanceToWaypoint)
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex >= waypoints.Count)
            {
                _currentWaypointIndex = 0;
            }
            currentWaypoint = waypoints[_currentWaypointIndex];
        }

        agent.isStopped = false;
        agent.destination = currentWaypoint.position;
        
        Transform controllerTransform = _controller.transform;
        float checkRadius = _controller.CheckRadius;
        float maximumDistanceForFollowing = _controller.MaximumDistanceForFollowing;
        
        RaycastHit hit;
        if (Physics.SphereCast(controllerTransform.position, checkRadius, controllerTransform.forward, out hit, maximumDistanceForFollowing))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player found!");
                _controller.ChangeState(new EnemyFollowState());
            }
        }
    }

    public void ExitState()
    {
        Debug.Log("Exit Idle State");
    }
}
