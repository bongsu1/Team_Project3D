using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Component")]
	public BoxCollider swordArea;
	public GameObject arrowPrefab;
	protected PlayerController oner;

	[Header("Spec")]
	public int damage;
	public int thirdAttackDamage;
	[SerializeField] float rate;
	public float Rate => rate;
	public float arrowRate;
	[SerializeField] float thirdRate;
	[SerializeField] float attackTime = 1f;
	[SerializeField] bool firstAttack = false;
	[SerializeField] bool secondAttack = false;
	[SerializeField] bool thirdAttack = false;
	[SerializeField] bool canAttack = true;

	[SerializeField] bool bowFirstTime = false;
	public bool BowFirstTime => bowFirstTime;
	[SerializeField] bool bowSecondTime = false;
	public bool BowSecondTime => bowSecondTime;
	[SerializeField] float bowTime;
	public int bowFirstDamage = 10;
	public int bowFecondDamag = 20;
	public int bowThirdDamage = 30;

	private Coroutine firstCoroutine;
	private Coroutine secondCoroutine;

	private Coroutine bowCoroutine;

	[Header("Attack")]
	[SerializeField] LayerMask monsterLayer;
	public LayerMask MonsterLayer => monsterLayer;


	public void Sword()
	{
		if (!canAttack)
			return;
		canAttack = false;

		if (secondAttack)
		{
			StopCoroutine(secondCoroutine);
			StartCoroutine(thirdAttackSwing());
			return;
		}
		else if (firstAttack)
		{
			StopCoroutine(firstCoroutine);
			secondCoroutine = StartCoroutine(secondAttackSwing());
			return;
		}
		firstCoroutine = StartCoroutine(firstAttackSwing());
	}


	IEnumerator firstAttackSwing()
	{
		Debug.Log("1번째 공격");
		firstAttack = true;
		swordArea.enabled = true;
		yield return new WaitForSeconds(rate);
		canAttack = true;
		swordArea.enabled = false;
		yield return new WaitForSeconds(attackTime);
		firstAttack = false;
	}

	IEnumerator secondAttackSwing()
	{
		Debug.Log("2번째 공격");
		secondAttack = true;
		swordArea.enabled = true;
		yield return new WaitForSeconds(rate);
		canAttack = true;
		swordArea.enabled = false;
		yield return new WaitForSeconds(attackTime);
		firstAttack = false;
		secondAttack = false;
	}

	IEnumerator thirdAttackSwing()
	{
		thirdAttack = true;
		Debug.Log("3번째 공격");
		firstAttack = false;
		secondAttack = false;
		swordArea.enabled = true;
		yield return new WaitForSeconds(rate);
		swordArea.enabled = false;
		thirdAttack = false;
		yield return new WaitForSeconds(thirdRate);
		canAttack = true;
	}

	public void Bow(bool onClick)
	{
		if (onClick)
		{
			bowCoroutine = StartCoroutine(BowTime());
		}
		else
		{
			StopCoroutine(bowCoroutine);
			/*if()
			{
				 // 대쉬중에 취소 캔슬시 등
			}*/
			if(bowSecondTime)
			{
				
			}
			else if(bowFirstTime)
			{

			}
			else
			{

			}
			bowSecondTime = false;
			bowFirstTime = false;
		}
	}

	IEnumerator BowTime()
	{
		yield return new WaitForSeconds(bowTime);
		bowFirstTime = true;
		yield return new WaitForSeconds(bowTime);
		bowSecondTime = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (monsterLayer.Contain(other.gameObject.layer))
		{
			IDamagable damagable = other.GetComponent<IDamagable>();
			if (damagable != null)
			{
				damagable.TakeDamage(thirdAttack ? thirdAttackDamage : damage);
			}
		}
	}
}