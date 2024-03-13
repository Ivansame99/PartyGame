using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/DropAttack")]
public class PlayerDropAttackState : PlayerState<PlayerController>
{
	[SerializeField]
	private float dropAttackBaseDamage;

	[SerializeField]
	private GameObject dropAttackParticle;

	[SerializeField]
	private float fallSped;

	[SerializeField]
	private float prepareAttackTime;

	[SerializeField]
	private float attackColliderDuration;

	[SerializeField]
	private float attackFreezeTime;

	[SerializeField]
	private float turnSmooth;

	[SerializeField]
	private ParticleSystem groundHit;

	private float timerPrepareAttack;
	private float timerattackCollider;
	private float timerattackFreeze;

	private bool fallParticleOn;
	private float originalGravityScale;
	private SlashController jumpAttackController;
	private float turnSmoothTime;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.gravityController.gravityOn = false;
		timerPrepareAttack = 0;
		timerattackCollider = 0;
		timerattackFreeze = 0;
		fallParticleOn = false;
		originalGravityScale = player.gravityController.gravityScale;
		player.anim.SetTrigger("JumpDrop");
		jumpAttackController = player.jumpAttackCollider.GetComponent<SlashController>();
		ResetVelocity();
	}

	public override void Exit()
	{
		
	}

	public override void FixedUpdate()
	{
		if (!player.groundCheck.DetectGround())
		{
			Vector3 currentVelocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

			if (currentVelocity.magnitude > 10)
			{
				currentVelocity = currentVelocity.normalized * 10;
			}

			Vector3 targetVelocity = player.direction * 10;
			Vector3 force = (targetVelocity - currentVelocity) / Time.fixedDeltaTime;
			player.rb.AddForce(force, ForceMode.Acceleration);
		}
	}

	public override void Update()
	{
		if (player.direction != Vector3.zero && !player.groundCheck.DetectGround())
		{
			float targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
			float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
			if (!float.IsNaN(angle))
			{
				var rotation = Quaternion.Euler(0f, angle, 0f);
				player.transform.rotation = rotation;
			}
		}

		if (timerPrepareAttack >= prepareAttackTime)
		{
			player.gravityController.gravityOn = true;
			player.gravityController.gravityScale = fallSped;
			if (!player.groundCheck.DetectGround())
			{
				return;
			}

			jumpAttackController.finalDamage = dropAttackBaseDamage + player.powerController.PowerDamage();
			player.jumpAttackCollider.SetActive(true);

			if (!fallParticleOn)
			{
				Instantiate(groundHit, player.transform.position, groundHit.transform.rotation);
				//groundHit.Play();
				Instantiate(dropAttackParticle, player.transform.position, dropAttackParticle.transform.rotation);
				fallParticleOn = true;
			}

			player.gravityController.gravityScale = originalGravityScale;

			if (timerattackCollider >= attackColliderDuration)
			{
				player.jumpAttackCollider.SetActive(false);
				if (timerattackFreeze >= attackFreezeTime)
				{
					player.ChangeState(typeof(PlayerIdleState));
					return;
				}
				else
				{
					timerattackFreeze += Time.deltaTime;
					return;
				}
			}
			else
			{
				timerattackCollider += Time.deltaTime;
				return;
			}

		}
		else
		{
			timerPrepareAttack += Time.deltaTime;
		}
	}

	private void ResetVelocity()
	{
		player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
	}
}
