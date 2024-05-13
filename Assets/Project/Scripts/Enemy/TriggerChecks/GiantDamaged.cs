using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDamaged : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
	[SerializeField] private float damageCooldown;
	private float damageTimer;
	private string damagePath = "event:/SFX/Enemies/Giant/Damage";
    // Update is called once per frame
    private void Start()
    {
        damageTimer = damageCooldown;
    }
    void Update()
    {
        if (enemy.IsDamaged && damageTimer <= 0)
        {
			FMODUnity.RuntimeManager.PlayOneShot(damagePath);
			enemy.SetDamagedStatus(false);
            damageTimer = damageCooldown;
		}
        else damageTimer -= Time.deltaTime;
    }
}
