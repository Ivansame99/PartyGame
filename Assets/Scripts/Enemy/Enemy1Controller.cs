using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] float health = 3;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float aggroRange = 4f;

    public List<Transform> players;
    Animator animator;
    NavMeshAgent agent;
    float timePassed;
    float newDestinationCD = 0.5f;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        player = FindPlayer();
        animator.SetFloat("speed",agent.velocity.magnitude / agent.speed);
        

        if(timePassed >= attackCD)
        {
            if(Vector3.Distance(player.transform.position,transform.position) <= attackRange)
            {
                animator.SetTrigger("attack");
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if(newDestinationCD <= 0) //&& Vector3.Distance(player.transform.position,transform.position) < aggroRange)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;
        //ROTATIONS
        // Calcula la dirección al jugador sin mirar hacia él
        Vector3 direccionAlJugador = (player.position - transform.position).normalized;
        direccionAlJugador.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direccionAlJugador);

        // Aplica la rotación sin cambiar la inclinación
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");
        if(health < 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SlashEffect"))
        {
            TakeDamage(2);
        }
    }
    private Transform FindPlayer()
    {
        Transform jugadorMasCercano = null;
        float distanciaMinima = float.MaxValue;

        foreach (Transform player in players)
        {
            float distancia = Vector3.Distance(transform.position, player.position);

            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                jugadorMasCercano = player;
            }
        }

        return jugadorMasCercano;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
