using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantAudioManager : MonoBehaviour
{
	[SerializeField]
	private float minPitch = 0.8f;

	[SerializeField]
	private float maxPitch = 1.2f;

	[SerializeField]
	private AudioSource damage;

	[SerializeField]
	private AudioSource death;

	[SerializeField]
	private AudioSource stomp;

	public void PlayDamage()
	{
		damage.Play();
	}

	public void PlayDeath()
	{
		death.Play();
	}

	public void PlayStomp()
	{
		stomp.Play();
	}
}
