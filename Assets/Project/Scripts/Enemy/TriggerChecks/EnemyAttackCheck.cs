using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player") && other != null)
        {
            _enemy.SetImpactStatus(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player") && other != null)
        {
            _enemy.SetImpactStatus(false);
        }
    }
}
