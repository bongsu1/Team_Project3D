using System.Collections;
using UnityEngine;

public class Sword : Weapon
{
    [Header("Sword")]
    [SerializeField] Collider attackArea;
    [SerializeField] LayerMask monsterLayer;

    [Header("Attack")]
    [SerializeField] int firstDamage; // ù��°�� �ι�°�� �������� ����
    [SerializeField] int thirdDamage;
    [SerializeField] float attackRate;
    public float AttackRate => attackRate; // ���ݻ��¿� �����ִ� �ð� (attackDelay�� ���ؼ� ���)
    [SerializeField] float secondAttackDelay;
    public float SecondAttackDelay => secondAttackDelay; // �� ��° ���� ���� �ִϸ��̼� �����̰� �ٸ�
    [SerializeField] float thirdAttackRate; // ����° ������ ����
    public float ThirdAttackRate => thirdAttackRate;

    [Header("Effect")]
    [SerializeField] HitEffect hitEffect;

    bool isNormalAttack;

    private void OnEnable()
    {
        hitEffect = Instantiate(Manager.Resource.Load<HitEffect>("Effect/HitEffect"), transform);
    }

    /// <summary>
    /// sequence�� ���° �������� �Է� 
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
