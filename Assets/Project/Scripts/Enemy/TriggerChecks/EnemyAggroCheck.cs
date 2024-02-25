using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistanceClose;
    [SerializeField] private float triggerDistanceFar;

    private Ray hit;
    private Enemy _enemy;
    private float distance;
    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        //CALCULATE DISTANCE BETWEEN ENEMY AND PLAYER
        if(_enemy.playerPos != null) distance = Vector3.Distance(_enemy.playerPos.position,transform.position);

        //NORMAL AGGRO STATUS
        if (distance < triggerDistanceClose)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }

        //SPECIAL AGGRO STATUS
        if(distance > triggerDistanceFar)
        {
            _enemy.SetSpecialAggroStatus(true);

        }
        else
        {
            _enemy.SetSpecialAggroStatus(false);
        }
    }

}
