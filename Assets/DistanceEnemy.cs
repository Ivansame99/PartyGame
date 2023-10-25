using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemy : MonoBehaviour
{
    public float velocidadMovimiento = 8.0f;
    public float distanciaAtaque = 10.0f;
    public float cadenciaDisparo = 5.0f; 
    public Transform proyectilPrefab; 
    public Transform puntoDisparo; 
    public List<Transform> jugadores; 

    private Transform jugadorMasCercano;
    private bool puedeDisparar = false;
    private float tiempoUltimoDisparo;

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
            {
                // Huir del jugador
                Vector3 direccionHuida = (transform.position - jugadorMasCercano.position).normalized;
                direccionHuida.y = 0; 
                direccionHuida.Normalize(); 
                transform.Translate(direccionHuida * velocidadMovimiento * Time.deltaTime);

                if (puedeDisparar)
                {
                    Disparar();
                }

            }
            else
            {
               //Dispara
                puedeDisparar = true;
                transform.LookAt(jugadorMasCercano);
               
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

    private void Disparar()
    {
        
        if (Time.time - tiempoUltimoDisparo >= cadenciaDisparo)
        {
            
            Transform proyectil = Instantiate(proyectilPrefab, puntoDisparo.position, puntoDisparo.rotation);
            proyectil.GetComponent<Rigidbody>().AddForce(puntoDisparo.forward * 500.0f);
            tiempoUltimoDisparo = Time.time;

           
            puedeDisparar = false;
        }
    }
}