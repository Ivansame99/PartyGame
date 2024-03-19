using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkAudioManager : MonoBehaviour
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
    private AudioSource projectileLaunch;

    [SerializeField]
    private AudioSource projectileHit;

    public void PlayDamage()
    {
        damage.Play();
        damage.pitch = Random.Range(1.5f, 1.8f);
    }
    public void PlayProjectileHit() { projectileHit.Play(); Random.Range(1.2f, 1.5f); }
    public void PlayProjectileLaunch() { projectileLaunch.Play(); Random.Range(1.5f, 1.8f); }
    public void PlayDeath()
    {
        death.pitch = Random.Range(minPitch, maxPitch);
        death.Play();
    }
}
