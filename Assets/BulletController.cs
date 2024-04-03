using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public float baseDamage;

	internal float finalDamage;

	internal float pushForce;

	[SerializeField]
	private GameObject parryParticles;

	[SerializeField]
	private float speed;

	private Rigidbody rb;

	internal GameObject owner;

	internal Vector3 ownerPos;

	private float minPitch = 0.8f;
	private float maxPitch = 1.2f;

	[SerializeField]
	private AudioSource hitSound;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		rb.MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "SlashEffect")
		{
			if (parryParticles != null) Instantiate(parryParticles, transform.position, parryParticles.transform.rotation);
			// Calcular la rotación para revertir la dirección original
			Quaternion rotacionRevertida = Quaternion.LookRotation(-transform.forward);

			// Aplicar la rotación al objeto
			transform.rotation = rotacionRevertida;

			speed += 5;
			owner = other.transform.parent.gameObject;
			ownerPos = other.transform.parent.gameObject.transform.position;

			if (hitSound != null)
			{
				hitSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
				hitSound.Play();
			}
		}
	}
}
