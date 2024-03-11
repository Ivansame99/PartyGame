using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    private Enemy enemy;

    public float baseDamage;
    public float finalDamage;

    public float pushForce;

    [HideInInspector]
    public GameObject owner;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();

        finalDamage = baseDamage + enemy.GetPowerDamage(); //cambiar escalado de poder
        SetPushForce(pushForce);
        owner = enemy.gameObject;
    }

    public void SetPushForce(float s)
    {
        pushForce = s;
    }
}
