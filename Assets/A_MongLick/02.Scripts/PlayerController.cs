using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
	[Header("Component")]
	[SerializeField] Rigidbody rigid;
	public Rigidbody Rigid => rigid;
	[SerializeField] Weapon swordWeapon;
	public Weapon SwordWeapon => swordWeapon;
	[SerializeField] Weapon bowWeapon;
	public Weapon BowWeapon => bowWeapon;
	[SerializeField] PlayerInput playerInput;
	public PlayerInput PlayerInput => playerInput;
	[SerializeField] int playerLayer;
	public int PlayerLayer => playerLayer;
	[SerializeField] int invincibilityLayer;
	public int InvincibilityLayer => invincibilityLayer;
	[SerializeField] Camera followCamera;
	public Camera FollowCamera => followCamera;
	[SerializeField] Transform spawnPonit;
	[SerializeField] Material lineMaterial;
	public Material LineMaterial => lineMaterial;
	[SerializeField] LineRenderer lineRenderer;
	public LineRenderer LineRenderer => lineRenderer;
	[SerializeField] LayerMask wallLayer;
	public LayerMask WallLayer => wallLayer;


	[Header("Spec")]
	[SerializeField] float moveSpeed;
	[SerializeField] float turnSpeed;
	[SerializeField] float dashSpeed;
	[SerializeField] float dashTime;
	[SerializeField] bool canDashCoolTime = true;
	[SerializeField] float lineSize;
	public float LineSize => lineSize;

	public bool CanDashCoolTime { get { return canDashCoolTime; } set { canDashCoolTime = value; } }
	public float DashTime => dashTime;

	public enum State { Move, Sword, Dash, Bow }
	private StateMachine<State> stateMachine = new StateMachine<State>();

	private Vector3 moveDir;

	private Coroutine dashCoolTimer;

	private void Start()
	{
		stateMachine.AddState(State.Move, new MoveState(this));
		stateMachine.AddState(State.Sword, new SwordState(this));
		stateMachine.AddState(State.Dash, new DashState(this));
		stateMachine.AddState(State.Bow, new BowState(this));
		stateMachine.Start(State.Move);
	}

	private void OnMove(InputValue value)
	{
		Vector2 inputDir = value.Get<Vector2>();
		moveDir.x = inputDir.x;
		moveDir.z = inputDir.y;
		moveDir.Normalize();
	}

	public void Move()
	{
		transform.forward = Vector3.Lerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
		rigid.MovePosition(this.gameObject.transform.position + moveDir * moveSpeed * Time.deltaTime);
	}

	private void FixedUpdate()
	{
		stateMachine.FixedUpdate();
	}

	private void Update()
	{
		stateMachine.Update();
	}

	private void OnSword(InputValue value)
	{
		Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		if (Physics.Raycast(ray, out rayHit, 100))
		{
			Vector3 nextVec = rayHit.point - transform.position;
			nextVec.y = 0;
			transform.LookAt(transform.position + nextVec);
		}
	}

	public void PlayerSword()
	{
		swordWeapon.Sword();
	}

	private void OnBow(InputValue value)
	{
		bowWeapon.Bow(value.isPressed);
	}

	Vector3 dashVec;

	private void OnDash(InputValue value)
	{
		if (canDashCoolTime == false) return;

		dashCoolTimer = StartCoroutine(DashCoolTime(5f));
		dashVec = moveDir;
	}

	IEnumerator DashCoolTime(float time)
	{
		yield return new WaitForSeconds(time);
		canDashCoolTime = true;
	}

	public void DashMove()
	{
		rigid.velocity = new Vector3(dashVec.x, 0, dashVec.z) * dashSpeed;
	}

	private void OnSkillQ(InputValue value)
	{

	}

	private void OnSkillE(InputValue value)
	{

	}

	private void OnInteraction(InputValue value)
	{

	}

	private void OnItem1(InputValue value)
	{

	}

	private void OnItem2(InputValue value)
	{

	}

	private void OnItem3(InputValue value)
	{

	}

	[ContextMenu("Test")]
	public void TestDamage()
	{
		TakeDamage(40);
	}

	public void TakeDamage(int damage)
	{

		Manager.Game.PlayerData.Hp -= damage;
		Debug.Log($"플레이어 받은 데미지 : {damage}");
		Debug.Log($"플레이어 남은 체력 : {Manager.Game.PlayerData.Hp}");

		if (Manager.Game.PlayerData.Hp <= 0)
		{
			Manager.Game.PlayerData.Hp = 0;
			Debug.Log($"플레이어 죽음");
			StartCoroutine(spawnRoutine());
		}
	}

	IEnumerator spawnRoutine()
	{
		yield return new WaitForSeconds(1.5f);
		transform.position = spawnPonit.position;
		Manager.Game.PlayerData.Hp = 100;
	}

}
