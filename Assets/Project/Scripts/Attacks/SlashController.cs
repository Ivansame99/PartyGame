using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashController : MonoBehaviour
{
    [HideInInspector]
    public float finalDamage;

    public float pushForce;

    [SerializeField]
    private GameObject parryParticles;

	[HideInInspector]
	public GameObject owner;

    private bool pushBackParry;
	private bool pushBackAttack;

	private Vector3 attackPosition;
    private float pushForceParry;
	private float pushForceAttack;

	private GameObject player;

    private PlayerHealthController playerHealthController;

    private EnemyHealthController enemy1Controller;

    private EnemyHealth enemy;

    private AudioSource hitSound;
    private void Awake()
    {
        player = transform.parent.gameObject;
        owner = player;
		playerHealthController = player.GetComponent<PlayerHealthController>();
        enemy1Controller = transform.parent.GetComponent<EnemyHealthController>();
        pushForceParry = 30f;
        pushForceAttack = 9f;
		hitSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlashEffect"))
        {
			if (parryParticles!=null) Instantiate(parryParticles, transform.position, parryParticles.transform.rotation);
            pushBackParry = true;
            attackPosition = other.GetComponent<SlashController>().owner.transform.position;
            if (playerHealthController != null)
            {
				playerHealthController.invencibleTimer = 1f; //Para que no se hagan da�o cuando pase esto
            }

            if (enemy1Controller)
            {
                enemy1Controller.timer = 1f; //Para que no se hagan da�o cuando pase esto
                enemy1Controller.invencibility = true;
            }

            if (enemy)
            {
                enemy.timer = 1f;
                enemy.invencibility = true;
            }
            if (hitSound != null)
            {
				hitSound.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
				hitSound.Play();
            }
        }

		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
			attackPosition = other.transform.position;
			pushBackAttack = true;
		}

	}

    private void FixedUpdate()
    {
        if (pushBackParry)
        {
            Vector3 direction = (this.transform.position - attackPosition).normalized;
            direction.y = 0;
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForceParry, ForceMode.Impulse);
			pushBackParry = false;
        }

		if (pushBackAttack)
		{
			Vector3 direction = (player.transform.position - attackPosition).normalized;
			direction.y = 0;
			player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.gameObject.GetComponent<Rigidbody>().AddForce(direction * pushForceAttack, ForceMode.Impulse);
			pushBackAttack = false;
		}
	}
}
