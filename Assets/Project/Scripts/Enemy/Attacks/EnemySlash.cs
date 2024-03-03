using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    private SlashController slashController;
    private Enemy _enemy;
    [SerializeField] private float damage;
    [SerializeField] private float pushForce;

    // Start is called before the first frame update
    void Start()
    {
        slashController = GetComponent<SlashController>();
        _enemy = GetComponentInParent<Enemy>();
        Slash();
    }

    public void Slash()
    {
        slashController.finalDamage = damage + _enemy.GetPowerDamage();
        slashController.pushForce = pushForce;
        Debug.Log(slashController.finalDamage);
    }
}
