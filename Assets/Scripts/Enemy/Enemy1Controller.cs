using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] float health = 3;

    public List<Transform> players;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
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
}
