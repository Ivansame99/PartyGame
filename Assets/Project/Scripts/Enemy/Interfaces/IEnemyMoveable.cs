using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemyMoveable 
{
    NavMeshAgent agent { get; set; }
    Rigidbody rb { get; set; }
    Transform playerPos { get; set; }
    Transform playerPos2 { get; set; }
    EnemyTargetController enemyTargetController { get; set; }
    Animator animator { get; set; }
    void AgentState(bool state);
    void MoveEnemy(Vector3 position);


}
