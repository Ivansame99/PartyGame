using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICombat
{
    PowerController powerController { get; set; }

    public float GetPowerDamage();
}
