using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRectanglePreview : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float scaleAmmount = 1.5f;

    private void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 1);
    }

    private void OnEnable()
    {
        scaleAmmount = 0;
    }

    private void Update()
    {
        if (scaleAmmount < 1)
        {
            this.transform.localScale = new Vector3(0.75f, scaleAmmount, 1);
            scaleAmmount += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        this.transform.localScale = new Vector3(0, 0, 1);
    }
}
