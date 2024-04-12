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
        oner.Animator.SetFloat("MoveSpeed", oner.MoveDir.magnitude);
        oner.Move();
    }

    public override void LateUpdate()
    {
        oner.Turn(true);
    }

    public override void Exit()
    {
        oner.Animator.SetFloat("MoveSpeed", 0f);

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
        oner.IsSword = true;
        oner.Animator.Play("Combo1");

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
        oner.Effect.PlayEffect(PlayerEffect.E_Type.FirstAttack);
        oner.Sound.PlaySound(PlayerSound.S_Type.FirstAttack);
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
        oner.Animator.Play("Combo2");

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
        yield return new WaitForSeconds(oner.Sword.SecondAttackDelay);
        oner.Sword.Use(2);
        oner.Effect.PlayEffect(PlayerEffect.E_Type.SecondAttack);
        oner.Sound.PlaySound(PlayerSound.S_Type.FirstAttack);
        yield return new WaitForSeconds(oner.Sword.AttackDelay - oner.Sword.SecondAttackDelay);
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
        oner.Animator.Play("Combo3");

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
        yield return new WaitForSeconds(oner.Sword.SecondAttackDelay);
        oner.Sword.Use(3);
        oner.Effect.PlayEffect(PlayerEffect.E_Type.ThirdAttack);
        oner.Sound.PlaySound(PlayerSound.S_Type.ThirdAttack);
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
    int shotCount;
    int charged;
    Coroutine chargingRoutine;

    public override void Enter()
    {
        shotCount = 1;
        oner.IsSword = false;
        oner.Animator.Play("Sniping_In");
        oner.Sound.PlaySound(PlayerSound.S_Type.Bowstring);

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
            oner.Bow.Cancel();
            Manager.Sound.StopSFX();
            oner.StopCoroutine(chargingRoutine);
            oner.Effect.StopChargeEffect();
            ChangeState(Player.State.Dash);
        }
        else if (!oner.Input.actions["Bow"].IsPressed() && oner.Input.actions["Bow"].triggered && shotCount > 0)
        {
            shotCount--;
            oner.StartCoroutine(ShotRoutine());
            oner.Effect.StopChargeEffect();

            oner.Bow.Use(oner.Input.actions["Bow"].IsPressed(), charged);
            if (charged == 1)
                oner.Effect.PlayEffect(PlayerEffect.E_Type.ChargedShot);
            else if (charged == 2)
                oner.Effect.PlayEffect(PlayerEffect.E_Type.FullChargeShot);
            Manager.Sound.StopSFX();
            oner.Sound.PlaySound(PlayerSound.S_Type.ArrowShot);

            oner.StopCoroutine(chargingRoutine);
            oner.StartCoroutine(CoolDownRoutine());
        }
    }

    IEnumerator ChargingRoutine()
    {
        yield return new WaitForSeconds(oner.Bow.ChargedTime);
        charged++;
        oner.Effect.PlayEffect(PlayerEffect.E_Type.ChargedBow);
        yield return new WaitForSeconds(oner.Bow.ChargedTime);
        charged++;
        oner.Effect.FullChargeEffect();
    }

    IEnumerator CoolDownRoutine()
    {
        oner.Bow.CanShot = false;
        yield return new WaitForSeconds(oner.Bow.AttackDelay);
        oner.Bow.CanShot = true;
    }

    IEnumerator ShotRoutine()// 애니메이션 전용 코루틴
    {
        oner.Animator.Play("Sniping_Out");
        yield return new WaitForSeconds(0.5f);
        ChangeState(Player.State.Normal);
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
        oner.Animator.Play("Dash");

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
        oner.Effect.PlayEffect(PlayerEffect.E_Type.Dash);
        oner.Sound.PlaySound(PlayerSound.S_Type.Dash);
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