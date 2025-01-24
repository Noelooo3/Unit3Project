using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _controller;
    
    public void EnterState(EnemyController controller)
    {
        Debug.Log("Enter Attack State");
        _controller = controller;
    }

    public void UpdateState()
    {
        if (Player.GetInstance() == null)
        {
            _controller.ChangeState(new EnemyIdleState());
            return;
        }

        NavMeshAgent agent = _controller.Agent;
        Transform controllerTransform = _controller.transform;
        float maximumDistanceForAttacking = _controller.MaximumDistanceForAttacking;
        
        agent.isStopped = true;
        Debug.Log("Attack the player");
        
        float distance = Vector3.Distance(controllerTransform.position, Player.GetInstance().transform.position);
        if (distance > maximumDistanceForAttacking)
        {
            _controller.ChangeState(new EnemyFollowState());
        }
    }

    public void ExitState()
    {
        Debug.Log("Exit Attack State");
    }
}
