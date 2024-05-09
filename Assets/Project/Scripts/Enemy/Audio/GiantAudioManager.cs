using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class GiantAudioManager : MonoBehaviour
{
	[SerializeField]
	private float minPitch = 0.8f;

	[SerializeField]
	private float maxPitch = 1.2f;

	public void PlayDamage() //que es
	{
	// FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemys/Drunk/Puke", transform.position);
	}

	public void PlayDeath()
	{
	//FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Archery/String", transform.position);
	}

	public void PlayStomp()
	{
	FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Giant/Attack_Stomp", transform.position);
	}
	}

