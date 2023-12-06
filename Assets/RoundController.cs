using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawns;

    [SerializeField]
    private GameObject enemy1Prefab;

    [SerializeField]
    private int[] enemiesInRound;

    [SerializeField]
    private float secondsBetweenRounds;

    private int roundIndex;

    private List<GameObject> currentEnemies;
    // Start is called before the first frame update
    void Start()
    {
        roundIndex = 0;
        currentEnemies = new List<GameObject>();
        Invoke("StartNextRound", secondsBetweenRounds);
    }

    // Update is called once per frame
    void Update()
    {
        if(roundIndex < enemiesInRound.Length) CheckCurrentEnemiesDeath();
    }

    void StartNextRound()
    {
        if (roundIndex < enemiesInRound.Length)
        {
            int enemyNumberInCurrentRound = enemiesInRound[roundIndex];

            for (int i = 0; i < enemyNumberInCurrentRound; i++)
            {
                int randomSpawn = Random.Range(0, spawns.Length);
                currentEnemies.Add(Instantiate(enemy1Prefab, spawns[randomSpawn].position, enemy1Prefab.transform.rotation));
            }

            roundIndex++;
        } else
        {
            Debug.Log("Se han acabado las rondas");
        }
    }

    void CheckCurrentEnemiesDeath()
    {
        if (currentEnemies.Count == 0) return;

        for(int i=0; i < currentEnemies.Count; i++)
        {
            if (currentEnemies[i] != null) return; //Si hay alguno que no sea null, para de mirar el resto
        }

        //Si todos son null
        Debug.Log("Han muerto todos");
        currentEnemies.Clear();
        Invoke("StartNextRound", secondsBetweenRounds);
        //if()
        //Debug.Log(currentEnemies[0]);
    }
}
