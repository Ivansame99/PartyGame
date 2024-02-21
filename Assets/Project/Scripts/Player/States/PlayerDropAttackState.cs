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

	private float timerPrepareAttack;
	private float timerattackCollider;
	private float timerattackFreeze;

	private bool fallParticleOn;
	private float originalGravityScale;
	private SlashController jumpAttackController;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.gravityController.gravityOn = false;
		timerPrepareAttack = 0;
		timerattackCollider = 0;
		timerattackFreeze = 0;
		fallParticleOn = false;
		originalGravityScale = player.gravityController.gravityScale;
		jumpAttackController = player.jumpAttackCollider.GetComponent<SlashController>();
		ResetVelocity();
	}

	public override void Exit()
	{
		
	}

	public override void FixedUpdate()
	{
		
	}

	public override void Update()
	{
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
