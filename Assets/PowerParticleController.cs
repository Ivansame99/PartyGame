using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerParticleController : MonoBehaviour
{
	public float powerAmmount;

	[SerializeField]
	private float speed;

	private Transform target;

	void Update()
	{
		if (target != null)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			transform.position += direction * speed * Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
		{
			target = other.transform;
		}
	}
}