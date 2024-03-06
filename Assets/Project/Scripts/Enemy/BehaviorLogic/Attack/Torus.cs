using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torus : MonoBehaviour
{
    public float baseDamage;
    public float finalDamage;

    public float pushForce;

    [Header("Wave Attack parameters")]
    [SerializeField] float waveSpeed = 0.05f;
    [SerializeField] float waveTimeLife = 1f;

    [HideInInspector]
    public GameObject owner;

    private void Start()
    {
        Destroy(gameObject,waveTimeLife);
    }
    private void Update()
    {
        transform.localScale += new Vector3(waveSpeed, 0, waveSpeed);
    }
    public void SetPushForce(float s)
    {
        pushForce = s;
    }
}
