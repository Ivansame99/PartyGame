using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEvent : MonoBehaviour
{
    public float damage;
    public float pushForce;
    public Vector3 originalPosition;
    [SerializeField] private GameObject destroyParticle;

    private void Start()
    {
        originalPosition = transform.position;    
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(destroyParticle, this.transform.position, destroyParticle.transform.rotation);
        Destroy(this.gameObject);
    }
}
