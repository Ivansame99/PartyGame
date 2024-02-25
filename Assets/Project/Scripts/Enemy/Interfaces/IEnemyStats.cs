using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ICombat
{
    PowerController powerController { get; set; }
    SlashController slashController { get; set; }

    public float GetPowerDamage();
}
