using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float baseDamage;

    [SerializeField]
	private AudioSource arrowCollision;

	[HideInInspector]
    public float finalDamage;
	
    [HideInInspector]
	public float pushForce;

	[SerializeField]
	private GameObject parryParticles;

	//[SerializeField]
	private float speed;

    private float gravityScale;
    private float globalGravity = -9.81f;
    private GameObject target;
    private bool attack;

    private Rigidbody rb;

    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
	public Vector3 ownerPos;

	private bool ground;

    public float invencibilityTimerOnSpawnOwner = 0.3f;

    private float destroyTime=10f;

    private float minPitch = 0.8f;
    private float maxPitch = 1.2f;

    [SerializeField]
	private AudioSource hitSound;

	void Start()
    {
        rb = GetComponent<Rigidbody>();
	}

    void Update()
    {
        if (invencibilityTimerOnSpawnOwner >= 0)
        {
            invencibilityTimerOnSpawnOwner -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            this.GetComponent<BoxCollider>().enabled = false;
            ground = true;
            rb.isKinematic = true;
            Destroy(this.gameObject, destroyTime);
		}
		arrowCollision.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
		arrowCollision.Play();
	}

    private void FixedUpdate()
    {
        if (!ground)
        {
            //rb.AddForce(transform.position + transform.forward * speed, ForceMode.Impulse);
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }

    void CustomGravity()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    public void SetSpeed(float s)
    {
        speed = s;
        gravityScale = s / 10;
    }

    public void SetPushForce(float s)
    {
        pushForce = s;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SlashEffect")
        {
			if(parryParticles != null) Instantiate(parryParticles, transform.position, parryParticles.transform.rotation);
			// Calcular la rotación para revertir la dirección original
			Quaternion rotacionRevertida = Quaternion.LookRotation(-transform.forward);

            // Aplicar la rotación al objeto
            transform.rotation = rotacionRevertida;

            speed += 5;
            owner = other.transform.parent.gameObject;
            ownerPos = other.transform.parent.gameObject.transform.position;
			arrowCollision.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
			arrowCollision.Play();

			if (hitSound != null)
			{
				hitSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
				hitSound.Play();
			}
		}
    }
}
