using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float baseDamage;
    public float finalDamage;

    private float pushForce;
    [SerializeField]
    private float speed;

    private float gravityScale;
    private float globalGravity = -9.81f;
    private GameObject target;
    private bool attack;

    private Rigidbody rb;

    public GameObject owner; //Tiene que ser publico

    private bool ground;

    private float invencibilityTimerOnSpawn = 0.1f;
    public float invencibilityTimerOnSpawnOwner = 0.3f;

    private Vector3 arrowDirection;
    // Start is called before the first frame update
    void Start()
    {
        //gravityScale = 0.1f;
        rb = GetComponent<Rigidbody>();
        this.GetComponent<BoxCollider>().enabled = false;
        arrowDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (invencibilityTimerOnSpawn >= 0)
        {
            invencibilityTimerOnSpawn -= Time.deltaTime;
            
        }
        else
        {
            this.GetComponent<BoxCollider>().enabled = true;
        }

        if (invencibilityTimerOnSpawnOwner >= 0)
        {
            invencibilityTimerOnSpawnOwner -= Time.deltaTime;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player"))
        {
            if (collision.gameObject == owner && invencibilityTimerOnSpawnOwner > 0)
            {
                //Debug.Log("Te has pegado a ti mismo");
                //Para que no colisione el jugador que ha lanzado la flecha al spawnear
            }
            else
            {
                target = collision.gameObject;
                attack = true;
            }
        }

        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall")
        {
            this.GetComponent<BoxCollider>().enabled = false;
            invencibilityTimerOnSpawn = 50;
            ground = true;
            rb.isKinematic = true;
            Destroy(this.gameObject, 2f);
        }
    }

    private void FixedUpdate()
    {
        if (!ground)
        {
            if (attack && target != null)
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                direction.y = 0;
                target.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
                attack = false;
            }

            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
            //rb.velocity = transform.forward * speed;
            //Debug.Log(rb.velocity);

            /*if (rb.velocity != Vector3.zero)
                rb.rotation = Quaternion.LookRotation(rb.velocity/1000);*/

            //CustomGravity();
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
        //Debug.Log(s);
    }

    public void SetPushForce(float s)
    {
        pushForce = s;
        //Debug.Log(s);
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
            //arrowDirection = Vector3.Reflect(arrowDirection, Random.onUnitSphere);
        }
    }
}
