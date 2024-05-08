using UnityEngine;

public class LionProjectile : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    public LayerMask groundLayer;

    [SerializeField] GameObject projectileFeedback;
    [SerializeField] GameObject explosionParticles;
    [SerializeField] ParticleSystem TrailSand;
    [SerializeField] ParticleSystem Ember;
    private bool onlyOnce = false;
    private Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

    private GameObject feedbackClone;

    private SphereCollider collider;
    private Vector3 hitPosition;

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }
    void StopParticleLoop(ParticleSystem particleSystemInstance)
    {
        // Detener el sistema de partículas
        ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
        }

        // Desvincular las partículas del enemigo
        particleSystemInstance.transform.SetParent(null);
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && !onlyOnce)
        {
            feedbackClone = Instantiate(projectileFeedback, new Vector3(hit.point.x,hit.point.y + 0.1f,hit.point.z), rotation);
            hitPosition = hit.point;
            
            onlyOnce = true;
        }

        if(Vector3.Distance(transform.position, hitPosition) <= 0.1) {
            collider.enabled = true;
            StopParticleLoop(TrailSand);
            StopParticleLoop(Ember);
            Instantiate(explosionParticles, hit.point, rotation);
            Destroy(feedbackClone,0.1f);
            Destroy(gameObject,0.1f);
        }
    }
}
