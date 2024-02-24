using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    SlashController slashController;
    PowerController powerController;

    private float damage;
    private float pushForce;
    // Start is called before the first frame update
    void Start()
    {
        powerController = GetComponent<PowerController>();
    }


    public void Slash()
    {
        slashController.finalDamage = damage + powerController.GetCurrentPowerLevel() / 5; //Cambiar escalado poder
        slashController.pushForce = pushForce;
    }
}