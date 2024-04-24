using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class LionProjectile : MonoBehaviour
{
    [SerializeField] private float maxDistance = 1f;
    public LayerMask groundLayer;

    [SerializeField] GameObject projectileFeedback;
    [SerializeField] GameObject explosionParticles;

    private bool onlyOnce = false;
    private Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

    private GameObject feedbackClone;

    private SphereCollider collider;

    public float baseDamage;
    public float finalDamage;
    public float pushForce;

    [HideInInspector] public GameObject owner;
    [HideInInspector] public Enemy enemy;

    public void SetPushForce(float s)
    {
        pushForce = s;
    }

    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.enabled = false;
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, groundLayer))
        {
            collider.enabled = true;
            Instantiate(explosionParticles, hit.point, rotation);
            Destroy(feedbackClone);
            Destroy(gameObject);
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && !onlyOnce)
        {
            feedbackClone = Instantiate(projectileFeedback, new Vector3(hit.point.x,hit.point.y + 0.1f,hit.point.z), rotation);
            onlyOnce = true;
        }
    }
}
