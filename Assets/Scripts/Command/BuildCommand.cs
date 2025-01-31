using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildCommand : Command
{
    private NavMeshAgent _robot;
    private Builder _builder;
    public BuildCommand(NavMeshAgent robot, Builder builder)
    {
        _robot = robot;
        _builder = builder;
    }
    
    public override void Execute()
    {
        _robot.SetDestination(_builder.transform.position);
    }

    public override void Undo()
    {
        _builder.Unbuild();
    }
    
    public override bool IsCompleted => IsBuildingCompleted();

    private bool IsBuildingCompleted()
    {
        if (_robot.remainingDistance > 0.2f)
        {
            return false;
        }

        _builder.Build();
        return true;
    } 
}
