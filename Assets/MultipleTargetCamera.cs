using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> Targets;
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = 0.5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;
    private Camera cam;

    private void Start()
    {
        Targets.Clear();
        cam = GetComponent<Camera>();
        GameObject[] jugadoresArray = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject jugadorObj in jugadoresArray)
        {
            Targets.Add(jugadorObj.transform);
        }
    }

    private void LateUpdate()
    {
        if (Targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private float GetGreatestDistance()
    {
        if (Targets.Count == 0)
        {
            return 0;
        }

        var bounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {
            if (Targets[i] != null)
            {
                bounds.Encapsulate(Targets[i].position);
            }
        }
        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if (Targets.Count == 0)
        {
            return Vector3.zero;
        }

        var bounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {
            if (Targets[i] != null)
            {
                bounds.Encapsulate(Targets[i].position);
            }
        }

        return bounds.center;
    }
}