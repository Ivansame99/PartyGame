using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
	private Vector2 moveUniversal;
	private Vector3 direction;

	[SerializeField]
	private float turnSmooth;

	[SerializeField]
	private float speed;

	private float turnSmoothTime;

	private Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		direction = new Vector3(this.moveUniversal.x, 0f, this.moveUniversal.y).normalized;
		float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
		if (!float.IsNaN(angle))
		{
			var rotation = Quaternion.Euler(0f, angle, 0f);
			this.transform.rotation = rotation;
		}
	}

	void FixedUpdate()
	{
		Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

		if (currentVelocity.magnitude > speed)
		{
			currentVelocity = currentVelocity.normalized * speed;
		}

		Vector3 targetVelocity = direction * speed;
		Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
		rb.AddForce(force, ForceMode.Acceleration);
	}

	public void SetInputVector(Vector2 direction)
	{
		moveUniversal = direction;
	}
}
