using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Monster : MonoBehaviour, IDamagable
{
    public enum State { Normal, Trace, Attack, Die }
    protected StateMachine<State> stateMachine = new StateMachine<State>();

    public enum Type { Slime, Bat, Mummy, Mage }
    public Type type;

    [Header("Test")]
    [SerializeField] string curState;

    [Header("Spec")]
    [SerializeField] int hp;
    [SerializeField] int damage;
    [SerializeField] float moveSpeed;

    [Header("Patrol")]
    [SerializeField] LayerMask searchLayer;
    [SerializeField] float searchDistance;
    [SerializeField] Transform[] patrolPoint; // 순찰지역
    [SerializeField] LayerMask baseCampLayer;

    [Header("Component")]
    [SerializeField] SphereCollider searchRange;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] protected Animator animator;

    [Header("Attack")]
    [SerializeField] protected MonsterBullet bullet;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float attackSpeed; // 돌진 스피드, 원거리면 총알 속도
    [SerializeField] float attackDistance; // 공격 사거리

    [Header("Hurt")]
    [SerializeField] Color curColor;
    [SerializeField] Color hurtColor;
    [SerializeField] bool canKnockback;

    [Header("HP Bar")]
    [SerializeField] HPBar hpBar;

    public UnityAction OnDead; // 죽었을 때 씬에 전달
    public UnityAction<float> OnChangeHP; // 체력바

    protected Transform target; // 플레이어 위치
    protected bool isCamp = true;
    protected bool onAttack;
    public bool OnAttack => onAttack;
    protected bool isDead;
    private int maxHp;

    // property..
    public NavMeshAgent Nav => nav;
    public Transform Target => target;
    public Rigidbody Rigid => rigid;
    public Animator Animator => animator;
    public float SearchDistance { get { return searchDistance; } set { searchDistance = value; } }
    public float AttackDistance => attackDistance;
    public bool IsCamp => isCamp;
    public bool IsDead => isDead;

    private void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav.speed = moveSpeed;
        bullet.Damage = damage;
        if (type != Type.Bat)
        {
            searchRange.radius = searchDistance;
        }
    }

    protected virtual void Start()
    {
        maxHp = hp;

        stateMachine.AddState(State.Normal, new M_NoramlState(this));
        stateMachine.AddState(State.Trace, new M_TraceState(this));
        stateMachine.AddState(State.Attack, new M_AttackState(this));
        stateMachine.AddState(State.Die, new M_DieState(this));

        stateMachine.Start(State.Normal);
    }

    private void Update()
    {
        curState = stateMachine.CurState;
        if (isDead) return;
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        stateMachine.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        if (searchRange.enabled)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(searchRange.transform.position, searchDistance * transform.localScale.x);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }

    int targetCount;
    private void OnTriggerEnter(Collider other)
    {
        // 박쥐는 집단 행동으로 개체간 트리거는 무시
        if (type == Type.Bat)
            return;

        if (((1 << other.gameObject.layer) & baseCampLayer) != 0 && !isCamp)
        {
            StartCoroutine(ComebackCampRoutine());
        }

        if (targetCount > 0 || !isCamp)
            return;

        if (((1 << other.gameObject.layer) & searchLayer) != 0)
        {
            targetCount++;
            target = other.transform;
            searchRange.enabled = false;
            stateMachine.ChangeState(State.Trace);
        }
    }

    IEnumerator ComebackCampRoutine()
    {
        yield return new WaitForSeconds(2f);
        searchRange.enabled = true;
        isCamp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // 박쥐는 집단 행동으로 개체간 트리거는 무시
        if (type == Type.Bat)
            return;

        if (((1 << other.gameObject.layer) & baseCampLayer) != 0)
        {
            isCamp = false;
            targetCount--;
        }
    }

    #region Method

    int patrolIndex;
    public void Patrol()
    {
        if (patrolPoint.Length > 0)
        {
            if (!nav.enabled)
                return;

            nav.SetDestination(patrolPoint[patrolIndex].position);
            if (Vector3.Distance(transform.position, patrolPoint[patrolIndex].transform.position) <= 0.5f)
            {
                patrolIndex++;
                if (patrolIndex == patrolPoint.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
    }

    public virtual void Attack()
    {
        // test..
        onAttack = true;
        StartCoroutine(AttackRoutine());
    }

    protected virtual IEnumerator AttackRoutine()
    {
        yield return null;
    }

    // test..
    [Header("HP Bar Test")]
    public bool hpBarInvisibleMode; // 맞고 시간이 지나면 체력 바가 사라지는 모드
    public virtual void TakeDamage(int damage)
    {
        if (hpBar == null)
        {
            GameObject hpBarUI = Instantiate(Manager.Resource.Load<HPBar>("UI/MonsterUI").gameObject, transform);
            hpBar = hpBarUI.GetComponent<HPBar>();
            hpBar.Monster = this;
        }
        if (hpBarInvisibleMode)
        {
            if (hpBarInvisibleRoutine != null)
            {
                StopCoroutine(hpBarInvisibleRoutine);
                hpBarInvisibleRoutine = null;
            }
            hpBarInvisibleRoutine = StartCoroutine(HPBarInvisibleRoutine(hpBar.gameObject));
        }

        hp -= damage;
        OnChangeHP?.Invoke((float)hp / maxHp);
        Debug.Log($"몬스터가 받은 데미지 : {damage}");
        if (canKnockback)
        {
            rigid.velocity = Vector3.zero;
        }

        StartCoroutine(HurtRoutine(hp <= 0));
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            nav.enabled = false;
            OnDead?.Invoke();
            switch (type)
            {
                case Type.Slime:
                    Collider slimeAttackRange = bullet.GetComponent<Collider>();
                    slimeAttackRange.enabled = false;
                    break;
                case Type.Bat:
                    Collider batAttackRange = bullet.GetComponent<Collider>();
                    batAttackRange.enabled = false;
                    break;
                case Type.Mummy:
                    break;
                case Type.Mage:
                    break;
            }
            stateMachine.ChangeState(State.Die);
            Destroy(gameObject, 5f);
        }
    }

    protected virtual IEnumerator HurtRoutine(bool isDead)
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
            if (nav.enabled)
                nav.isStopped = true;
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = hurtColor;
            }
            yield return new WaitForSeconds(0.1f);

            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = curColor;//Color.white;
            }
            if (nav.enabled)
                nav.isStopped = false;
        }
    }

    Coroutine hpBarInvisibleRoutine;
    IEnumerator HPBarInvisibleRoutine(GameObject hpBar)
    {
        hpBar.SetActive(true);
        yield return new WaitForSeconds(2f);
        hpBar.SetActive(false);
    }
    #endregion
}
