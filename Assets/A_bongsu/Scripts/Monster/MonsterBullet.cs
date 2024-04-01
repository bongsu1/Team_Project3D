using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            IDamagable player = other.GetComponent<IDamagable>();
            player?.TakeDamage(damage);
        }
    }
}
