using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : MonoBehaviour
{
    public float baseDamage;
    public float finalDamage;

    public float pushForce;

    [HideInInspector]
    public GameObject owner;

    public void SetPushForce(float s)
    {
        pushForce = s;
    }
}
