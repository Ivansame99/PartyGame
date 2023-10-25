using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float velocidadMovimiento = 3.0f;
    public float distanciaAtaque = 2.0f;
    public List<Transform> jugadores; 

    private Transform jugadorMasCercano;

    private void Start()
    {
       
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            jugadores.Add(jugadorObj.transform);
        }
    }

    private void Update()
    {
        jugadorMasCercano = EncontrarJugadorMasCercano();

        if (jugadorMasCercano != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, jugadorMasCercano.position);

            if (distanciaAlJugador <= distanciaAtaque)
            {//ATACANDO
                Debug.Log(distanciaAlJugador);
                Debug.Log(distanciaAtaque);
            }
            else
            {
                Debug.Log("Siguiendo a un player");
                Vector3 direccionAlJugador = (jugadorMasCercano.position - transform.position).normalized;
                direccionAlJugador.y = 0; 
                transform.Translate(direccionAlJugador * velocidadMovimiento * Time.deltaTime);
            }
        }
    }

    private Transform EncontrarJugadorMasCercano()
    {
        Transform jugadorMasCercano = null;
        float distanciaMinima = float.MaxValue;

        foreach (Transform jugador in jugadores)
        {
            float distancia = Vector3.Distance(transform.position, jugador.position);

            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                jugadorMasCercano = jugador;
            }
        }

        return jugadorMasCercano;
    }
}