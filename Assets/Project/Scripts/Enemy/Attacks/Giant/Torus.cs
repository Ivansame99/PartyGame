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

    [Header("Wave Attack parameters")]
    public float waveSpeed;
    public float waveTimeLife;

    public void SetPushForce(float s)
    {
        pushForce = s;
    }

    private void Update()
    {
        transform.localScale += new Vector3(waveSpeed, waveSpeed, waveSpeed);   
    }
}
