using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
	[SerializeField]
	private float minPitch = 0.8f;

	[SerializeField]
	private float maxPitch = 1.2f;

	[SerializeField]
    private AudioSource bowCharge;

	[SerializeField]
	private AudioSource bowRelease;

	[SerializeField]
	private AudioSource damage;

	[SerializeField]
	private AudioSource death;

	[SerializeField]
	private AudioSource jump;

	[SerializeField]
	private AudioSource stomp;

	[SerializeField]
	private AudioSource swordWhoosh;

	[SerializeField]
	private AudioSource roll;

	public void PlayBowCharge()
	{
		bowCharge.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		bowCharge.Play();
	}

	public void PlayBowShoot()
	{
		bowRelease.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		bowRelease.Play();
	}

	public void PlayDamage()
	{
		damage.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		damage.Play();
	}

	public void PlayDeath()
	{
		death.Play();
	}

	public void PlayJump()
	{
		jump.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		jump.Play();
	}

	public void PlayStomp()
	{
		stomp.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		stomp.Play();
	}

	public void PlaySwordWhoosh()
	{
		swordWhoosh.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		swordWhoosh.Play();
	}

	public void PlayRoll()
	{
		roll.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		roll.Play();
	}
}
