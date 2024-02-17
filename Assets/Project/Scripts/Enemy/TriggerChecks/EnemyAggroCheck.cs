using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistance;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        float distance = Vector3.Distance(_enemy.playerPos.position,transform.position);
        Vector3 dir = _enemy.playerPos.transform.position - this.transform.position;

        if (distance < triggerDistance)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }
    }
}
