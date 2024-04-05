using System.Collections;
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
        oner.Move();//...
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
        if (time > oner.SwordWeapon.AttackDelay)
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

    public override void Enter()
    {
        oner.LineRenderer.enabled = true;
        oner.BowWeapon.Bow(oner.PlayerInput.actions["Bow"].IsPressed());
    }

    public override void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100, oner.GroundLayer))
        {
            Vector3 nextVec = rayHit.point - oner.transform.position;
            nextVec.y = 0;
            oner.transform.LookAt(oner.transform.position + nextVec);
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

    public override void Exit()
    {
        oner.LineRenderer.enabled = false;
    }

    public override void Transition()
    {

        if (!oner.PlayerInput.actions["Bow"].IsPressed() && oner.PlayerInput.actions["Bow"].triggered)
        {
            oner.BowWeapon.Bow(oner.PlayerInput.actions["Bow"].IsPressed());
            ChangeState(PlayerController.State.Move);
        }

        if (oner.PlayerInput.actions["Dash"].IsPressed() && oner.CanDashCoolTime)
        {
            oner.BowWeapon.Cancel();
            Debug.Log("캔슬시킴");
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

        if (oner.MoveDir == Vector3.zero)
        {
            oner.DashVec = oner.transform.forward;
        }
        else
        {
            oner.DashVec = oner.MoveDir;
        }
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
        oner.StartCoroutine(DashCoolTime(1f));
    }

    IEnumerator DashCoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        oner.CanDashCoolTime = true;
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



#region Remake
public class RePlayerState : BaseState<Player.State>
{
    protected Player oner;
}

public class NormalState : RePlayerState
{
    public override void FixedUpdate()
    {
        oner.Move();
    }

    public override void LateUpdate()
    {
        oner.Turn(true);
    }

    public override void Exit()
    {
        oner.Rigid.velocity = Vector3.zero;
    }

    public override void Transition()
    {
        if (oner.Input.actions["Dash"].IsPressed() && oner.CanDash)
        {
            ChangeState(Player.State.Dash);
        }
        else if (oner.Input.actions["Sword"].IsPressed())
        {
            ChangeState(Player.State.FirstSword);
        }
        else if (oner.Input.actions["Bow"].IsPressed() && oner.Bow.CanShot)
        {
            ChangeState(Player.State.Bow);
        }
    }

    public NormalState(Player oner)
    {
        this.oner = oner;
    }
}

public class FirstSwordState : RePlayerState
{
    bool isAttack;
    bool canSecondAttack;
    Coroutine attackRoutine;

    public override void Enter()
    {
        oner.Turn(false);
        isAttack = true;
        attackRoutine = oner.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        canSecondAttack = false;
        isAttack = false;
    }

    public override void Transition()
    {
        if (oner.Input.actions["Dash"].IsPressed())
        {
            oner.StopCoroutine(attackRoutine);
            attackRoutine = null;
            ChangeState(Player.State.Dash);
        }
        else if (oner.Input.actions["Sword"].IsPressed() && canSecondAttack && isAttack)
        {
            oner.StopCoroutine(attackRoutine);
            attackRoutine = null;
            ChangeState(Player.State.SecondSword);
        }
        else if (!isAttack)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(oner.Sword.AttackDelay);
        oner.Sword.Use(1);
        canSecondAttack = true;
        yield return new WaitForSeconds(oner.Sword.AttackRate);
        isAttack = false;
    }

    public FirstSwordState(Player oner)
    {
        this.oner = oner;
    }
}

public class SecondSwordState : RePlayerState
{
    bool isAttack;
    bool canThirdAttack;
    Coroutine attackRoutine;

    public override void Enter()
    {
        oner.Turn(false);
        isAttack = true;
        attackRoutine = oner.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        canThirdAttack = false;
        isAttack = false;
    }

    public override void Transition()
    {
        if (oner.Input.actions["Dash"].IsPressed())
        {
            oner.StopCoroutine(attackRoutine);
            attackRoutine = null;
            ChangeState(Player.State.Dash);
        }
        else if (oner.Input.actions["Sword"].IsPressed() && canThirdAttack && isAttack)
        {
            oner.StopCoroutine(attackRoutine);
            attackRoutine = null;
            ChangeState(Player.State.ThirdSword);
        }
        else if (!isAttack)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(oner.Sword.AttackDelay);
        oner.Sword.Use(2);
        canThirdAttack = true;
        yield return new WaitForSeconds(oner.Sword.AttackRate);
        isAttack = false;
    }

    public SecondSwordState(Player oner)
    {
        this.oner = oner;
    }
}

public class ThirdSwordState : RePlayerState
{
    bool isAttack;
    Coroutine attackRoutine;

    public override void Enter()
    {
        oner.Turn(false);
        isAttack = true;
        attackRoutine = oner.StartCoroutine(AttackRoutine());
    }

    public override void Exit()
    {
        isAttack = false;
    }

    public override void Transition()
    {
        if (oner.Input.actions["Dash"].IsPressed())
        {
            oner.StopCoroutine(attackRoutine);
            attackRoutine = null;
            ChangeState(Player.State.Dash);
        }
        else if (!isAttack)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(oner.Sword.AttackDelay);
        oner.Sword.Use(3);
        yield return new WaitForSeconds(oner.Sword.ThirdAttackRate);
        isAttack = false;
    }

    public ThirdSwordState(Player oner)
    {
        this.oner = oner;
    }
}

public class ReBowState : RePlayerState
{
    int charged;
    Coroutine chargingRoutine;

    public override void Enter()
    {
        oner.Bow.Use(oner.Input.actions["Bow"].IsPressed(), charged);
        chargingRoutine = oner.StartCoroutine(ChargingRoutine());
    }

    public override void FixedUpdate()
    {
        oner.Turn(false);
    }

    public override void Exit()
    {
        charged = 0;
    }

    public override void Transition()
    {
        if (oner.Input.actions["Dash"].IsPressed())
        {
            // 차징 캔슬
            oner.StopCoroutine(chargingRoutine);
            ChangeState(Player.State.Dash);
        }
        else if (!oner.Input.actions["Bow"].IsPressed() && oner.Input.actions["Bow"].triggered)
        {
            oner.Bow.Use(oner.Input.actions["Bow"].IsPressed(), charged);
            oner.StopCoroutine(chargingRoutine);
            oner.StartCoroutine(CoolDownRoutine());
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator ChargingRoutine()
    {
        yield return new WaitForSeconds(oner.Bow.ChargedTime);
        charged++;
        yield return new WaitForSeconds(oner.Bow.ChargedTime);
        charged++;
    }

    IEnumerator CoolDownRoutine()
    {
        oner.Bow.CanShot = false;
        yield return new WaitForSeconds(oner.Bow.AttackDelay);
        oner.Bow.CanShot = true;
    }

    public ReBowState(Player oner)
    {
        this.oner = oner;
    }
}


public class ReDashState : RePlayerState
{
    bool isDash;

    public override void Enter()
    {
        isDash = true;
        oner.CanDash = false;
        oner.gameObject.layer = 22; // 22 = 무적레이어
        if (oner.MoveDir != Vector3.zero)
        {
            oner.DashDir = oner.MoveDir;
            oner.transform.forward = oner.MoveDir;
        }
        else // 방향키 입력이 없으면 바라보고 있는 방향으로 대시
        {
            oner.DashDir = oner.transform.forward;
        }
        oner.StartCoroutine(DashRoutine());
    }

    public override void FixedUpdate()
    {
        oner.Dash();
    }

    public override void Exit()
    {
        oner.gameObject.layer = 3; // 3 = 플레이어 레이어
        oner.Rigid.velocity = Vector3.zero;
    }

    public override void Transition()
    {
        if (!isDash)
        {
            ChangeState(Player.State.Normal);
        }
    }

    IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(oner.DashTime);
        isDash = false;
        yield return new WaitForSeconds(oner.DashCoolTime);
        oner.CanDash = true;
    }

    public ReDashState(Player oner)
    {
        this.oner = oner;
    }
}
#endregion