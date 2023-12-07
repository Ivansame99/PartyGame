using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvadeState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Vector3 oppositeDirection;
    private Vector3 targetPosition;
    [SerializeField] private float speed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        oppositeDirection = -animator.transform.forward;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        

        // Obtener la posición objetivo restando la dirección opuesta multiplicada por una distancia
        //targetPosition = animator.transform.position + oppositeDirection * speed * Time.deltaTime; // Cambia 5.0f por la distancia deseada

        // Establecer la posición objetivo usando SetDestination del NavMeshAgent
        //agent.SetDestination(targetPosition);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isEvading", false);

    }
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
//rb = animator.GetComponent<Rigidbody>();
//Vector3 oppositeDirection = -animator.transform.forward;
//Vector3 randomOffset = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)).normalized * 60f;
//Vector3 finalDirection = oppositeDirection + randomOffset;
//rb.AddForce(finalDirection, ForceMode.Impulse);

//rb.velocity = Vector3.zero;
//rb.angularVelocity = Vector3.zero;