using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class playersearch : MonoBehaviour
{
    public string tagJugador = "Player"; // Tag del jugador
    public float rango = 1f; // Rango en el que el objeto seguirá al jugador
    public float velocidad = 5f; // Velocidad a la que el objeto se moverá hacia el jugador

    private Transform jugador; // Referencia al transform del jugador

    void Start()
    {
        // Busca el jugador al inicio del juego
        BuscarJugador();
    }

    void Update()
    {
        if (jugador != null)
        {
            // Calcula la distancia entre este objeto y el jugador
            float distancia = Vector3.Distance(transform.position, jugador.position);

            // Si el jugador está dentro del rango, mueve este objeto hacia el jugador
            if (distancia <= rango)
            {
                Vector3 direccion = (jugador.position - transform.position).normalized;
                transform.position += direccion * velocidad * Time.deltaTime;
            }
        }
        else
        {
            // Si el jugador no se ha encontrado, busca de nuevo
            BuscarJugador();
        }
    }

    void BuscarJugador()
    {
        // Encuentra el objeto del jugador por su tag
        GameObject jugadorObject = GameObject.FindGameObjectWithTag(tagJugador);
       
        // Si se encuentra el jugador, actualiza la referencia al transform del jugador
        if (jugadorObject != null)
        {
            Debug.LogWarning("se ha encontrado jugador");
            jugador = jugadorObject.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag " + tagJugador);
        }
    }
}