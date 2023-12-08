using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlashController : MonoBehaviour
{
    public float finalDamage;
    public float pushForce;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") Debug.Log("Enemigo");
    }
}
