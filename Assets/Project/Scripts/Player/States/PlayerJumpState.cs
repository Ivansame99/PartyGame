using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Jump")]
public class PlayerJumpState : PlayerState<PlayerController>
{
	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private GameObject jumpParticles;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.transform.DOPunchScale(new Vector3(1f, -1f, 1f), 0.3f).SetRelative(true).SetEase(Ease.OutBack);
		player.isJumping = false;
		Instantiate(jumpParticles, player.transform.position, jumpParticles.transform.rotation);
		player.playerAudioManager.PlayJump();
	}

	public override void Exit()
	{
		player.transform.DORewind();
		player.transform.DOPunchScale(new Vector3(1, 1, 1), .25f);
	}

	public override void FixedUpdate()
	{
		player.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		player.ChangeState(typeof(PlayerIdleState));
	}

	public override void Update()
	{

	}
}
