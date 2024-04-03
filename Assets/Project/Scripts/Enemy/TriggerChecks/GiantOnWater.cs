using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantOnWater : MonoBehaviour
{
    private Enemy enemy;

    private float triggerTimeout = 0f;

    private RaycastHit hit;

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

        if(Physics.Raycast(transform.position, -transform.up, out hit, 0.5f))
        {
            enemy.SetWaterStatus(true);
            triggerTimeout = 0.1f;
        }
    }
}
