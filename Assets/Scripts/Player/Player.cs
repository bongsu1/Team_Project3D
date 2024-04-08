using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamagable
{
    public enum State { Normal, FirstSword, SecondSword, ThirdSword, Bow, Dash }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("State")] // test 스테이트 확인용 빌드전 삭제 요망
    [SerializeField] string curStateName;

    [Header("Component")]
    [SerializeField]
    Rigidbody rigid;
    public Rigidbody Rigid => rigid;
    [SerializeField]
    PlayerInput input;
    public PlayerInput Input => input;
    [SerializeField]
    Animator animator;
    public Animator Animator => animator;

    [Header("Render")]
    [SerializeField] GameObject swordRender;
    [SerializeField] GameObject bowRender;

    [Header("Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [Header("Attack")]
    [SerializeField] LayerMask groundLayer; // 방향돌리기용 다른 충돌체에 부딪혔을때 방향이 휨
    [SerializeField]
    Sword sword;
    public Sword Sword => sword;
    [SerializeField]
    Bow bow;
    public Bow Bow => bow;

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField]
    float dashTime;
    public float DashTime => dashTime;
    [SerializeField]
    float dashCoolTime;
    public float DashCoolTime => dashCoolTime;

    [Header("Collision")]
    [SerializeField] LayerMask monsterLayer;

    Vector3 moveDir;
    public Vector3 MoveDir => moveDir;
    Vector3 dashDir;
    public Vector3 DashDir { get { return dashDir; } set { dashDir = value; } }
    Vector2 Pointer;

    bool canDash = true;
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    bool isDead;
    bool isSword = true;
    public bool IsSword
    {
        get { return isSword; }
        set
        {
            isSword = value;
            animator.SetBool("IsSword", value);
            if (value)
            {
                swordRender.SetActive(true);
                bowRender.SetActive(false);
            }
            else
            {
                swordRender.SetActive(false);
                bowRender.SetActive(true);
            }
        }
    }

    private void Start()
    {
        stateMachine.AddState(State.Normal, new NormalState(this));
        stateMachine.AddState(State.FirstSword, new FirstSwordState(this));
        stateMachine.AddState(State.SecondSword, new SecondSwordState(this));
        stateMachine.AddState(State.ThirdSword, new ThirdSwordState(this));
        stateMachine.AddState(State.Bow, new BowState(this));
        stateMachine.AddState(State.Dash, new DashState(this));

        animator.SetBool("IsSword", isSword); // 초기 값 설정 처음은 검을 들고있는 상태
        stateMachine.Start(State.Normal);
    }

    private void Update()
    {
        curStateName = stateMachine.CurState;
        if (isDead)
            return;

        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        if (isDead)
            return;

        stateMachine.LateUpdate();
    }

    public void Move()
    {
        rigid.velocity = new Vector3(moveDir.x * moveSpeed, rigid.velocity.y, moveDir.z * moveSpeed);
    }

    public void Turn(bool isNormalState)
    {
        if (isNormalState)
        {
            transform.forward = Vector3.Lerp(transform.forward, moveDir, turnSpeed);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Pointer);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 turnDir = hit.point - transform.position;
                turnDir.y = 0;
                transform.LookAt(transform.position + turnDir);
            }
        }
    }

    public void Dash()
    {
        rigid.velocity = new Vector3(dashDir.x * dashSpeed, rigid.velocity.y, dashDir.z * dashSpeed);
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void OnPointer(InputValue value)
    {
        Pointer = value.Get<Vector2>();
    }

    private void OnItem1(InputValue value)
    {
        Manager.Game.Inventory.ItmeUse();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & monsterLayer) != 0)
        {
            moveDir = Vector3.zero;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & monsterLayer) != 0)
        {
            Rigidbody monsterRigid = collision.gameObject.GetComponent<Rigidbody>();
            if (monsterRigid != null)
            {
                monsterRigid.velocity = Vector3.zero;
            }
        }
    }

    // test..
    [Header("Test")]
    public int testDamage;
    public int fillCount;
    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        TakeDamage(testDamage);
    }
    [ContextMenu("Test Fill Potion")]
    public void TestFillPotion()
    {
        Manager.Game.Inventory.FillHealpotion(fillCount);
    }
    public void TakeDamage(int damage)
    {
        Manager.Game.PlayerData.Hp -= damage;

        if (Manager.Game.PlayerData.Hp <= 0)
        {
            isDead = true;
            animator.Play("Death");
            gameObject.layer = 22; // 22 = 무적 레이어
        }
    }
}
