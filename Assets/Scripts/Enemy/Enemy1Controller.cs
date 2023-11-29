using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1Controller : MonoBehaviour
{
    [SerializeField] private int HP = 9;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP < 0)
        {
            animator.SetTrigger("die");
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    

    void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("SlashEffect"))
        {
            TakeDamage(3);
        }
    }

}
