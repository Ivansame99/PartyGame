using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private float speed;
    //public int comboLenght;

    private GameObject target;
    private bool attack;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            target = collision.gameObject;
            target.GetComponent<EnemyHealthController>().ReceiveDamage(damage);
            attack = true;
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Ground") Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (attack && target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;

            direction.y = 1;
            target.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForce, ForceMode.Impulse);
            attack = false;
        }

        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }
}
