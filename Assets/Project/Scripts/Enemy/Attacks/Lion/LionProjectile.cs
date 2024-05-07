using UnityEngine;

public class LionProjectile : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    public LayerMask groundLayer;

    [SerializeField] GameObject projectileFeedback;
    [SerializeField] GameObject explosionParticles;

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
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.6f, groundLayer))
        {
           
        }

        if (Physics.Raycast(ray, out hit, maxDistance, groundLayer))
        {
            //Instantiate(explosionParticles, hit.point, rotation);
            //Destroy(gameObject);
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && !onlyOnce)
        {
            feedbackClone = Instantiate(projectileFeedback, new Vector3(hit.point.x,hit.point.y + 0.1f,hit.point.z), rotation);
            hitPosition = hit.point;
            
            onlyOnce = true;
        }

        if(Vector3.Distance(transform.position, hitPosition) <= 0.1) {
            collider.enabled = true;
            Instantiate(explosionParticles, hit.point, rotation);
            Destroy(feedbackClone,0.2f);
            Destroy(gameObject,0.2f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
