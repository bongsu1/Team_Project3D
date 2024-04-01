using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Normal, Trace, Attack, Return, Die }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    public enum Type { Slime, Bat, Mummy, Mage }

    [Header("Atrribute")]
    [SerializeField] int id;
    [SerializeField] string name;
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 rotation;
    [Header("Spec")]
    [SerializeField] int hp;
    [SerializeField] int damage;
    [SerializeField] float moveSpeed;

    [Header("Component")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] SphereCollider searchRange;
    [SerializeField] SphereCollider traceRange;
    [SerializeField] Collider attackRange;
    [SerializeField] MeshRenderer[] meshs;

    [Header("Editor")]
    [SerializeField] float searchDistance;
    [SerializeField] float traceDistance;
    [SerializeField] LayerMask searchLayer;
    [SerializeField] float attackDelay;
    [SerializeField] float attackRate;
    [SerializeField] MonsterBullet bullet;
    [SerializeField] Color curColor;
    [SerializeField] Color hurtColor;
    [SerializeField] float groggyTime;

    // test..
    [Header("Attack")]
    [SerializeField] float attackSpeed; // 돌진 스피드
    [SerializeField] float attackDistance; // 공격 거리

    // test..
    private Transform target; // 플레이어 위치

    // property..
    public NavMeshAgent Nav => nav;
    public Transform Target => target;
    public float AttackDistance => attackDistance;

    private void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav.speed = moveSpeed;
        bullet.Damage = damage;
        searchRange.radius = searchDistance;
        traceRange.radius = traceDistance;
    }

    private void Start()
    {
        stateMachine.AddState(State.Normal, new M_NoramlState(this));
        stateMachine.AddState(State.Trace, new M_TraceState(this));
        stateMachine.AddState(State.Attack, new M_AttackState(this));
        stateMachine.AddState(State.Return, new M_ReturnState(this));
        stateMachine.AddState(State.Die, new M_DieState(this));

        stateMachine.Start(State.Normal);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        if (searchRange.enabled)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, searchDistance);
        }

        if (traceRange.enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, traceDistance);
        }
    }

    // test.. 
    int count;
    private void OnTriggerEnter(Collider other)
    {
        if (count > 0)
            return;

        if (((1 << other.gameObject.layer) & searchLayer) != 0)
        {
            count++;
            target = other.GetComponent<TestPlayer>().transform;
            searchRange.enabled = false;
            traceRange.enabled = true;
            stateMachine.ChangeState(State.Trace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((((1 << other.gameObject.layer) & searchLayer) != 0)
            && Vector3.Distance(target.position, transform.position) >= traceDistance)
        {
            count--;
            searchRange.enabled = true;
            traceRange.enabled = false;
            stateMachine.ChangeState(State.Return);
        }
    }

    #region Method

    public virtual void Attack()
    {
        // test..
        StartCoroutine(AttackRotine());
    }

    protected virtual IEnumerator AttackRotine()
    {
        // test..
        float time = 0;
        while (time <= 1)
        {
            Vector3 targetDir = new Vector3((target.position - transform.position).x,
                transform.position.y, (target.position - transform.position).z);
            transform.forward = Vector3.Lerp(transform.forward, target.position - transform.position, time);
            time += Time.deltaTime / attackDelay;
            yield return null;
        }
        attackRange.enabled = true;
        rigid.velocity = transform.forward * attackSpeed + Vector3.up * 3f;

        yield return new WaitForSeconds(0.5f);
        attackRange.enabled = false;

        yield return new WaitForSeconds(attackRate - 0.5f);
        stateMachine.ChangeState(State.Trace);
    }

    // test..
    [ContextMenu("TakeDamage")]
    public void DamageTest()
    {
        TakeDamage(10);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (attackRange.enabled)
        {
            attackRange.enabled = false;
        }
        rigid.velocity = Vector3.zero; // 넉백을 할지 멈출지

        StartCoroutine(HurtRoutine(hp <= 0));
        if (hp <= 0)
        {
            hp = 0;
            stateMachine.ChangeState(State.Die);
            Destroy(gameObject, 4f);
            // 아이템 드랍
        }
    }

    IEnumerator HurtRoutine(bool isDead)
    {
        if (isDead)
        {
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.gray;
            }
            gameObject.layer = 7; // 7 = DeadMonsterLayer
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = hurtColor;
            }
            yield return new WaitForSeconds(0.1f);

            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = curColor;//Color.white;
            }
        }
    }

    #endregion
}
