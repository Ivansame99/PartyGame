using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CoinCollision : MonoBehaviour
{
    [FMODUnity.EventRef] 
    public string waterEventPath = "event:/SFX/Events/Water"; //cambiar evento

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Material: Sand"))
        {
          FMODUnity.RuntimeManager.PlayOneShot(waterEventPath);  
        }
    }
}
