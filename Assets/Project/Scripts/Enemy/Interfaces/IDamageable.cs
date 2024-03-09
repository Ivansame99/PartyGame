using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void SetDamagedStatus(bool isDamaged);

    public float maxHealth { get; set; }
    float currentHealth { get; set; }
    public float inmuneTime { get; set; }
    bool isDead { get; set; }
    bool IsDamaged { get; set; }
}
