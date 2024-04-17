using System.Collections;
using UnityEngine;

public class Mummy : Monster
{
    [Header("Mummy")]
    [SerializeField] Collider attackRange;

    protected override IEnumerator AttackRoutine()
    {
        animator.SetInteger("Random", Random.Range(0, 3));
        animator.SetTrigger("DoAttack");
        float time = 0;
        while (time <= 1)
        {
            Vector3 targetDir = new Vector3((target.position - transform.position).x,
                0, (target.position - transform.position).z).normalized;
            transform.forward = Vector3.Lerp(transform.forward, targetDir, time);
            time += Time.deltaTime / attackDelay;
            yield return null;
        }

        attackRange.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackRange.enabled = false;

        yield return new WaitForSeconds(attackRate);
        onAttack = false;
    }
}
