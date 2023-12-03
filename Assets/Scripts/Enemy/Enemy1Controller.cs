using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private float HP;
    [SerializeField] private int damageValue;
    [SerializeField] private float inmuneTime;
    private float timer;
    private float lowHP;
    private Animator animator;
    private bool onlyOnce;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lowHP = HP * 0.50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void TakeDamage(int damageAmount)
    {
        if (timer <= 0)
        {
            timer = inmuneTime;
            HP -= damageAmount;
            if (HP < 0)
            {
                animator.SetTrigger("die");
                animator.SetBool("isChasing",false);
            }
            else if (HP <= lowHP)
            {
                animator.SetTrigger("evading");
            }
            else
            {
                //if(!animator.GetBool("isEvading")) 
                    animator.SetTrigger("damage");               
            }
        }



    }

    

    public void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SlashEffect"))
        {
            TakeDamage(damageValue);
        }
    }

}
