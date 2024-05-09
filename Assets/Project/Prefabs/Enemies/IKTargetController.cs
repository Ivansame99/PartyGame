using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetController : MonoBehaviour
{
    [SerializeField] Transform body = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] Vector3 footOffset = default;

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = body.position + (body.forward * stepLength) + footOffset;

        // Calcular la distancia entre currentPosition y targetPosition
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // Si la distancia supera el umbral de stepDistance, mover el target hacia targetPosition con lerp
        if (distance > stepDistance)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * speed);
        }
    }
}
