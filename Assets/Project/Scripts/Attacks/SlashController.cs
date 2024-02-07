using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlashController : MonoBehaviour
{
    public float finalDamage;
    public float pushForce;

    private bool pushBack;
    private Vector3 attackPosition;
    private float pushForceParry;

    private GameObject player;

    private PlayerHealthController playerHealthController;

    private EnemyHealthController enemy1Controller;

    private AudioSource hitSound;
    private void Awake()
    {
        player = transform.parent.gameObject;
		playerHealthController = player.GetComponent<PlayerHealthController>();
        enemy1Controller = transform.GetComponent<EnemyHealthController>();
        pushForceParry = 15f;
        hitSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SlashEffect")
        {
            pushBack = true;
            attackPosition = other.transform.position;

            if (playerHealthController != null)
            {
				playerHealthController.invencibleTimer = 1f; //Para que no se hagan daño cuando pase esto
                //playerController.invencibility = true;
            }

            if (enemy1Controller)
            {
                enemy1Controller.timer = 1f; //Para que no se hagan daño cuando pase esto
                enemy1Controller.invencibility = true;
            }

            if (hitSound != null)
            {
                hitSound.Play();
                hitSound.pitch = UnityEngine.Random.Range(0.5f, 0.8f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (pushBack)
        {
            Vector3 direction = (this.transform.position - attackPosition).normalized;
            direction.y = 0;
            player.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForceParry, ForceMode.Impulse);
            pushBack = false;
        }
    }
}
