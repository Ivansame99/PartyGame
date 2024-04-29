using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DrunkProjectile : MonoBehaviour
{
    //Linea de trayectoria
    [Header("Line Renderer")]
    [SerializeField] LineRenderer line;
    [SerializeField] private float lineTimeLife = 0.5f;
    private SphereCollider collider;

    //Variables para el cálculo de la trayectoria
    private float step;
    private Camera _cam;

    //Posiciones
    [HideInInspector] public Transform firePoint;
    [HideInInspector] public Transform finalPosition;

    //Prefabs
    [Header("Prefabs")]
    [SerializeField] GameObject projectileFeedback;
    [SerializeField] GameObject explosionParticles;

    [Header("Stats")]
    [SerializeField] private float speedMultiplier = 2f;


    //Prefabs Clones
    private GameObject projectile;

    //Variables de legibilidad
    private Vector3 fPos;
    Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
    private bool start = false;
    private float yHitPos;

    //COOLDOWN ATTACKS
    [SerializeField] private float selfDestruction;



    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        _cam = Camera.main;
        
        step = 0.1f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position,-transform.up, out hit,0.3f))
        {
            yHitPos = hit.point.y;
            if(finalPosition != null) fPos = new Vector3(finalPosition.position.x, yHitPos + 0.1f, finalPosition.position.z);
        }
        if (!start)
            {
                Vector3 direction = fPos - firePoint.position;
                Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
                Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
                float height = targetPos.y + targetPos.magnitude / 2f;
                height = Mathf.Max(0.01f, height);
                float angle;
                float v0;
                float time;

                CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);
                //DrawPath(groundDirection.normalized, v0, angle, time, step);
                start = true;
                if (start)
                {
                    StopAllCoroutines();
                    StartCoroutine(Coroutine_Movement(groundDirection.normalized, v0, angle, time, speedMultiplier));
                    projectile = Instantiate(projectileFeedback, fPos, rotation);
                    if(projectile != null) Destroy(projectile, selfDestruction);
                    if(gameObject != null) Destroy(gameObject, selfDestruction);
            }
            }
        
    }
    private void Update()
    {
        if(line != null) Destroy(line, lineTimeLife);
        if (Vector3.Distance(fPos, transform.position) < 3f)
        {
            //enemy.drunkAudioManager.PlayProjectileHit();
            Destroy(projectile);
            collider.enabled = true;
            Instantiate(explosionParticles, fPos, rotation);
            Destroy(gameObject,0.1f);
        }

        
    }

    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        line.positionCount = (int)(time / step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i,2);
            line.SetPosition(count, firePoint.position + direction*x + Vector3.up*y);
            count++;
        }
        float xfinal = v0 * time * Mathf.Cos(angle);
        float yfinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        line.SetPosition(count, firePoint.position + direction * xfinal + Vector3.up * yfinal);
    }

    private float QuadraticEquation(float a, float b, float c, int sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);
        v0 = b / Mathf.Sin(angle);
    }
    IEnumerator Coroutine_Movement(Vector3 direction, float v0, float angle, float time, float speedMultiplier)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime * speedMultiplier; // Ajusta el tiempo de acuerdo al multiplicador de velocidad
            float x = v0 * Mathf.Cos(angle) * t;
            float y = v0 * Mathf.Sin(angle) * t - 0.5f * 9.8f * t * t;
            transform.position = firePoint.position + direction * x + Vector3.up * y;
            yield return null;
        }
    }
}
