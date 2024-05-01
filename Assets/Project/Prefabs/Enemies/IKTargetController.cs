using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetController : MonoBehaviour
{
    public Transform root;
    public float maxDistance = 1.5f;
    public float lerpSpeed = 0.1f;

    private void Update()
    {
        // Calcula la distancia entre este objeto y el root
        float distance = Vector3.Distance(transform.position, root.position);

        // Si la distancia es mayor que la distancia m�xima permitida
        if (distance > maxDistance)
        {
            // Calcula la direcci�n hacia el root
            Vector3 direction = (root.position - transform.position).normalized;

            // Calcula la posici�n hacia la que se mover� el target
            Vector3 targetPosition = transform.position + direction * (distance - maxDistance);

            // Aplica el lerp para suavizar el movimiento hacia adelante
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        }
    }
}
