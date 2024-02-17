using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckeable
{
    bool IsAggreed { get; set; }
    bool IsWithStrikingDistance { get; set; }
    void SetAggroStatus(bool isAggreed);
    void SetStrikingDistanceBool(bool isWithStrikingDistance);
}
