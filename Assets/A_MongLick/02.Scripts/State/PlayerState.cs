using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.OpenVR;
using UnityEngine;

public class PlayerState : BaseState<PlayerController.State>
{
	protected PlayerController oner;
}

public class MoveState : PlayerState
{
	public MoveState(PlayerController oner)
	{
		this.oner = oner;
	}

	public override void FixedUpdate()
	{
		oner.Move();
	}

	public override void Transition()
	{
		if (oner.PlayerInput.actions["Sword"].IsPressed() && oner.PlayerInput.actions["Sword"].triggered)
		{
			ChangeState(PlayerController.State.Sword);
		}

		if (oner.PlayerInput.actions["Dash"].IsPressed() && oner.CanDashCoolTime)
		{
			ChangeState(PlayerController.State.Dash);
		}
	}
}

public class SwordState : PlayerState
{
	float time;

	public SwordState(PlayerController oner)
	{
		this.oner = oner;
	}

	public override void Enter()
	{
		oner.Use();
	}

	public override void Exit()
	{
		time = 0;
	}

	public override void Update()
	{
		time += Time.deltaTime;
	}

	public override void Transition()
	{
		if(time > oner.Weapon.Rate)
		{
			ChangeState(PlayerController.State.Move);
		}
		if (oner.PlayerInput.actions["Dash"].IsPressed() && oner.CanDashCoolTime)
		{
			ChangeState(PlayerController.State.Dash);
		}
	}
}

public class DashState : PlayerState
{
	float time;
	private Coroutine dashRoutine;
	public DashState(PlayerController oner)
	{
		this.oner = oner;
	}

	public override void Enter()
	{
		oner.gameObject.layer = oner.InvincibilityLayer;
		oner.CanDashCoolTime = false;
	}

	public override void FixedUpdate()
	{
		oner.DashMove();
	}

	public override void Exit()
	{
		time = 0;
		oner.gameObject.layer = oner.PlayerLayer;
	}

	public override void Update()
	{
		time += Time.deltaTime;
	}

	public override void Transition()
	{
		if (time > oner.DashTime)
		{
			ChangeState(PlayerController.State.Move);
		}
	}
}




