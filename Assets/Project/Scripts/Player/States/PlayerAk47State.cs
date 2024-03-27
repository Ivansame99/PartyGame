using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Player/Ak47")]
public class PlayerAk47State : PlayerState<PlayerController>
{
	public override void Init(PlayerController p)
	{
		base.Init(p);
		player.anim.SetBool("Bow", true);
		player.EquipAk();
		Debug.Log("Estado AK47");
	}

	public override void Update()
	{
		throw new System.NotImplementedException();
	}

	public override void FixedUpdate()
	{
		throw new System.NotImplementedException();
	}

	public override void Exit()
	{
		throw new System.NotImplementedException();
	}
}
