using System.Collections;
using UnityEngine;

public class Slime : Monster
{
    protected override IEnumerator AttackRotine()
    {
        float time = 0;
        while (time <= 1)
        {
            Vector3 targetDir = new Vector3((target.position - transform.position).x,
                0, (target.position - transform.position).z).normalized;
            transform.forward = Vector3.Lerp(transform.forward, targetDir, time);
            time += Time.deltaTime / attackDelay;
            yield return null;
        }
        rigid.velocity = transform.forward * attackSpeed + Vector3.up * 3f;

        yield return new WaitForSeconds(attackRate);
        onAttack = false;
    }
}
