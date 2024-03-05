using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetection : MonoBehaviour
{
	[HideInInspector]
	public bool onWater;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
			onWater = true;
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.CompareTag("Water"))
		{
			onWater = false;
		}
	}
}
