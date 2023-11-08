using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Character2Controller : MonoBehaviour
{
    //public CharacterController controller;
    [Header("Speed")]
    //Speed
    [SerializeField]
    private float speed;
    [SerializeField]
    private float dodgeSpeed;

    [SerializeField]
    private float turnSmooth;
    float turnSmoothTime;

    [Header("Stamina")]
    //Stamina
    [SerializeField]
    private float maxStamina;
    [SerializeField]
    private float staminaRegen;
    [SerializeField]
    private float dodgeStamina;
    [SerializeField]
    private float attackStamina;
    [SerializeField]
    private float greatSwordAttackStamina;

    private float stamina;

    [Header("Components")]
    //Components
    private Rigidbody rb;
    private Animator anim;
    [SerializeField]
    private StaminaBarController staminaBarC;

    [Header("Timers")]
    //Timers
    [SerializeField]
    private float dodgeCD;
    //[SerializeField]
    //private float invencibilityCD;
    private float dodgeTimer = 0;
    //private float invencibilityTimer = 0;
    private float greatSwordTimePressed = 0;

    [Header("Weapons")]
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
    private Vector3 greatSwordAttackDirection = Vector3.zero;

    //Positions&Rotations
    public GameObject boundCharacter;
    public GameObject SlashP, Slash;

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null) weaponController = weapon.GetComponent<Weapon>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stamina=maxStamina;
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
            WasteStamina(dodgeStamina);
            anim.SetTrigger("Roll");
            if (attacking) EndCombo();
        }

        //Ataque
        Attack();
        ExitAttack();

        //Stamina
        RecoverStamina();

        //Animaciones
        if (isWalking && !dodge)
        {
            anim.SetBool("Walking", true);
        }
        else if (!isWalking)
        {
            anim.SetBool("Walking", false);
        }

        //CD de la voltereta
        if (dodgeTimer >= 0) dodgeTimer -= Time.deltaTime;
    }

    void RecoverStamina()
    {
        if (stamina < maxStamina && staminaBarC.canRecover)
        {
            stamina += Time.deltaTime * staminaRegen;
            staminaBarC.RecoverBar(stamina/maxStamina);
        }
    }

    void WasteStamina(float wastedStamina)
    {
        stamina -= wastedStamina;
        staminaBarC.SetProgress(stamina / maxStamina, 2);
    }

    void WasteStaminaPerSecond()
    {
        stamina -= Time.deltaTime * greatSwordAttackStamina;
        staminaBarC.WasteBar(stamina / maxStamina);
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
        dodge = false;
        invencibility = true;
    }

    private void Attack()
    {
        //Forma nueva
        if (isAttacking && !dodge)
        {
            if (weapon != null)
            {
                if (Time.time - lastComboEnd > 0.5f && comboCounter <= weaponController.combo.Count && stamina >= attackStamina) //Tiempo entre combos
                {
                    CancelInvoke("EndCombo");
                   
                    if (Time.time - lastClicked >= 0.2f) //Tiempo entre ataques
                    {

                        Vector3 savedPosition = boundCharacter.transform.position;
                        Quaternion savedRotation = boundCharacter.transform.rotation;
                        SlashP.transform.rotation = savedRotation;
                        Slash.transform.rotation = savedRotation;

                        SlashP.transform.position = savedPosition;
                        Slash.transform.position = savedPosition;

                   
                        WasteStamina(attackStamina);
                        attacking = true;
                        anim.runtimeAnimatorController = weaponController.combo[comboCounter].animatorOR;
                        anim.Play("Attack", 0, 0);
                        weaponController.damage = weaponController.combo[comboCounter].damage;
                        weaponController.pushForce = weaponController.combo[comboCounter].pushForce;
                        attackMovement = weaponController.combo[comboCounter].attackMovement;
                        comboCounter++;
                        moveAttack = true;
                        lastClicked = Time.time;
                       // weapon.GetComponent<BoxCollider>().enabled = true;
                        if (comboCounter >= weaponController.combo.Count)
                        {
                            comboCounter = 0;
                            lastComboEnd = Time.time;
                        }


                     

                        SlashP.SetActive(false);
                        Slash.SetActive(false);
                     

                        SlashP.SetActive(true);
                        Slash.SetActive(true);
                    
                    }
                }
            }
        }

        //Especial espadon
        if (weapon != null && weapon.tag == "GreatSword")
        {
            if (isSpecialAttacking && !dodge && greatSwordAttackState==0)
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
                isCharging = true;
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("GreatAttack")) anim.Play("GreatAttackUlti", 0, 0);
                weapon.GetComponent<BoxCollider>().enabled = true;
                if (stamina >= 10)
                {
                    WasteStaminaPerSecond();
                    greatSwordTimePressed += Time.deltaTime;
                }
                else greatSwordAttackState = 1;
                //if (greatSwordTimePressed < 2) greatSwordTimePressed += Time.deltaTime;
                //else greatSwordAttackState = 1; //Llega al maximo tiempo
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

            if (greatSwordAttackState == 1 && greatSwordTimePressed >= 0) greatSwordTimePressed -= Time.deltaTime; //Se va reduciendo el timer
        }
    }

    private void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            SlashP.SetActive(false);
            Slash.SetActive(false);
            Invoke("EndCombo", 0.1f);
        }
    }

    private void EndCombo()
    {
        SlashP.SetActive(false);
        Slash.SetActive(false);
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
