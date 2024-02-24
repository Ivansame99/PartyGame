using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : MonoBehaviour
{
    public float torusDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<PlayerHealthController>().TakeDamage(torusDamage);
        }
    }
}
