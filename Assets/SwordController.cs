using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private GameObject target;
    private bool attack;

    [SerializeField]
    private float pushForce;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Character2")
        {
            target = collision.gameObject;
            attack = true;
        }        
    }

    private void FixedUpdate()
    {
        if (attack)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;

            direction.y = 1;
            target.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
            attack = false;
        }
    }
}
