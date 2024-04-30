using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerParticleController : MonoBehaviour
{
	[SerializeField]
	private float activeTime;

	[SerializeField]
	private float speed;

	[SerializeField] private Color color1 = Color.red;
	[SerializeField] private Color color2 = Color.white;

	private Transform target;
	private int powerAmmount;
	private float timer;
	private bool canBePicked = false;

	private void Awake()
	{
		Color randomColor = Random.value < 0.5f ? color1 : color2;

		Renderer renderer = GetComponent<Renderer>();

		if (renderer != null)
		{
			renderer.material.color = randomColor;
		}
	}

	void Update()
	{
		if (timer > activeTime)
		{
			canBePicked = true;
		} else
        {
			timer += Time.deltaTime;
		}

		if (target != null && canBePicked)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			transform.position += direction * speed * Time.deltaTime;
		}
	}

	public int GetPowerAmount()
    {
		return powerAmmount;
    }

	public bool CanBePicked()
    {
		return canBePicked;
    }

	public void SetPowerAmount(int value)
	{
		powerAmmount = value;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
		{
			target = other.transform;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
		{
			target = null;
		}
	}
}