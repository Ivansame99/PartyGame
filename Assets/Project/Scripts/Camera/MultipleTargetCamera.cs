using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 50f;
    [SerializeField]
    private Camera guiCamera;
    #endregion

    #region Variables
    private List<Transform> targets = new List<Transform>();
    private Camera mainCamera;
    private Vector3 velocity;
    #endregion

    #region Life Cycle
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }
    #endregion

    #region Public Methods
    public void AddPlayer(Transform player)
    {
        if (targets.Contains(player))
        {
            return;
        }

        targets.Add(player);
    }

    public bool RemovePlayer(Transform player)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == player)
            {
                targets.Remove(targets[i]);
                player.transform.position = new Vector3(100, 10, 0);
                return true;
            }
        }
        return false;
    }

    #endregion

    #region Private Methods
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, newZoom, Time.deltaTime);
        guiCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, newPosition, ref velocity, smoothTime);
    }

    private float GetGreatestDistance()
    {
        if (targets.Count == 0)
        {
            return 0;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].position);
            }
        }
        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 0)
        {
            return Vector3.zero;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].position);
            }
        }

        return bounds.center;
    }
    #endregion
}