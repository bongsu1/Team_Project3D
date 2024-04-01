using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
	[Header("Component")]
	[SerializeField] Rigidbody rigid;
	[SerializeField] Weapon weapon;
	public Weapon Weapon => weapon;
	[SerializeField] PlayerInput playerInput;
	public PlayerInput PlayerInput => playerInput;
	[SerializeField] int playerLayer;
	public int PlayerLayer => playerLayer;
	[SerializeField] int invincibilityLayer;
	public int InvincibilityLayer => invincibilityLayer;
	[SerializeField] Camera followCamera;
	[SerializeField] Transform ponit;

	[Header("Spec")]
	[SerializeField] float moveSpeed;
	[SerializeField] float turnSpeed;
	[SerializeField] float dashSpeed;
	[SerializeField] float dashTime;
	[SerializeField] bool canDashCoolTime = true;
	public bool CanDashCoolTime { get { return canDashCoolTime; } set { canDashCoolTime = value; } }
	public float DashTime => dashTime;

	public enum State { Move, Sword, Dash }
	private StateMachine<State> stateMachine = new StateMachine<State>();

	private Vector3 moveDir;

	private Coroutine dashCoolTimer;

	private void Start()
	{
		stateMachine.AddState(State.Move, new MoveState(this));
		stateMachine.AddState(State.Sword, new SwordState(this));
		stateMachine.AddState(State.Dash, new DashState(this));
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
		if(Physics.Raycast(ray, out rayHit, 100))
		{
			Vector3 nextVec = rayHit.point - transform.position;
			nextVec.y = 0;
			transform.LookAt(transform.position + nextVec);
		}
	}

	public void Use()
	{
		weapon.Use();
	}

	private void OnBow(InputValue value)
	{
		
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
		rigid.MovePosition(this.gameObject.transform.position + dashVec * dashSpeed * Time.deltaTime);
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

	[ContextMenu("Damage")]
	public void TakeDamage(int damage)
	{
		
		Manager.Game.PlayerData.Hp -= 50;
		Debug.Log($"�÷��̾� ���� ������ : {damage}");
		Debug.Log($"�÷��̾� ���� ü�� : {Manager.Game.PlayerData.Hp}");

		if(Manager.Game.PlayerData.Hp <= 0)
		{
			Manager.Game.PlayerData.Hp = 0;
			Debug.Log($"�÷��̾� ����");
			StartCoroutine(spawnRoutine());
		}
	}

	IEnumerator spawnRoutine()
	{
		gameObject.SetActive(false);
		yield return new WaitForSeconds(1.5f);
		transform.position = ponit.position;
		gameObject.SetActive(true);
		Manager.Game.PlayerData.Hp = 100;
	}

}