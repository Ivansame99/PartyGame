using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemy : MonoBehaviour
{
    public float velocidadMovimiento = 3.0f;
    public float distanciaAtaque = 2.0f;
    public List<Transform> jugadores;
    private int playerIndex = 0;
    private Animator anim;
    private Transform jugadorMasCercano;
    bool attackCooldown;
    private float cooldownTimer;
    //public GameObject[i] players;
    private void Start()
    {
       anim = GetComponent<Animator>();
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

            if (distanciaAlJugador <= distanciaAtaque && !attackCooldown)
            {//ATACANDO
                anim.SetBool("Run", false);
                anim.SetBool("Attack", true);
                attackCooldown = true;
                //Debug.Log(distanciaAlJugador);
                //Debug.Log(distanciaAtaque);
            }
            else if(attackCooldown){
                cooldownTimer += Time.deltaTime;
                
                if (cooldownTimer > 1.0)
                {
                    anim.SetBool("Attack", false);
                    
                    attackCooldown = false;
                    cooldownTimer = 0;
                }
            }
            else
            {
                // Calcula la dirección al jugador sin mirar hacia él
                Vector3 direccionAlJugador = (jugadorMasCercano.position - transform.position).normalized;
                direccionAlJugador.y = 0;

                // Calcula la rotación para mirar en la dirección correcta
                Quaternion targetRotation = Quaternion.LookRotation(direccionAlJugador);

                // Aplica la rotación sin cambiar la inclinación
                transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

                // Mueve al enemigo en la dirección correcta
                transform.Translate(Vector3.forward * velocidadMovimiento * Time.deltaTime);
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
            anim.SetBool("Run", true);
            
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                jugadorMasCercano = jugador;
            }
        }

        return jugadorMasCercano;
    }
}