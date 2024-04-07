using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Ak47")]
public class PlayerAk47State : PlayerState<PlayerController>
{
	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private float fireRate = 0.1f;

	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float recoilForce = 1.0f;

	[SerializeField]
	private float bulletSpreadAngle = 5f;

	[SerializeField]
	private GameObject shootParticle;

	private float timer = 0;
	private float turnSmooth = 0.1f;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.anim.SetBool("Bow", true);
		player.EquipAk();
	}

	public override void Update()
	{
		//Change to Idle
		if (!player.ak)
		{
			player.ChangeState(typeof(PlayerIdleState));
			return;
		}

		if (player.isJumping && player.groundCheck.DetectGround())
		{
			player.ChangeState(typeof(PlayerJumpState));
			return;
		}

		if (player.direction != Vector3.zero) //Player can rotate
		{
			float targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, 0.1f);
			player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}

		if (timer<=0)
		{
			player.transform.DOPunchScale(new Vector3(1f, -1f, 1f), 0.1f).SetRelative(true).SetEase(Ease.OutBack);
			Shoot();
			timer = fireRate;
		} else
		{
			timer-=Time.deltaTime;
		}
	}

	void Shoot()
	{
		Quaternion rot = player.transform.rotation;
		//Vector3 randomRot = rot.eulerAngles + new Vector3(Random.Range(-bulletSpreadAngle, bulletSpreadAngle), 0, 0);
		Vector3 randomRot = rot.eulerAngles + new Vector3(90, 0, Random.Range(-bulletSpreadAngle, bulletSpreadAngle));

		// Instanciar la bala en la dirección calculada
		Instantiate(shootParticle, player.akFirePoint.position, rot);
		GameObject bullet = Instantiate(bulletPrefab, player.akFirePoint.position, rot);
		bullet.transform.eulerAngles = randomRot;

		BulletController bc = bullet.GetComponent<BulletController>();

		bc.finalDamage = bc.baseDamage + player.powerController.PowerDamage();
		bc.owner = player.gameObject;
		bc.ownerPos = player.gameObject.transform.position;
	}

	public override void FixedUpdate()
	{
		//Recoil movement
		player.rb.AddForce(-player.transform.forward * recoilForce, ForceMode.Impulse);

		//If its in ground, player can move
		if (player.groundCheck.DetectGround() && player.direction!=Vector3.zero)
		{
			Vector3 currentVelocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

			if (currentVelocity.magnitude > moveSpeed)
			{
				currentVelocity = currentVelocity.normalized * moveSpeed;
			}

			Vector3 targetVelocity = player.direction * moveSpeed;
			Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
			player.rb.AddForce(force, ForceMode.Acceleration);
		}
	}

	public override void Exit()
	{
		player.ShowWeapons();
		player.anim.SetBool("Bow", false);
	}
}
