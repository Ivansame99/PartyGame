using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
    [SerializeField]
    private PlayerWalkState walkState;

	[SerializeField]
	private float speedOnWater;

	private float originalSpeed;

    private void Awake()
    {
        originalSpeed = walkState.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            walkState.speed = speedOnWater;
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.CompareTag("Water"))
		{
			walkState.speed = originalSpeed;
		}
	}
}
