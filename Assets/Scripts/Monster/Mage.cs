using System.Collections;
using UnityEngine;

public class Mage : Monster
{
    [Header("Mage")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject castBox; // 캐스팅하면서 보일 것

    Coroutine attackRoutine;
    public override void Attack()
    {
        onAttack = true;
        attackRoutine = StartCoroutine(AttackRotine());
    }

    protected override IEnumerator AttackRotine()
    {
        animator.SetTrigger("DoAttack");
        float time = 0;
        while (time <= 1)
        {
            castBox.SetActive(true);
            castBox.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.5f, time);

            Vector3 targetDir = new Vector3((target.position - transform.position).x,
                0, (target.position - transform.position).z).normalized;
            transform.forward = Vector3.Lerp(transform.forward, targetDir, time);
            time += Time.deltaTime / attackDelay;
            yield return null;
        }
        castBox.SetActive(false);

        Rigidbody bulletRigid = Instantiate(bullet, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * attackSpeed;
        yield return new WaitForSeconds(attackRate);

        onAttack = false;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (isDead)
        {
            StopCoroutine(attackRoutine);
            castBox.SetActive(false);
        }
    }
}
