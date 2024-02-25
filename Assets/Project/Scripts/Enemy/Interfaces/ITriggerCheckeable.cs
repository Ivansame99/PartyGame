using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckeable
{
    bool IsAggreed { get; set; }
    bool IsSpecialAggro { get; set; }
    bool IsGeneralBoolean { get; set; }
    void SetAggroStatus(bool isAggreed);
    void SetSpecialAggroStatus(bool isImpact);
    void SetGeneralBooleanStatus(bool isGeneralBoolean);
}
