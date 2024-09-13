using System.Collections;
using UnityEngine;

public class Sword : Weapon
{
    [Header("Sword")]
    [SerializeField] Collider attackArea;
    [SerializeField] LayerMask monsterLayer;

    [Header("Attack")]
    [SerializeField] int firstDamage; // 첫번째와 두번째는 데미지가 같음
    [SerializeField] int thirdDamage;
    [SerializeField] float attackRate;
    public float AttackRate => attackRate; // 공격상태에 남아있는 시간 (attackDelay와 더해서 계산)
    [SerializeField] float secondAttackDelay;
    public float SecondAttackDelay => secondAttackDelay; // 두 번째 공격 부터 애니메이션 딜레이가 다름
    [SerializeField] float thirdAttackRate; // 세번째 공격후 경직
    public float ThirdAttackRate => thirdAttackRate;

    [Header("Effect")]
    [SerializeField] HitEffect hitEffect;

    bool isNormalAttack;

    private void OnEnable()
    {
        hitEffect = Instantiate(Manager.Resource.Load<HitEffect>("Effect/HitEffect"), transform);
    }

    /// <summary>
    /// sequence에 몇번째 공격인지 입력 
    /// </summary>
    /// <param name="sequence"></param>
    public void Use(int sequence)
    {
        switch (sequence)
        {
            case 1:
            case 2:
                StartCoroutine(AttackRoutine());
                break;
            case 3:
                StartCoroutine(ThirdAttackRoutine());
                break;
            default: // do nothing
                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        isNormalAttack = true;
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
    }

    IEnumerator ThirdAttackRoutine()
    {
        isNormalAttack = false;
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & monsterLayer) != 0)
        {
            IDamagable monster = other.GetComponent<IDamagable>();
            monster?.TakeDamage(isNormalAttack ? firstDamage : thirdDamage);
            hitEffect.PlayHitEffect(other.ClosestPoint(other.transform.position));
        }
    }
}
