using System.Collections;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask collisionLayer;
    [SerializeField] int damage;
    [SerializeField] float rate;

    private bool canDamage = true;

    public int Damage { get { return damage; } set { damage = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0
            && canDamage)
        {
            StartCoroutine(AttackRateRoutine());
            IDamagable player = other.GetComponent<IDamagable>();
            player?.TakeDamage(damage);
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
