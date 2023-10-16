using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2Controller : MonoBehaviour
{
    //public CharacterController controller;

    public float speed = 6;

    public float turnSmooth = 1f;
    float turnSmoothTime;
    private Rigidbody rb;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private float dodgeCD;

    private float dodgeTimer = 0;

    private bool dodge = false;
    private bool isWalking = false;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            //controller.Move(direction*speed*Time.deltaTime);
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            isWalking = true;
            anim.SetBool("Walking", true);
        }
        else
        {
            isWalking = false;
            anim.SetBool("Walking", false);
        }

        if (Input.GetMouseButtonDown(1) && dodgeTimer <= 0)
        {
            dodge = true;
            dodgeTimer = dodgeCD;
            Debug.Log("Rueda");
            anim.SetTrigger("Roll");
            
        }

        if (isWalking)
        {
            anim.SetBool("Walking", true);
        } else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void FixedUpdate()
    {
        //if(isWalking) rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        if (dodge) rb.AddForce(direction * speed * 30);
    }
}
