using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float velocity = 3.0f;
    public float AtackDistance = 2.0f;
    public List<Transform> players; 

    private Transform NearestPlayer;

    private void Start()
    {
       
      //  GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
      //  foreach (GameObject jugadorObj in jugadoresArray)
    //    {
      //      players.Add(jugadorObj.transform);
      //  }
    }

    private void Update()
    {
        NearestPlayer = FindNearestPlayer();

        if (NearestPlayer != null)
        {
            float distancePlayer = Vector3.Distance(transform.position, NearestPlayer.position);

            if (distancePlayer <= AtackDistance)
            {//ATACANDO
                Debug.Log(distancePlayer);
                Debug.Log(AtackDistance);
            }
            else
            {
                Debug.Log("Siguiendo a un player");
                Vector3 DirectionToPLayer = (NearestPlayer.position - transform.position).normalized;
                DirectionToPLayer.y = 0; 
                transform.Translate(DirectionToPLayer * velocity * Time.deltaTime);
            }
        }
    }

    private Transform FindNearestPlayer()
    {
        Transform NearestPlayer2 = null;
        float MinumunDistance = float.MaxValue;

        foreach (Transform player in players)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance < MinumunDistance)
            {
                MinumunDistance = distance;
                NearestPlayer2 = player;
            }
        }

        return NearestPlayer2;
    }
}