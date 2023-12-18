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

    private PlayerController playerController;

    private EnemyHealthController enemy1Controller;

    private AudioSource hitSound;
    private void Awake()
    {
        player = transform.parent.parent.gameObject;
        playerController = player.GetComponent<PlayerController>();
        enemy1Controller = transform.parent.parent.GetComponent<EnemyHealthController>();
        pushForceParry = 15f;
        hitSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SlashEffect")
        {
            pushBack = true;
            attackPosition = other.transform.position;
            if (playerController != null)
            {
                playerController.invencibilityTimer = 1f; //Para que no se hagan daño cuando pase esto
                playerController.invencibility = true;
            }

            if (enemy1Controller)
            {
                enemy1Controller.timer = 1f; //Para que no se hagan daño cuando pase esto
                enemy1Controller.invencibility = true;
            }

            if (!hitSound.isPlaying)
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
