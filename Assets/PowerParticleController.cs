using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerParticleController : MonoBehaviour
{
	[SerializeField]
	private float activeTime;

	[SerializeField]
	private float speed;

	private Transform target;
	private int powerAmmount;
	private float timer;
	private bool canBePicked = false;

    void Update()
	{

	/*	if(this.transform.position.y < 1.5f)
        {
			this.transform.position = new Vector3(this.transform.position.x, 1.6f, this.transform.position.z);
		}
	*/
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