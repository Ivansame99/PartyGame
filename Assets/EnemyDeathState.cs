using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : StateMachineBehaviour
{
    private ChaseState chaseState;
    private EnemyDirector enemyDirector;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyDirector = GameObject.Find("GameManager").GetComponent<EnemyDirector>();
        chaseState = animator.GetBehaviour<ChaseState>();

        // Llama a OnStateExit de ChaseState para actualizar la variable 'player'
        chaseState.OnStateExit(animator, stateInfo, layerIndex);

        // Ahora puedes acceder a la variable 'player' desde chaseState
        DecreasePlayerTarget(chaseState.player.gameObject.name);
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
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
