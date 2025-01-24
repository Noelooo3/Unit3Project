using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowState : IEnemyState
{
    private EnemyController _controller;
    
    public void EnterState(EnemyController controller)
    {
        Debug.Log("Enter Follow State");
        _controller = controller;
    }

    public void UpdateState()
    {
        if (Player.GetInstance() == null)
        {
            _controller.ChangeState(new EnemyIdleState());
            return;
        }
        Vector3 playerPos = Player.GetInstance().transform.position;
        NavMeshAgent agent = _controller.Agent;
        Transform controllerTransform = _controller.transform;
        float maximumDistanceForFollowing = _controller.MaximumDistanceForFollowing;
        float maximumDistanceForAttacking = _controller.MaximumDistanceForAttacking;
        
        agent.isStopped = false;
        agent.destination = playerPos;

        float distance = Vector3.Distance(controllerTransform.position, playerPos);
        if (distance > maximumDistanceForFollowing)
        {
            _controller.ChangeState(new EnemyIdleState());
        }
        else if (distance <= maximumDistanceForAttacking)
        {
            _controller.ChangeState(new EnemyAttackState());
        }
    }

    public void ExitState()
    {
        Debug.Log("Exit Follow State");
    }
}
