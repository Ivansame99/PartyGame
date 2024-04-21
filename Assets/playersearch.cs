using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class playersearch : MonoBehaviour
{
    public string playerTag = "Player";
    public string enemyTag = "Enemy";
    public float range = 1f; 
    public float speed = 5f;
    private List<Transform> targets = new List<Transform>();

    void Update()
    {
        SearchTargets();

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= range)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    void SearchTargets()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag(playerTag);
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);

        targets.Clear();

        foreach (GameObject playerObject in playerObjects)
        {
            targets.Add(playerObject.transform);
          //  Debug.Log("Reconoce player");
            Debug.Log(targets.Count);
        }

        foreach (GameObject enemyObject in enemyObjects)
        {
            targets.Add(enemyObject.transform);
            //Debug.Log("Reconoce enemigo");
            Debug.Log(targets.Count);
        }
    }

}