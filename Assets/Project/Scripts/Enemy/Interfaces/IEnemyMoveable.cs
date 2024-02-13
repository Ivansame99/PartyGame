using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMoveable 
{
    NavMeshAgent agent { get; set; }
    Rigidbody rb { get; set; }
    void MoveEnemy(Vector3 destination, float speed);
    void AgentState(bool state);
   
}
