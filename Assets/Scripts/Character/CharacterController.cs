using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float strafeSpeed;
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody hips;

    [SerializeField]
    private Animator anim;

    //Variables que tienen que ser publicas
    public bool isGround;

    private bool isWalking=false;
    private bool isWalkingLeft = false;
    private bool isWalkingRight = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if(isWalking) anim.SetBool("Walking", true);
        else anim.SetBool("Walking", false);

        if (isWalkingLeft) anim.SetBool("WalkLeft", true);
        else anim.SetBool("WalkLeft", false);

        if (isWalkingRight) anim.SetBool("WalkRight", true);
        else anim.SetBool("WalkRight", false);
    }

    private void FixedUpdate()
    {
        isWalking = false;
        isWalkingLeft = false;
        isWalkingRight = false;
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            isWalking=true;
            hips.AddForce(hips.transform.forward * speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            isWalkingRight = true;
            hips.AddForce(hips.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            isWalkingLeft = true;
            hips.AddForce(-hips.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            isWalking = true;
            hips.AddForce(-hips.transform.forward * speed);
        }

        //Jump
        if (Input.GetAxis("Jump") > 0)
        {
            if (isGround)
            {
                hips.AddForce(new Vector3(0, jumpForce, 0));
                isGround = false;
            }
        }
    }



}
