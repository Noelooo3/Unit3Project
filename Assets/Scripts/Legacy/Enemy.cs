using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float maximumDistanceForFollowing;
    [SerializeField] private float maximumDistanceForAttacking;
    [SerializeField] private float minimumDistanceToWaypoint;
    [SerializeField] private float checkRadius;
    
    private int _currentWaypointIndex;
    
    private bool _isIdling;
    private bool _isFollowing;
    private bool _isAttacking;

    private void Awake()
    {
        _currentWaypointIndex = 0;
        _isIdling = true;
        _isFollowing = false;
        _isAttacking = false;
        agent.destination = waypoints[0].position;
    }

    private void Update()
    {
        if (_isIdling)
        {
            Idle();
        }
        else if (_isFollowing)
        {
            Follow();
        }
        else if (_isAttacking)
        {
            Attack();
        }
    }

    private void Idle()
    {
        Transform currentWaypoint = waypoints[_currentWaypointIndex];
        
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

        RaycastHit hit;
        if (Physics.SphereCast(this.transform.position, checkRadius, this.transform.forward, out hit, maximumDistanceForFollowing))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player found!");
                _isIdling = false;
                _isFollowing = true;
                _isAttacking = false;
            }
        }
    }

    private void Follow()
    {
        if (Player.GetInstance() == null)
        {
            _isIdling = true;
            _isFollowing = false;
            _isAttacking = false;
            return;
        }
        
        Vector3 playerPos = Player.GetInstance().transform.position;
        agent.isStopped = false;
        agent.destination = playerPos;

        float distance = Vector3.Distance(this.transform.position, playerPos);
        if (distance > maximumDistanceForFollowing)
        {
            _isIdling = true;
            _isFollowing = false;
            _isAttacking = false;
        }
        else if (distance <= maximumDistanceForAttacking)
        {
            _isIdling = false;
            _isFollowing = false;
            _isAttacking = true;
        }
    }

    private void Attack()
    {
        if (Player.GetInstance() == null)
        {
            _isIdling = true;
            _isFollowing = false;
            _isAttacking = false;
            return;
        }
        
        agent.isStopped = true;
        Debug.Log("Attack the player");
        
        float distance = Vector3.Distance(this.transform.position, Player.GetInstance().transform.position);
        if (distance > maximumDistanceForAttacking)
        {
            _isIdling = false;
            _isFollowing = true;
            _isAttacking = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * maximumDistanceForFollowing);
        Gizmos.DrawWireSphere(this.transform.position + this.transform.forward * maximumDistanceForFollowing, checkRadius);
    }
}
