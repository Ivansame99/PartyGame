using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLineIndicatorController : MonoBehaviour
{
	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
    {
		rb.MovePosition(transform.position + transform.forward * 5000 * Time.fixedDeltaTime);
	}
}
