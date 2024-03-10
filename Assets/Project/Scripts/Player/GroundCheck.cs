using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	[SerializeField]
	private Transform raycastPoint;

	[SerializeField]
	private LayerMask groundLayer;

	[SerializeField]
	private LayerMask charactersLayer;

	private Rigidbody rb;

	private float raycastDistance = 0.5f;

	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Debug.DrawRay(raycastPoint.position, Vector3.down * raycastDistance, Color.red);

		if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, raycastDistance, charactersLayer))
		{
			Vector3 slideValue;
			slideValue = Random.onUnitSphere * 10f;
			slideValue.y = rb.velocity.y;
			rb.velocity = slideValue;
		}
	}

	public bool DetectGround()
	{
		Debug.DrawRay(raycastPoint.position, Vector3.down * raycastDistance, Color.red);

		//Check if it's on ground
		if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
		{
			return true;
		}

		return false;
	}
}
