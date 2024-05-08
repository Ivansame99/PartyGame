using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICombat
{
    PowerController powerController { get; set; }
    EnemyDirector enemyDirector { get; set; }
    public float GetPowerDamageScale();
    public float GetPoweLevel();
}
