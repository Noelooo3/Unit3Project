using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    
    // private void Start()
    // {
    //     agent.destination = player.position;
    // }

    private void Update()
    {
        agent.destination = Player.GetInstance().transform.position;
    }
}
