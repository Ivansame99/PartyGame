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

    //Weapon
    [SerializeField]
    private GameObject weapon;
    private Weapon weaponController;

    private int comboCounter;
    float attackCoolDownTime = 0.1f;
    float lastClicked;
    float lastComboEnd;

    [SerializeField]
    private GameObject hand;

    //States
    private bool invencibility = false;
    private bool dodge = false;
    private bool isWalking = false;
    private int attacking = 0;

    //Movement
    private Vector3 direction;
    private Vector3 rollDirection;

    // Start is called before the first frame update
    void Start()
    {
        weaponController = weapon.GetComponent<Weapon>();
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
        Attack();
        /*if (Input.GetMouseButtonDown(0) && !dodge && Time.time - lastComboEnd > attackCoolDownTime)
        {
            //Attack();
            weapon.GetComponent<BoxCollider>().enabled = true;
            //Debug.Log("Entras");
            if(attacking==0) anim.SetTrigger("Attack1");
            attacking++;
            rb.AddForce(transform.forward * 3, ForceMode.Impulse);
        }*/

        if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;

        /*if (invencibilityTimer >= 0)
        {
            dodgeTimer -= Time.deltaTime;

        }*/

        //Animaciones
        if (isWalking && !dodge)
        {
            //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            anim.SetBool("Walking", true);
        }
        else if (!isWalking)
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

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !dodge && Time.time - lastComboEnd > attackCoolDownTime)
        {
            comboCounter++;
            comboCounter = Mathf.Clamp(comboCounter, 0, weaponController.comboLenght);

            for (int i = 0; i < comboCounter; i++)
            {
                if (i == 0) //Si es el primer ataque del combo
                {
                    if (comboCounter == 1)
                    {
                        anim.SetBool("AttackStart", true);
                        anim.SetBool(weaponController.weaponName + "Attack" + (i + 1), true);
                        lastClicked = Time.time;
                    }
                }
                else
                {
                    if (comboCounter >= (i + 1) && anim.GetCurrentAnimatorStateInfo(0).IsName(weaponController.weaponName + "Attack" + i))
                    {
                        anim.SetBool(weaponController.weaponName + "Attack" + (i + 1), true);
                        anim.SetBool(weaponController.weaponName + "Attack" + i, true);
                        lastClicked = Time.time;
                    }
                }
            }
        }

        //animator bool reset
        for (int i = 0; i < weaponController.comboLenght; i++)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsName(weaponController.weaponName + "Attack" + (i + 1)))
            {
                comboCounter = 0;
                lastComboEnd = Time.time;
                anim.SetBool(weaponController.weaponName + "Attack" + (i + 1), false);
                anim.SetBool("AttackStart", false);
            }
        }
    }

    public void Attack1Ended()
    {
        if (attacking == 1) //Si solo ha hecho un ataque para, sino ya entrara en el siguiente
        {
            Debug.Log("Sales");
            //Debug.Log("asad");
            rb.velocity = Vector3.zero;
            weapon.GetComponent<BoxCollider>().enabled = false;
            attacking = 0;
        }
        else AttackCombo();
    }

    public void Attack2Ended()
    {
        Debug.Log("Sales");
        //Debug.Log("asad");
        rb.velocity = Vector3.zero;
        weapon.GetComponent<BoxCollider>().enabled = false;
        attacking = 0;
    }

    public void AttackCombo()
    {
        if (attacking >= 2) anim.SetTrigger("Attack2");
    }

    private void FixedUpdate()
    {
        //if(isWalking) rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        //if (dodge) rb.AddForce(direction * speed * 30);
        if (attacking == 0)
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
