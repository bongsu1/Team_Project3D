using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] LayerMask playerlayer;
    [SerializeField] int damage;

    HitEffect hitEffect;
    private void Start()
    {
        hitEffect = Instantiate(Manager.Resource.Load<HitEffect>("Effect/HitEffect"), transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerlayer) != 0)
        {
            IDamagable player = other.GetComponent<IDamagable>();
            player?.TakeDamage(damage);
            hitEffect.PlayHitEffect(other.ClosestPoint(other.transform.position));
        }
    }
}
