using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10.0f; // Velocidad del proyectil.

    private void Update()
    {
        // Mueve el proyectil hacia adelante en su dirección local.
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
}