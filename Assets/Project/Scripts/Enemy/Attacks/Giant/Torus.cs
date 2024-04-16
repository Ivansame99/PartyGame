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
    [SerializeField] float waveSpeed;
    [SerializeField] float waveTimeLife;

    private void Start()
    {
        Destroy(gameObject, waveTimeLife);
    }
    private void Update()
    {

            this.transform.localScale += new Vector3(waveSpeed, waveSpeed, waveSpeed);
        
    }
    public void SetPushForce(float s)
    {
        pushForce = s;
    }
}
