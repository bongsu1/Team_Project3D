using System.Collections;
using UnityEngine;

#region Remake
public class PlayerState : BaseState<Player.State>
{
    protected Player oner;
}

public class NormalState : PlayerState
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

public class FirstSwordState : PlayerState
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

public class SecondSwordState : PlayerState
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

public class ThirdSwordState : PlayerState
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

public class BowState : PlayerState
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

    public BowState(Player oner)
    {
        this.oner = oner;
    }
}


public class DashState : PlayerState
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

    public DashState(Player oner)
    {
        this.oner = oner;
    }
}
#endregion