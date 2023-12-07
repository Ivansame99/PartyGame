using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player,player2;
    public List<Transform> players;
    [SerializeField] float triggerDistance = 2.5f;
    private float evadeAttackCooldown;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        agent.speed = 3.0f;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindPlayer();
        //animator.transform.LookAt(player);
        agent.SetDestination(player.position);
        

        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance < triggerDistance)
        {
            animator.SetBool("isAttacking",true);
        }
        //if (distance < triggerDistance && secondDistance < triggerDistance)
        //{
        //Debug.Log("esquiva ahora");
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
