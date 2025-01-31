using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveCommand : Command
{
    private NavMeshAgent _robot;
    private Vector3 _destination;
    
    public MoveCommand(NavMeshAgent robot, Vector3 destination)
    {
        _robot = robot;
        _destination = destination;
    }
    
    public override void Execute()
    {
        _robot.SetDestination(_destination);   
    }

    public override void Undo()
    {
        
    }
    
    public override bool IsCompleted => IsArrived();

    private bool IsArrived()
    {
        float remainingDistance = _robot.remainingDistance;
        return remainingDistance <= 0.2f;
    }
}
