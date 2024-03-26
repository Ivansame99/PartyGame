using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEvent : MonoBehaviour
{
    [SerializeField] 
    private float damageMultiplier;

    public float GetDamageMultipler()
    {
        return damageMultiplier;
    }
}
