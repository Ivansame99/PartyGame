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

	[SerializeField]
	private LayerMask enemiesLayer;

	private Rigidbody rb;

	private float raycastDistance = 0.5f;

	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		Debug.DrawRay(raycastPoint.position, Vector3.down * raycastDistance, Color.red);

		if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, raycastDistance, charactersLayer) || Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit2, raycastDistance, enemiesLayer))
		{
			rb.AddForce(Vector3.up * 15f, ForceMode.Impulse);
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
