using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistanceAttack;
    [SerializeField] private float triggerCloseAttack;
    [SerializeField] private float deg;

    private Enemy _enemy;
    private float distancePlayer1, distancePlayer2;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy.playerPos != null)
        {
            distancePlayer1 = Vector3.Distance(_enemy.playerPos.position, transform.position);
            direction = _enemy.playerPos.transform.position - _enemy.transform.position;
        }
        if (_enemy.playerPos2 != null)
        {
            distancePlayer2 = Vector3.Distance(_enemy.playerPos2.position, transform.position);
        }

        if(distancePlayer1 < triggerCloseAttack)
        {
            _enemy.SetSpecialAggroStatus(true);
        }
        else
        {
            _enemy.SetSpecialAggroStatus(false);
        }
        if (distancePlayer1 < triggerDistanceAttack && distancePlayer1 > triggerCloseAttack &&Math.Abs(Vector3.Angle(_enemy.transform.forward, direction)) < deg)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }



    }
}
