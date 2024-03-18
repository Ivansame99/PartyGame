using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGiant : MonoBehaviour
{
    private Enemy enemy;
    private GiantAudioManager giantAudioManager;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        giantAudioManager = enemy.giantAudioManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.IsDamaged)
        {
            giantAudioManager.PlayDamage();
            enemy.IsDamaged = false;
        }
    }
}
