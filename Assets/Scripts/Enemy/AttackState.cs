using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AttackState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player,player2;
    public List<Transform> players;
    [SerializeField] float triggerDistance = 2.5f;
    [SerializeField] float evadeAttackCooldown;
    private float secondDistance;
    private float timer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        player = FindPlayer();
        player2 = FindSecondClosestPlayer();
        //animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if(player2 != null) secondDistance = Vector3.Distance(player2.position, animator.transform.position);
        if (timer <= 0 && player2 != null && secondDistance < triggerDistance) // Verificar si player2 no es null
        {
            animator.SetBool("isAttacking", false);
            animator.SetTrigger("evading");
            timer = evadeAttackCooldown;
        }
        else if (distance > triggerDistance)
        {
            animator.SetBool("isAttacking", false);
            
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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

    private Transform FindSecondClosestPlayer()
    {
        Transform closestPlayer = FindPlayer();
        Transform secondClosestPlayer = null;
        float minDist = float.MaxValue;

        foreach (Transform player in players)
        {
            if (player != closestPlayer)
            {
                float distance = Vector3.Distance(agent.transform.position, player.position);

                if (distance < minDist)
                {
                    minDist = distance;
                    secondClosestPlayer = player;
                }
            }
        }
        return secondClosestPlayer;
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
