using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform[] targets; // Array de los cuatro objetivos
    public float minDistance = 5f; // Distancia mínima entre objetivos antes de alejar la cámara
    public float maxDistance = 15f; // Distancia máxima entre objetivos antes de acercar la cámara
    public float smoothSpeed = 0.5f;
    public float cameraHeight = 5f; // Altura de la cámara

    private Vector3 cameraVelocity;

    private void Update()
    {
        if (targets.Length == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();
        float distance = GetGreatestDistance();

        Vector3 desiredPosition = centerPoint - transform.forward * distance;
        desiredPosition.y = cameraHeight; // Ajusta la altura de la cámara

        float zoom = Mathf.Clamp(distance, minDistance, maxDistance);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraVelocity, smoothSpeed);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        Vector3 centerPoint = Vector3.zero;
        foreach (Transform target in targets)
        {
            centerPoint += target.position;
        }
        centerPoint /= targets.Length;

        return centerPoint;
    }

    float GetGreatestDistance()
    {
        float maxDistance = 0f;
        Vector3 centerPoint = GetCenterPoint();

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(target.position, centerPoint);
            maxDistance = Mathf.Max(maxDistance, distance);
        }

        return maxDistance;
    }
}