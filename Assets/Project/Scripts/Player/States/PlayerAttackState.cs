using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Attack")]
public class PlayerAttackState : PlayerState<PlayerController>
{
	[SerializeField]
	private float betweenAttacksCD;

	[SerializeField]
	private float comboCD;

	[SerializeField]
	private float invokeEndComboTime;

	[SerializeField]
	private float airFriction;

	[SerializeField]
	private float turnSmooth;

	[SerializeField]
	private float minPitch;

	[SerializeField]
	private float maxPitch;

	private bool moveAttack;
	private float attackMovement;
	private int comboCounter;
	float lastClicked;
	private float turnSmoothTime;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		comboCounter = 0;
		lastClicked = betweenAttacksCD;
	}

	public override void Exit()
	{
		player.anim.ResetTrigger("Attack");
		ResetVelocity();
		player.gravityController.gravityOn = true;
		player.lastComboTimer = comboCD;
	}

	public override void FixedUpdate()
	{
		if (moveAttack)
		{
			player.rb.AddForce(player.transform.forward * attackMovement, ForceMode.Impulse);
			moveAttack = false;
		}
	}

	public override void Update()
	{
		//Change to roll
		if (player.isDodging && player.dodgeTimer <= 0)
		{
			player.ChangeState(typeof(PlayerRollState));
			return;
		}

		Attack();
		ExitAttack();
	}

	private void Attack()
	{
		if (player.attackBuffer.Count >= 1)
		{
			if (comboCounter < player.weaponController.combo.Count)
			{
				if (Time.time - lastClicked >= betweenAttacksCD)
				{
					player.CancelInvoke(nameof(player.EndCombo));

					ResetVelocity();

					//Sound
					player.anim.runtimeAnimatorController = player.weaponController.combo[comboCounter].animatorOR;
					player.anim.Play("Attack", 0, 0);
					player.swordAttackSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

					//Animation
					player.swordAttackSound.Play();
					player.transform.DOPunchScale(new Vector3(0.6f, -0.6f, 0.6f), 0.6f).SetRelative(true).SetEase(Ease.OutBack);

					//Insert damage and pushforce
					player.slashCollider.finalDamage = player.weaponController.combo[comboCounter].damage + +player.powerController.PowerDamage(); //Cambiar escalado poder
					player.slashCollider.pushForce = player.weaponController.combo[comboCounter].pushForce;

					//Move player when attack
					if (player.direction.magnitude >= 0.1f)
					{
						float targetAngle;
						targetAngle = Mathf.Atan2(player.direction.x, player.direction.z) * Mathf.Rad2Deg;
						float angle = Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmoothTime);
						player.transform.rotation = Quaternion.Euler(0f, angle, 0f);
						if (player.groundCheck.DetectGround()) attackMovement = player.weaponController.combo[comboCounter].attackMovement;
						else attackMovement = player.weaponController.combo[comboCounter].attackMovement * airFriction;
						moveAttack = true;
					}

					Transform target = TryGetNearestEnemy();

					if (target != null)
					{
						Vector3 direction = target.position - player.transform.position;
						direction.y = 0;
						Quaternion rotation = Quaternion.LookRotation(direction);
						player.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
						if (player.groundCheck.DetectGround()) attackMovement = player.weaponController.combo[comboCounter].attackMovement;
						else attackMovement = player.weaponController.combo[comboCounter].attackMovement * airFriction;
						moveAttack = true;
					}


					comboCounter++;

					player.gravityController.gravityOn = false;
					lastClicked = Time.time;
				}
			}
		}
	}

	private void ExitAttack()
	{
		if (player.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f && player.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
			player.anim.SetTrigger("Attack");
			ResetVelocity();
			player.gravityController.gravityOn = true;
			player.Invoke(nameof(player.EndCombo), invokeEndComboTime);
		}
	}

	private void ResetVelocity()
	{
		player.rb.velocity = new Vector3(0, player.rb.velocity.y, 0);
	}

	private Transform TryGetNearestEnemy()
	{
		Transform nearestEnemy = null;
		float nearestEnemyDistance = float.MaxValue;

		if (player.detectEnemiesNear.enemiesNear.Count >= 1)
		{
			foreach (GameObject enemy in player.detectEnemiesNear.enemiesNear)
			{
				if (enemy == null)
				{
					player.detectEnemiesNear.enemiesNear.Remove(enemy);
					return null;
				}

				Vector3 enemyDistanceDiff = enemy.transform.position - player.transform.position;
				float enemyDistance = enemyDistanceDiff.sqrMagnitude;

				if (enemyDistance < nearestEnemyDistance)
				{
					nearestEnemyDistance = enemyDistance;
					nearestEnemy = enemy.transform;
				}
			}
		}

		return nearestEnemy;
	}
}
