using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Attacks/Normal Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController animatorOR;
    public float damage;
    public float pushForce;
    public float attackMovement;
    //public GameObject effect;
    //public float knockback;
}
