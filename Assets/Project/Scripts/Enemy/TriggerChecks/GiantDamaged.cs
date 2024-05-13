using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDamaged : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
	private string damagePath = "event:/SFX/Enemies/Giant/Damage";
	// Update is called once per frame
	void Update()
    {
        if (enemy.IsDamaged)
        {
			FMODUnity.RuntimeManager.PlayOneShot(damagePath);
			enemy.SetDamagedStatus(false);
		}
    }
}
