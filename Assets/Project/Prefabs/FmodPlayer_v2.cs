using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FmodPlayer_v2 : MonoBehaviour
{
	private float distance = 0.1f;
	private float Material;
	private float raycastDistance = 0.5f;

	[SerializeField]
	private Transform raycastPoint;

	[SerializeField]
	private LayerMask groundLayer;

	void PlayMeleeEvent(string path)
	{
		FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
	}

	void FixedUpdate()
	{
		MaterialCheck();
		Debug.DrawRay(transform.position, Vector2.down * distance, Color.blue);
	}

	void MaterialCheck()
	{
		if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, raycastDistance))
		{
			if (hit.collider)
			{
				if (hit.collider.tag == "Material: Sand")
				{
					Material = 1f;
				}
				else if (hit.collider.tag == "Material: Stone")
				{
					Material = 2f;
				}
				else
					Material = 1f;
			}
		}

		//Debug.Log(hit.collider);
		//      if (hit.collider)
		//      {
		//          if (hit.collider.tag == "Material: Sand")
		//              Material = 1f;
		//          else if (hit.collider.tag == "Material: Stone")
		//              Material = 2f;
		//          else
		//              Material = 1f;
		//      }
	}

	void PlayFootstepsEvent(string path)
	{
		FMOD.Studio.EventInstance Footsteps = FMODUnity.RuntimeManager.CreateInstance(path);
		Footsteps.setParameterByName("Material", Material);
		Footsteps.start();
		Footsteps.release();
	}
}