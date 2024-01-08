using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyDirector : MonoBehaviour
{
    public int[] playerTarget;
    public List<Transform> players;
    public GameObject[] jugadoresArray;
    // Start is called before the first frame update
    void Start()
    {
        jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            players.Add(jugadorObj.transform);
        }
        playerTarget = new int[jugadoresArray.Length];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
