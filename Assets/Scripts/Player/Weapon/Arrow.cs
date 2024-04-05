using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] LayerMask monsterLayer;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Rigidbody rigid;
    [SerializeField] int bouncedDamage;

    int damage;
    public int Damage { set { damage = value; } }
    int bounceCount;
    public int BounceCount { set { bounceCount = value; } }
    float arrowSpeed;
    public float ArrowSpeed { set { arrowSpeed = value; } }
    float arrowRange;
    public float ArrowRange { set { arrowRange = value; } }

    private void Start()
    {
        Destroy(gameObject, arrowRange / arrowSpeed);
    }

    private void Update()
    {
        rigid.velocity = transform.forward * arrowSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & monsterLayer) != 0)
        {
            IDamagable monster = other.GetComponent<IDamagable>();
            monster?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & monsterLayer) != 0)
        {
            Rigidbody monsterRigid = collision.gameObject.GetComponent<Rigidbody>();
            if (monsterRigid != null)
            {
                monsterRigid.velocity = Vector3.zero;
            }
        }

        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            if (bounceCount > 0)
            {
                bounceCount--;
                damage += bouncedDamage;

                Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
                transform.forward = reflectDir; // 자신 위치기준으로 돌림 ex) (1,0)오른쪽 (-1,0)왼쪽
                //transform.LookAt(transform.position + reflectDir); // 타겟 위치 기준으로 방향돌림
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
