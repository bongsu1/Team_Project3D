using UnityEngine;

public class MonsterState : BaseState<Monster.State>
{
    protected Monster oner;
}

public class M_NoramlState : MonsterState
{
    // update Ȱ������������ �̵� ����

    public M_NoramlState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_TraceState : MonsterState
{
    public override void Enter()
    {
        oner.Nav.enabled = true;
    }

    public override void Update()
    {
        oner.Nav.SetDestination(oner.Target.position);
    }

    public override void Exit()
    {
        oner.Nav.enabled = false;
    }
    public override void Transition()
    {
        if (Vector3.Distance(oner.transform.position, oner.Target.position) < oner.AttackDistance)
        {
            ChangeState(Monster.State.Attack);
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

    public M_AttackState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_ReturnState : MonsterState
{
    // ������ ����� ���� ��ġ�� ���ư��� ��� ���� ����
    // ������ ����� ���� ���� ������ �װ��� ����� ���ư���
    // �÷��̾ �־����� ���� �÷��̾ ���� �Ÿ��� ����� ���ư��� 

    public M_ReturnState(Monster oner)
    {
        this.oner = oner;
    }
}

public class M_DieState : MonsterState
{
    public M_DieState(Monster oner)
    {
        this.oner = oner;
    }
}
