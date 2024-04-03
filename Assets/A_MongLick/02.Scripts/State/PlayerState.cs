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
		if (oner.PlayerInput.actions["Dash"].IsPressed() && oner.CanDashCoolTime)
		{
			ChangeState(PlayerController.State.Dash);
		}

		else if (oner.PlayerInput.actions["Sword"].IsPressed() && oner.PlayerInput.actions["Sword"].triggered)
		{
			ChangeState(PlayerController.State.Sword);
		}

		else if (oner.PlayerInput.actions["Bow"].IsPressed() && oner.PlayerInput.actions["Bow"].triggered)
		{
			ChangeState(PlayerController.State.Bow);
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
		oner.PlayerSword();
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
		if (time > oner.SwordWeapon.Rate)
		{
			ChangeState(PlayerController.State.Move);
		}
		if (oner.PlayerInput.actions["Dash"].IsPressed() && oner.CanDashCoolTime)
		{
			ChangeState(PlayerController.State.Dash);
		}
	}
}

public class BowState : PlayerState
{
	public BowState(PlayerController oner)
	{
		this.oner = oner;
	}

	public override void Update()
	{
		Ray ray = oner.FollowCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		if (Physics.Raycast(ray, out rayHit, 100))
		{
			Vector3 nextVec = rayHit.point - oner.transform.position;
			nextVec.y = 0;
			oner.transform.LookAt(oner.transform.position + nextVec);
		}

		/*// 라인 시작점 설정
		oner.LineRenderer.SetPosition(0, oner.transform.position);
		RaycastHit hit;
		if (Physics.Raycast(oner.transform.position, oner.transform.forward, out hit, oner.LineSize, oner.BowWeapon.MonsterLayer))
		{
			oner.LineRenderer.positionCount = 2;
			oner.LineRenderer.SetPosition(1, hit.point);
		}
		else if (Physics.Raycast(oner.transform.position, oner.transform.forward, out hit, oner.LineSize, oner.WallLayer) && oner.BowWeapon.BowFirstTime)
		{
			oner.LineRenderer.positionCount = 3;
			oner.LineRenderer.SetPosition(1, hit.point);
			Vector3 NewDir = Vector3.Reflect(oner.transform.forward, hit.normal);

			if (Physics.Raycast(hit.point, NewDir, out hit, oner.LineSize))
			{
				oner.LineRenderer.SetPosition(2, hit.point);
			}
			else
			{
				oner.LineRenderer.SetPosition(2, hit.point + NewDir * oner.LineSize);
			}
		}
		else
		{
			oner.LineRenderer.positionCount = 2;
			// 라인 끝점 설정 (oner.LineSize 범위 내에 아무것도 충돌하지 않았을 때)
			oner.LineRenderer.SetPosition(1, oner.transform.position + oner.transform.forward * oner.LineSize);
			Debug.DrawRay(oner.transform.position, oner.transform.forward * oner.LineSize, Color.yellow);
		}*/

		if (!oner.PlayerInput.actions["Bow"].IsPressed())
		{
			oner.LineRenderer.enabled = false;
		}
		else
		{
			oner.LineRenderer.enabled = true;
		}

		// 라인 시작점 설정
		oner.LineRenderer.positionCount = 2;
		oner.LineRenderer.SetPosition(0, oner.transform.position);
		RaycastHit hit;
		if (Physics.Raycast(oner.transform.position, oner.transform.forward, out hit, oner.LineSize, oner.WallLayer))
		{
			oner.LineRenderer.SetPosition(1, hit.point);
		}
		else
		{
			oner.LineRenderer.SetPosition(1, oner.transform.position + oner.transform.forward * oner.LineSize);
		}

	}

	public override void Transition()
	{

		if (!oner.PlayerInput.actions["Bow"].IsPressed() && oner.PlayerInput.actions["Bow"].triggered)
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
		oner.Rigid.velocity = Vector3.zero;
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




