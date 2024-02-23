using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    [SerializeField] private float triggerDistanceClose;
    [SerializeField] private float triggerDistanceFar;
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        float distance = Vector3.Distance(_enemy.playerPos.position,transform.position);
        //Vector3 dir = _enemy.playerPos.transform.position - this.transform.position;

        if (distance < triggerDistanceClose /*|| distance > triggerDistanceFar*/)
        {
            _enemy.SetAggroStatus(true);
        }
        else
        {
            _enemy.SetAggroStatus(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            _enemy.SetAggroStatus(false);
            Debug.Log("entra");
        }
    }
}
