using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player,player2;
    [SerializeField] float triggerDistance = 2.5f;
    [SerializeField] private float deg;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float evadeAttackCooldown;
    [SerializeField] private float normalAttackCooldown;
    private float fieldOfView;
    private float timerAttack,timerSpecial;
    private EnemyDirector enemyDirector;
    private bool newTarget;
    private Transform lastTarget;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyDirector = GameObject.Find("GameManager").GetComponent<EnemyDirector>();
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = speed;
        agent.acceleration = acceleration;
        agent.angularSpeed = angularSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timerSpecial >= 0)
        {
            timerSpecial -= Time.deltaTime;
        }
        if (timerAttack >= 0)
        {
            timerAttack -= Time.deltaTime;
        }
        player = FindPlayer();
        player2 = FindSecondClosestPlayer(player);
        
        if (player != null)
        {
            if (agent.isActiveAndEnabled) agent.SetDestination(player.position);

            float distance = Vector3.Distance(player.position, animator.transform.position);
            Vector3 dir = player.transform.position - animator.transform.position;

            if (distance < triggerDistance && timerAttack <= 0 && Math.Abs(Vector3.Angle(animator.transform.forward, dir)) < deg)
            {
                animator.SetBool("isAttacking", true);
                timerAttack = normalAttackCooldown;
            }

            //animator.transform.LookAt(player);
            float distanceSpecial = Vector3.Distance(player.position, animator.transform.position);
            if (player2 != null) distanceSpecial = Vector3.Distance(player2.position, animator.transform.position);
            if (timerSpecial <= 0 && player2 != null && distanceSpecial < triggerDistance) // Verificar si player2 no es null
            {
                animator.SetTrigger("evading");
                timerSpecial = evadeAttackCooldown;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.isActiveAndEnabled) agent.SetDestination(animator.transform.position);
    }
private Transform FindPlayer()
{
    Transform searchPlayer = null;
    float minDist = float.MaxValue;

    foreach (Transform player in enemyDirector.players)
    {
        float distance = Vector3.Distance(agent.transform.position, player.position);
        
        if (distance < minDist && player.GetComponent<PlayerHealthController>().dead == false)
        {
            minDist = distance;
            searchPlayer = player;
        }
    }
        if (!newTarget)
        {
            // Disminuir el contador del objetivo anterior si ya estaba siguiendo a otro jugador
            // Incrementar el contador del nuevo objetivo
            IncreasePlayerTarget(searchPlayer.gameObject.name);

            lastTarget = searchPlayer;
            newTarget = true;
        }
        if (lastTarget != null && lastTarget != searchPlayer)
        {
            DecreasePlayerTarget(lastTarget.gameObject.name);
            newTarget = false;
        }
        return searchPlayer;
}

    private void IncreasePlayerTarget(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                enemyDirector.playerTarget[0]++;
                break;
            case "Player2":
                enemyDirector.playerTarget[1]++;
                break;
            case "Player3":
                enemyDirector.playerTarget[2]++;
                break;
            case "Player4":
                enemyDirector.playerTarget[3]++;
                break;
        }
    }

    private void DecreasePlayerTarget(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                enemyDirector.playerTarget[0]--;
                break;
            case "Player2":
                enemyDirector.playerTarget[1]--;
                break;
            case "Player3":
                enemyDirector.playerTarget[2]--;
                break;
            case "Player4":
                enemyDirector.playerTarget[3]--;
                break;
        }
    }
        private Transform FindSecondClosestPlayer(Transform closestPlayer)
        {
            Transform secondClosestPlayer = null;
            float minDist = float.MaxValue;

            foreach (Transform player in enemyDirector.players)
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
