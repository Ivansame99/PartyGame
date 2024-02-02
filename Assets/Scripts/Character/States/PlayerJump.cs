using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Jump")]
public class PlayerJump : PlayerState<PlayerController>
{
	[SerializeField]
	private float jumpForce;

	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.transform.DOPunchScale(new Vector3(1f, -1f, 1f), 0.7f).SetRelative(true).SetEase(Ease.OutBack);
	}

	public override void Exit()
	{

	}

	public override void FixedUpdate()
	{
		player.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	}

	public override void Update()
	{

	}
}
