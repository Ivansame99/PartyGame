using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SecutorAudioManager : MonoBehaviour
{
	[SerializeField]
	private float minPitch = 0.8f;

	[SerializeField]
	private float maxPitch = 1.2f;

	public void PlayDamage()
	{
	// FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemys/Drunk/Puke", transform.position);
	}

	public void PlayDeath()
	{
	//FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Archery/String", transform.position);
	}

	public void PlaySwordWhoosh()
	{
	FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Sword/Whoosh", transform.position);
	}

	public void PlaySpin()
	{
	FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Secutor/Attack_Spin", transform.position);
	}
}

