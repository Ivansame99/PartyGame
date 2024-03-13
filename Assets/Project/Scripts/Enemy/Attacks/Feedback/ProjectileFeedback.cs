using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFeedback : MonoBehaviour
{
    [SerializeField] float scaleAmmount = 1.5f;
    [SerializeField] float maxScale;
    [SerializeField] float speedScale;

    private void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    private void OnEnable()
    {
        scaleAmmount = 0;
    }

    private void Update()
    {
        if (scaleAmmount <= maxScale)
        {
            this.transform.localScale = new Vector3(scaleAmmount, scaleAmmount,0);
            scaleAmmount += speedScale;
        }
    }

    private void OnDisable()
    {
        this.transform.localScale = new Vector2(0, 0);
    }
}
