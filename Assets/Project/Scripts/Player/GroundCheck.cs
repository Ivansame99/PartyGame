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

	private bool ground = true;
	private float raycastDistance = 0.5f;
	private bool littleMove = false;

	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}

	public bool DetectGround()
	{
		Debug.DrawRay(raycastPoint.position, Vector3.down * raycastDistance, Color.red);

		//Check if player is on top of a character
		/*if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hitEnemy, raycastDistance, charactersLayer))
		{
			if (hitEnemy.transform.name != this.transform.name)
			{
				Debug.Log("Estas encima de alguien");
				littleMove = true;
				return false;
			}
		}
		else
		{
			littleMove = false;
		}*/

		//Check if it's on ground
		if (Physics.Raycast(raycastPoint.position, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
		{
			return true;
		}

		return false;
	}

	private void FixedUpdate()
	{
		/*if (littleMove)
		{
			rb.AddForce(transform.forward * 3, ForceMode.Impulse);
		}*/
	}
}
