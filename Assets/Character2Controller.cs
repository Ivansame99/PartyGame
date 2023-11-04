using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    private float greatSwordTimePressed = 0;

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

    private float attackMovement;

    //States
    private bool invencibility = false;
    private bool dodge = false;
    private bool isWalking = false;
    private bool attacking = false;
    private bool moveAttack = false;
    private int greatSwordAttackState = 0;
    private bool isCharging = false;
    private bool isDodging, isAttacking,isSpecialAttacking;
    //Movement
    Vector2 moveUniversal;
    private Vector3 direction;
    private Vector3 rollDirection;
    private Vector3 greatSwordAttackDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null) weaponController = weapon.GetComponent<Weapon>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        //controller = GetComponent<CharacterController>();
    }
    public void SetInputVector(Vector2 direction)
    {
        moveUniversal = direction;
    }
    public void SetDodge(bool pressDodge)
    {
        isDodging = pressDodge;
    }
    public void SetAttack(bool pressAttack)
    {
        isAttacking = pressAttack;
    }
    public void SetSpecialAttack(bool pressSpecialAttack)
    {
        isSpecialAttacking = pressSpecialAttack;
    }
    // Update is called once per frame
    void Update()
    {
        //Movimiento
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(moveUniversal.x, 0f, moveUniversal.y).normalized;

        if (direction.magnitude >= 0.1f && !dodge && !attacking)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //controller.Move(direction*speed*Time.deltaTime);
            //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
            isWalking = true;
            if (attacking) EndCombo();
        }
        else
        {
            isWalking = false;
        }

        //Voltereta
        if (isDodging && dodgeTimer <= 0 && !dodge)
        {
            //Debug.Log(direction);
            if (direction == Vector3.zero)
            {
                rollDirection = this.transform.forward;
            }
            else
            {
                rollDirection = direction;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            dodge = true;
            dodgeTimer = dodgeCD;
            //invencibilityTimer = invencibilityCD;
            anim.SetTrigger("Roll");
            if (attacking) EndCombo();
            //rb.AddForce(direction * dodgeSpeed * Time.deltaTime);
        }
        //Ataque


        Attack();
        ExitAttack();
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
        Destroy(weapon);
        weapon = newWeapon;
        weaponController = weapon.GetComponent<Weapon>();
        weapon.GetComponent<BoxCollider>().isTrigger = false;
        weapon.GetComponent<BoxCollider>().enabled = false;
        GameObject parent = weapon.transform.parent.gameObject;
        weapon.transform.parent = hand.transform;
        Destroy(parent);
        weapon.layer = this.gameObject.layer;
        Destroy(weapon.GetComponent<Animator>());
        switch (weapon.tag)
        {

            case "Sword":
                //Debug.Log("Espada");
                //weapon.transform.rotation = Quaternion.Euler(-4.941f, -251.08f, -340.81f);

                //weapon.transform.eulerAngles = new Vector3(355.059021f, 108.920013f, 19.1900253f);

                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = new Quaternion(0.110803805f, 0.805757701f, 0.131379381f, 0.566759765f);
                break;

            case "GreatSword":
                weapon.transform.localPosition = new Vector3(0.0134290522f, -0.00872362964f, 0.00462706713f);
                weapon.transform.localRotation = new Quaternion(-0.147978142f, 0.552269399f, -0.596118569f, 0.563687623f);
                break;

            case "Bow":
                weapon.transform.localPosition = new Vector3(0.000869999989f, -0.000429999985f, -0.00173999998f);
                weapon.transform.localRotation = new Quaternion(-0.389829606f, -0.491401315f, -0.705046117f, 0.330858946f);
                break;

            default:
                Console.WriteLine("Nothing");
                break;
        }


    }

    public void RollEnded()
    {
        //Debug.Log("asad");
        dodge = false;
        invencibility = true;
    }

    private void Attack()
    {
        //Forma antigua
        /*if (Input.GetMouseButtonDown(0) && !dodge) //&& Time.time - lastComboEnd > attackCoolDownTime
        {
            comboCounter++;
            comboCounter = Mathf.Clamp(comboCounter, 0, weaponController.comboLenght);

            for (int i = 0; i < comboCounter; i++)
            {
                if (i == 0) //Si es el primer ataque del combo
                {
                    if (comboCounter == 1 && anim.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
                    {
                        anim.SetBool("AttackStart", true);
                        anim.SetBool(weaponController.weaponName + "Attack" + (i + 1), true);
                        weapon.GetComponent<BoxCollider>().enabled = true;
                        //rb.AddForce(transform.forward * 3, ForceMode.Impulse);
                        lastClicked = Time.time;
                    }
                }
                else
                {
                    if (comboCounter >= (i + 1) && anim.GetCurrentAnimatorStateInfo(0).IsName(weaponController.weaponName + "Attack" + i))
                    {
                        anim.SetBool(weaponController.weaponName + "Attack" + (i + 1), true);
                        anim.SetBool(weaponController.weaponName + "Attack" + i, false);
                        weapon.GetComponent<BoxCollider>().enabled = true;
                        //rb.AddForce(transform.forward * 3, ForceMode.Impulse);
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
                weapon.GetComponent<BoxCollider>().enabled = false;
            }
        }*/

        //Forma nueva
        if (isAttacking && !dodge)
        {
            if (weapon != null)
            {
                if (Time.time - lastComboEnd > 0.5f && comboCounter <= weaponController.combo.Count) //Tiempo entre combos
                {
                    CancelInvoke("EndCombo");

                    if (Time.time - lastClicked >= 0.3f) //Tiempo entre ataques
                    {
                        attacking = true;
                        anim.runtimeAnimatorController = weaponController.combo[comboCounter].animatorOR;
                        anim.Play("Attack", 0, 0);
                        weaponController.damage = weaponController.combo[comboCounter].damage;
                        weaponController.pushForce = weaponController.combo[comboCounter].pushForce;
                        attackMovement = weaponController.combo[comboCounter].attackMovement;
                        comboCounter++;
                        moveAttack = true;
                        lastClicked = Time.time;
                        weapon.GetComponent<BoxCollider>().enabled = true;
                        if (comboCounter >= weaponController.combo.Count)
                        {
                            comboCounter = 0;
                            lastComboEnd = Time.time;
                        }
                    }
                }
            }
        }

        //Especial espadon
        if (weapon!=null && weapon.tag == "GreatSword")
        {
            if (isSpecialAttacking && !dodge && greatSwordAttackState==0)
            {
                attacking = true;
                isCharging = true;
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("GreatAttack")) anim.Play("GreatAttackUlti", 0, 0);
                weapon.GetComponent<BoxCollider>().enabled = true;
                if (greatSwordTimePressed < 2) greatSwordTimePressed += Time.deltaTime;
                else greatSwordAttackState = 1; //Llega al maximo tiempo
                //Debug.Log(greatSwordTimePressed);
            }
            else if(!isSpecialAttacking && isCharging == true)//Para de apretar antes de llegar al maximo
            {
                greatSwordAttackState = 1;
            }

            //Se acaba el tiempo
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("GreatAttack") && greatSwordTimePressed <= 0)
            {
                attacking = false;
                isCharging = false;
                greatSwordTimePressed = 0;
                greatSwordAttackState = 0;
                weapon.GetComponent<BoxCollider>().enabled = false;
                anim.SetTrigger("Ulti");
            }

            if(greatSwordAttackState==1 && greatSwordTimePressed>=0) greatSwordTimePressed-= Time.deltaTime; //Se va reduciendo el timer
        }
    }

    private void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 0.3f);
            //weapon.GetComponent<BoxCollider>().enabled = false;
            //attacking = false;
        }
    }

    private void EndCombo()
    {
        attacking = false;
        comboCounter = 0;
        lastComboEnd = Time.time;
        weapon.GetComponent<BoxCollider>().enabled = false;
    }


    /*public void Attack1Ended()
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
    }*/

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
            
        } else if (greatSwordAttackState == 1 && greatSwordTimePressed >= 0) rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);

        if (moveAttack)
        {
            rb.AddForce(transform.forward * attackMovement, ForceMode.Impulse);
            moveAttack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" || other.gameObject.tag == "GreatSword" || other.gameObject.tag == "Bow") //|| tag==greatsword||tag==bow
        {
            ChangeWeapon(other.gameObject);
        }
    }
}
