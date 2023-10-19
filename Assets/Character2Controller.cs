using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2Controller : MonoBehaviour
{
    //public CharacterController controller;
    
    //Speed
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dodgeSpeed;

    [SerializeField]
    private float turnSmooth;
    float turnSmoothTime;

    //Components
    private Rigidbody rb;
    private Animator anim;

    //Timers
    [SerializeField]
    private float dodgeCD;
    //[SerializeField]
    //private float invencibilityCD;
    private float dodgeTimer = 0;
    //private float invencibilityTimer = 0;

    //GameObjects
    [SerializeField]
    private GameObject weapon;

    [SerializeField]
    private GameObject hand;

    //States
    private bool invencibility = false;
    private bool dodge = false;
    private bool isWalking = false;
    private bool attacking = false;

    //Movement
    private Vector3 direction;
    private Vector3 rollDirection;

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
        //Movimiento
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !dodge)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //controller.Move(direction*speed*Time.deltaTime);
            //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        //Voltereta
        if (Input.GetMouseButtonDown(1) && dodgeTimer <= 0 && isWalking && !dodge)
        {
            rollDirection = direction;
            dodge = true;
            dodgeTimer = dodgeCD;
            //invencibilityTimer = invencibilityCD;
            anim.SetTrigger("Roll");
            //rb.AddForce(direction * dodgeSpeed * Time.deltaTime);
        }

        //Ataque
        if (Input.GetMouseButtonDown(0) && !dodge)
        {
            weapon.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log("Entras");
            attacking = true;
            anim.SetTrigger("Attack1");
        }

        if (dodgeTimer>=0) dodgeTimer-=Time.deltaTime;

        /*if (invencibilityTimer >= 0)
        {
            dodgeTimer -= Time.deltaTime;

        }*/

        //Animaciones
        if (isWalking && !dodge)
        {
            //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            anim.SetBool("Walking", true);
        } else if(!isWalking)
        {
            anim.SetBool("Walking", false);
        }
    }

    void ChangeWeapon(GameObject newWeapon)
    {
        weapon = newWeapon;
        weapon.transform.parent = hand.transform;
    }

    public void RollEnded()
    {
        //Debug.Log("asad");
        dodge = false;
        invencibility = true;
    }

    public void Attack1Ended()
    {
        //Debug.Log("asad");
        weapon.GetComponent<BoxCollider>().enabled = false;
        attacking = false;
    }

    private void FixedUpdate()
    {
        //if(isWalking) rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        //if (dodge) rb.AddForce(direction * speed * 30);
        if (!attacking)
        {
            if (dodge)
            {
                rb.MovePosition(transform.position + rollDirection * dodgeSpeed * Time.fixedDeltaTime);
            }
            else if (isWalking)
            {
                rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
                //anim.SetBool("Walking", true);
            }
        }
    }
}
