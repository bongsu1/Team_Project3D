using System.Collections;
using UnityEngine;

public class Bat : Monster
{
    [Header("Bat")]
    [SerializeField] float backMoveDistance;

    Vector3 curPos;

    protected override IEnumerator AttackRotine()
    {
        Debug.Log("Attack");
        curPos = transform.position;

        Vector3 targetDir = new Vector3((target.position - transform.position).x,
            0, (target.position - transform.position).z);
        transform.forward = targetDir;
        float time = 0;
        while (time <= 1)
        {
            rigid.velocity = -transform.forward;
            time += Time.deltaTime / attackDelay;
            yield return null;
        }

        rigid.velocity = transform.forward * attackSpeed;
        yield return new WaitForSeconds(0.5f);
        rigid.velocity = Vector3.zero;
        time = 0;
        while (time <= 1)
        {
            Vector3 movePos = Vector3.Lerp(transform.position, curPos, time);
            transform.position = movePos;
            time += Time.deltaTime / attackRate;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        onAttack = false;
    }

    public void ChangeTraceState(Transform target)
    {
        this.target = target;
        stateMachine.ChangeState(State.Trace);
    }
}
