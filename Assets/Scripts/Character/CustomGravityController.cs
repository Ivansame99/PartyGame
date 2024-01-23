using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravityController : MonoBehaviour
{
	[SerializeField]
	private float gravityScale;
	[SerializeField]
	private static float globalGravity = -9.81f;

	private float fallMultiplier = 1.5f;
	private Vector3 gravity;
	Rigidbody m_rb;

	[HideInInspector]
	public bool gravityOn = true;

	void OnEnable()
	{
		m_rb = GetComponent<Rigidbody>();
		m_rb.useGravity = false;
	}

	private void Start()
	{
		fallMultiplier = 2.0f;
	}
	private void Update()
	{
		gravity = globalGravity * gravityScale * Vector3.up;
	}

	void FixedUpdate()
	{
		if (gravityOn)
		{
			m_rb.AddForce(gravity, ForceMode.Acceleration);

			if (m_rb.velocity.y < -1.0f && m_rb.velocity.y > -20.0f)
			{
				m_rb.velocity += Vector3.up * gravity.y * (fallMultiplier - 1) * Time.deltaTime;
			}
		} else
		{
			//m_rb.velocity = new Vector3(m_rb.velocity.x, 0, m_rb.velocity.z);
			m_rb.AddForce(gravity/1.5f, ForceMode.Acceleration);
		}
	}
}
