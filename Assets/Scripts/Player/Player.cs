using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamagable
{
    public enum State { Normal, FirstSword, SecondSword, ThirdSword, Bow, Dash }
    private StateMachine<State> stateMachine = new StateMachine<State>();

    [Header("Test")] // test 스테이트 확인용 빌드전 삭제 요망
    [SerializeField] string curStateName;

    [Header("Component")]
    [SerializeField]
    Rigidbody rigid;
    public Rigidbody Rigid => rigid;
    [SerializeField]
    PlayerInput input;
    public PlayerInput Input => input;
    [SerializeField] Inventory inventory;

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

    private void Start()
    {
        stateMachine.AddState(State.Normal, new NormalState(this));
        stateMachine.AddState(State.FirstSword, new FirstSwordState(this));
        stateMachine.AddState(State.SecondSword, new SecondSwordState(this));
        stateMachine.AddState(State.ThirdSword, new ThirdSwordState(this));
        stateMachine.AddState(State.Bow, new BowState(this));
        stateMachine.AddState(State.Dash, new DashState(this));
        stateMachine.Start(State.Normal);
    }

    private void Update()
    {
        curStateName = stateMachine.CurState;
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    public void Move()
    {
        rigid.velocity = new Vector3(moveDir.x, rigid.velocity.y, moveDir.z) * moveSpeed;
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
        rigid.velocity = new Vector3(dashDir.x, rigid.velocity.y, dashDir.z) * dashSpeed;
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
        inventory.ItmeUse();
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

    public void TakeDamage(int damage)
    {
        Manager.Game.PlayerData.Hp -= damage;

        if (Manager.Game.PlayerData.Hp <= 0)
        {
            Debug.Log("죽음");
        }
    }
}
