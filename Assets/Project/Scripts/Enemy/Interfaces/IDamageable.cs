using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float damageAmount);
    void Die();

    public float maxHealth { get; set; }
    float currentHealth { get; set; }
    public float inmuneTime { get; set; }
    bool isDead { get; set; }

}
