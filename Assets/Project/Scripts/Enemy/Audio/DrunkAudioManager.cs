using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DrunkAudioManager : MonoBehaviour
{
    [SerializeField]
    private float minPitch = 0.8f;

    [SerializeField]
    private float maxPitch = 1.2f;

    public void PlayDamage() 
    {
    //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemys/Drunk/Puke", transform.position);
    }

    public void PlayDeath()
    {
    // FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Archery/String", transform.position);
    }

    public void PlayProjectileLaunch()
    {
    //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Drunk/Bottle_Attack", transform.position);
    }
    public void PlayProjectileHit()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Drunk/Bottle_Attack", transform.position);
    }

    public void PlayVomitAttack()
    {
    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Enemies/Drunk/Puke", transform.position);
    }

    }