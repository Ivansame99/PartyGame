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

    //Movement
    private Vector3 direction;
    private Vector3 rollDirection;
    private Vector3 greatSwordAttackDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null) weaponController = weapon.GetComponent<Weapon>();
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
        if (Input.GetKey(KeyCode.Space) && dodgeTimer <= 0 && !dodge)
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
        //Forma nueva
        if (Input.GetMouseButtonDown(0) && !dodge)
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
            if (Input.GetMouseButton(1) && !dodge && greatSwordAttackState==0)
            {
                if (direction == Vector3.zero)
                {
                    greatSwordAttackDirection = this.transform.forward;
                }
                else
                {
                    greatSwordAttackDirection = direction;
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }

                attacking = true;
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("GreatAttack")) anim.Play("GreatAttackUlti", 0, 0);
                weapon.GetComponent<BoxCollider>().enabled = true;
                if (greatSwordTimePressed < 2) greatSwordTimePressed += Time.deltaTime;
                else greatSwordAttackState = 1; //Llega al maximo tiempo
                //Debug.Log(greatSwordTimePressed);
            }
            else if (Input.GetMouseButtonUp(1)) //Para de apretar antes de llegar al maximo
            {
                greatSwordAttackState = 1;
            }

            //Se acaba el tiempo
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("GreatAttack") && greatSwordTimePressed <= 0)
            {
                attacking = false;
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

    private void FixedUpdate()
    {
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
            
        } else if (greatSwordAttackState == 1 && greatSwordTimePressed >= 0) rb.MovePosition(transform.position + greatSwordAttackDirection * speed * Time.fixedDeltaTime);

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
