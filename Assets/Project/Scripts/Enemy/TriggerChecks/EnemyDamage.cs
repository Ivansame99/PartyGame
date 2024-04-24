using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float baseDamage;
    public float finalDamage;
    public float pushForce;

    [HideInInspector] public GameObject owner;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public float power;
    public void SetPushForce(float s)
    {
        pushForce = s;
    }

    private void Start()
    {
        owner = enemy.gameObject;
        finalDamage = baseDamage + enemy.GetPowerDamageScale(); //cambiar escalado de poder
        SetPushForce(pushForce);
    }
}
