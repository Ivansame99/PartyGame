using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPotionEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject potionPrefab;

    [SerializeField]
    private float maxTimeToSpawn;

    [SerializeField]
    private float minTimeToSpawn;

    private float randomTimeToSpawn;

    private float xPosMax = 18f;
    private float xPosMin = -19f;
    private float zPosMax = 19f;
    private float zPosMin = -9f;
    private float yPos = 50f;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer==0) randomTimeToSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);

        timer += Time.deltaTime;

        if (timer >= randomTimeToSpawn)
        {
            float randomXPos = Random.Range(xPosMin, xPosMax);
            float randomZPos = Random.Range(zPosMin, zPosMax);

            Instantiate(potionPrefab, new Vector3(randomXPos, yPos, randomZPos), potionPrefab.transform.rotation);
            timer = 0;
        }
    }
}
