using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> Targets;

    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = .5f;
    public float minZoom = 40f;
    public float mazZoom = 10f;
    public float ZoomLimiter = 50f;
    private Camera Cam;

     void Start()
    {
        Cam = GetComponent<Camera>();   
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
    void Zoom()
    {
      //  Debug.Log(GetGreatestDistance());
        float newZoom = Mathf.Lerp(mazZoom,minZoom,GetGreatestDistance()/ZoomLimiter);
        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, newZoom, Time.deltaTime);
    }
     void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {

            bounds.Encapsulate(Targets[i].position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
       if(Targets.Count == 1)
        {
            return Targets[1].position;
        }


        var bounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {

            bounds.Encapsulate(Targets[i].position);
        }

        return bounds.center;
    }
}
