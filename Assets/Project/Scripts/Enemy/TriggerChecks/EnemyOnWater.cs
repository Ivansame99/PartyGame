using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyOnWater : MonoBehaviour
{
    private Enemy enemy;

    private float triggerTimeout = 0f;

    private void Awake()
    {
       enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (triggerTimeout > 0)
        {
            triggerTimeout -= Time.deltaTime;

            if (triggerTimeout <= 0)
            {
                triggerTimeout = 0f;
                enemy.SetWaterStatus(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            enemy.SetWaterStatus(true);
            triggerTimeout = 0.1f;
        }
    }
}
