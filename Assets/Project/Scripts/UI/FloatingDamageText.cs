using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Vector3 randomizePos;

    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.position += offset;
        transform.position += new Vector3(Random.Range(-randomizePos.x, randomizePos.x), Random.Range(-randomizePos.y, randomizePos.y), Random.Range(-randomizePos.y, randomizePos.y));
    }
}
