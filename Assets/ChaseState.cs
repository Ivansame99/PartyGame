using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public List<Transform> players;
    [SerializeField] float triggerDistance = 2.5f;

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
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if(distance < triggerDistance)
        {
            animator.SetBool("isAttacking",true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
    private Transform FindPlayer()
    {
        Transform jugadorMasCercano = null;
        float distanciaMinima = float.MaxValue;

        foreach (Transform player in players)
        {
            float distancia = Vector3.Distance(agent.transform.position, player.position);

            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                jugadorMasCercano = player;
            }
        }

        return jugadorMasCercano;
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
