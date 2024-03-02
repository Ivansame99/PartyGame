using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuterAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistanceClose;

    private Enemy _enemy;
    private float distance;
    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //CALCULATE DISTANCE BETWEEN ENEMY AND PLAYER
        if (_enemy.playerPos != null) distance = Vector3.Distance(_enemy.playerPos.position, transform.position);

        //NORMAL AGGRO STATUS
        if (distance < triggerDistanceClose)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }
    }
}
