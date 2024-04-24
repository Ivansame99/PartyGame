using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantArmorFeedback : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("SlashEffect") || other.CompareTag("JumpAttack")))
        {
            if (other.gameObject.transform.parent.tag != "Enemy")
            {
                Debug.Log("Hit by slash effect");
                /*
                lastAttacker = other.transform.parent.gameObject;
                SlashController slashController = other.GetComponent<SlashController>();
                attackPosition = other.gameObject.transform.position;
                if (other.CompareTag("JumpAttack")) pushForce = slashController.pushForce;
                else pushForce = slashController.pushForce * 2;
                pushBack = true;

                ReceiveDamage(slashController.finalDamage);
                */
            }
        }

    }
}
