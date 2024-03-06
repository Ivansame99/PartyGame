using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SecuterAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistanceClose;
    [SerializeField] private float deg;

    private Enemy _enemy;
    private float distancePlayer1,distancePlayer2;
    private Vector3 direction;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //CALCULATE DISTANCE BETWEEN ENEMY AND PLAYER
        if (_enemy.playerPos != null)
        {
            distancePlayer1 = Vector3.Distance(_enemy.playerPos.position, transform.position);
            direction = _enemy.playerPos.transform.position - _enemy.transform.position;
        }
        

        //NORMAL AGGRO STATUS
        if (distancePlayer1 < triggerDistanceClose && Math.Abs(Vector3.Angle(_enemy.transform.forward, direction)) < deg)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }

        if (_enemy.playerPos2.position != null)
        {
            distancePlayer2 = Vector3.Distance(_enemy.playerPos2.position, transform.position);
            if (distancePlayer2 < triggerDistanceClose)
            {
                _enemy.SetSpecialAggroStatus(true);
            }
            else
            {
                _enemy.SetSpecialAggroStatus(false);
            }
        }
    }
}
