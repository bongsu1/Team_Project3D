using System.Collections;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] int damage;
    [SerializeField] float rate;

    [Header("Effect")]
    [SerializeField] HitEffect hitEffect;

    private bool canDamage = true;

    public int Damage { get { return damage; } set { damage = value; } }

    private void OnEnable()
    {
        hitEffect = Instantiate(Manager.Resource.Load<HitEffect>("Effect/HitEffect"), transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0
            && canDamage)
        {
            StartCoroutine(AttackRateRoutine());
            IDamagable player = other.GetComponent<IDamagable>();
            player?.TakeDamage(damage);
            hitEffect.PlayHitEffect(other.ClosestPoint(other.transform.position));
        }

        if (((1 << other.gameObject.layer) & collisionLayer) != 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AttackRateRoutine()
    {
        canDamage = false;
        yield return new WaitForSeconds(rate);
        canDamage = true;
    }
}
