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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //Movement
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walking", true);
            hips.AddForce(hips.transform.forward * speed);
        } else
        {
            anim.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            hips.AddForce(hips.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            hips.AddForce(-hips.transform.right * strafeSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
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
