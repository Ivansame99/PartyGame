using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRectanglePreview : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float scaleAmmount = 1.5f;
    [SerializeField] float maxScale;

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
        if (scaleAmmount < maxScale)
        {
            this.transform.localScale = new Vector3(0.25f, scaleAmmount, 0.5f);
            scaleAmmount += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        this.transform.localScale = new Vector3(0, 0, 1);
    }
}
