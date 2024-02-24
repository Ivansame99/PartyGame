using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float baseDamage;
    public float finalDamage;

    public float pushForce;
    [SerializeField]
    private float speed;

    private float gravityScale;
    private float globalGravity = -9.81f;
    private GameObject target;
    private bool attack;

    private Rigidbody rb;

    public GameObject owner; //Tiene que ser publico
	public Vector3 ownerPos; //Tiene que ser publico

	private bool ground;

    private float invencibilityTimerOnSpawn = 0.1f;
    public float invencibilityTimerOnSpawnOwner = 0.3f;

    private Vector3 arrowDirection;

    private float destroyTime=10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowDirection = transform.forward;
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
            invencibilityTimerOnSpawn = 50;
            ground = true;
            rb.isKinematic = true;
            Destroy(this.gameObject, destroyTime);
        }
    }

    private void FixedUpdate()
    {
        if (!ground)
        {
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
            // Calcular la rotación para revertir la dirección original
            Quaternion rotacionRevertida = Quaternion.LookRotation(-transform.forward);

            // Aplicar la rotación al objeto
            transform.rotation = rotacionRevertida;

            speed += 5;
            owner = other.transform.parent.gameObject;
            ownerPos = other.transform.parent.gameObject.transform.position;
		}
    }
}
