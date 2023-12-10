using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EvadeState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public List<Transform> players;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angularSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isEvading", true);
        agent = animator.GetComponent<NavMeshAgent>();
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindPlayer();
        agent.SetDestination(player.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isEvading", false);
        agent.SetDestination(animator.transform.position);
    }
    private Transform FindPlayer()
    {
        Transform searchPlayer = null;
        float minDist = float.MaxValue;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(agent.transform.position, player.position);

            if (distance < minDist)
            {
                minDist = distance;
                searchPlayer = player;
            }
        }
        return searchPlayer;
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
