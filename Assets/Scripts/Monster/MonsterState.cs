using UnityEngine;

public class MonsterState : BaseState<Monster.State>
{
    protected Monster oner;
}

public class M_NoramlState : MonsterState
{
    public override void Enter()
    {
        switch (oner.type)
        {
            case Monster.Type.Slime:
                break;
            case Monster.Type.Bat:
                break;
            case Monster.Type.Mummy:
                break;
            case Monster.Type.Mage:
                break;
        }
        oner.Nav.enabled = true;
        oner.Rigid.useGravity = false;
    }

    public override void Update()
    {
        oner.Rigid.velocity = Vector3.zero;
        oner.Patrol();
    }

    public override void Exit()
    {
        oner.Nav.enabled = false;
        oner.Rigid.useGravity = true;
    }

    public M_NoramlState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_TraceState : MonsterState
{
    public override void Enter()
    {
        switch (oner.type)
        {
            case Monster.Type.Slime:
                oner.Animator.SetTrigger("DoTrace");
                break;
            case Monster.Type.Bat:
                break;
            case Monster.Type.Mummy:
                break;
            case Monster.Type.Mage:
                break;
        }
        oner.Nav.enabled = true;
        oner.Rigid.useGravity = false;
    }

    public override void Update()
    {
        oner.Rigid.velocity = Vector3.zero;
        oner.Nav.SetDestination(oner.Target.position);
    }

    public override void Exit()
    {
        oner.Nav.enabled = false;
        oner.Rigid.useGravity = true;
    }
    public override void Transition()
    {
        if (Vector3.Distance(oner.transform.position, oner.Target.position) < oner.AttackDistance)
        {
            ChangeState(Monster.State.Attack);
        }
        else if (!oner.IsCamp)
        {
            ChangeState(Monster.State.Normal);
        }
    }

    public M_TraceState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_AttackState : MonsterState
{
    public override void Enter()
    {
        oner.Attack();
    }

    public override void Transition()
    {
        if (!oner.OnAttack && oner.IsCamp)
        {
            ChangeState(Monster.State.Trace);
        }
        else if (!oner.OnAttack && !oner.IsCamp)
        {
            ChangeState(Monster.State.Normal);
        }
    }

    public M_AttackState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_DieState : MonsterState
{
    public override void Enter()
    {
        oner.Rigid.velocity = Vector3.zero;
        switch (oner.type)
        {
            case Monster.Type.Slime:
                oner.Animator.Play("Die");
                break;
            case Monster.Type.Bat:
                break;
            case Monster.Type.Mummy:
                break;
            case Monster.Type.Mage:
                break;
        }
    }

    public M_DieState(Monster oner)
    {
        this.oner = oner;
    }
}
