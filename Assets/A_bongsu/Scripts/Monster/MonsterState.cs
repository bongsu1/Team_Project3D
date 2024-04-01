using UnityEngine;

public class MonsterState : BaseState<Monster.State>
{
    protected Monster oner;
}

public class M_NoramlState : MonsterState
{
    // update 활동범위내에서 이동 구현

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
    // 범위를 벗어나면 원래 위치로 돌아가는 기능 구현 예정
    // 구역을 벗어나는 경우는 구역 지정후 그곳을 벗어나면 돌아가기
    // 플레이어가 멀어지는 경우는 플레이어가 일정 거리를 벗어나면 돌아가기 

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
