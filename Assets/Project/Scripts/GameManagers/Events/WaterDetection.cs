using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
	[HideInInspector]
	public bool onWater;

	private float triggerTimeout = 0f;

	private void Update()
	{
		if (triggerTimeout > 0)
		{
			triggerTimeout -= Time.deltaTime;

			if (triggerTimeout <= 0)
			{
				triggerTimeout = 0f;
				onWater = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Water"))
		{
			onWater = true;
			triggerTimeout = 0.1f;
		}
	}

	//   private void OnTriggerExit(Collider other)
	//   {
	//	if (other.CompareTag("Water"))
	//	{
	//		onWater = false;
	//	}
	//}
}
