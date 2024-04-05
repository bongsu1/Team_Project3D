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
                transform.forward = reflectDir; // �ڽ� ��ġ�������� ���� ex) (1,0)������ (-1,0)����
                //transform.LookAt(transform.position + reflectDir); // Ÿ�� ��ġ �������� ���⵹��
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
