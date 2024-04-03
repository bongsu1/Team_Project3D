using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Component")]
	public BoxCollider swordArea;
	public GameObject arrowPrefab;
	protected PlayerController oner;
	public Rigidbody arrowRigid;

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
	[SerializeField] float arrowSize;
	[SerializeField] float arrowChargingSize;
	[SerializeField] float maxArrowRange;
	[SerializeField] float arrowSpeed;
	[SerializeField] Transform arrowTransform;
	[SerializeField] bool isArrowShot;
	[SerializeField] bool isArrowChargingShot;
	[SerializeField] bool canArrow = true;
	// 스크립트 안에서 하는거 다 뺴기

	[SerializeField] bool bowFirstTime = false;
	public bool BowFirstTime => bowFirstTime;
	[SerializeField] bool bowSecondTime = false;
	public bool BowSecondTime => bowSecondTime;
	[SerializeField] float bowTime;
	public int bowFirstDamage = 10;
	public int bowSecondDamag = 20;
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
			if (!canArrow)
				return;

			bowCoroutine = StartCoroutine(BowTime());
		}
		else
		{
            if (!canArrow)
                return;

			canArrow = false;

			if (bowCoroutine != null)
			{
				StopCoroutine(bowCoroutine);
			}
			if (bowSecondTime)
			{
				StartCoroutine(ChargingShootArrow2());
			}
			else if (bowFirstTime)
			{
				StartCoroutine(ChargingShootArrow());
			}
			else
			{
				StartCoroutine(ShootArrow());
			}
			bowSecondTime = false;
			bowFirstTime = false;
			StartCoroutine(BowDelays());
		}
	}

	IEnumerator BowTime()
	{
		yield return new WaitForSeconds(bowTime);
		bowFirstTime = true;
		yield return new WaitForSeconds(bowTime);
		bowSecondTime = true;
	}

	IEnumerator BowDelays()
	{
		yield return new WaitForSeconds(arrowRate);
		canArrow = true;
	}

	IEnumerator ShootArrow()
	{
		Arrow arrow = Instantiate(arrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.MaxArrowRange = maxArrowRange;
		arrow.Damage = bowFirstDamage;
		arrow.ArrowSpeed = arrowSpeed;
		yield return null;
	}

	IEnumerator ChargingShootArrow()
	{
		Arrow arrow = Instantiate(arrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.MaxArrowRange = maxArrowRange * 2;
		arrow.Damage = bowSecondDamag;
		arrow.ArrowSpeed = arrowSpeed * 2;

		yield return null;
	}

	IEnumerator ChargingShootArrow2()
	{
		Arrow arrow = Instantiate(arrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.Damage = bowThirdDamage;
		arrow.ArrowSpeed = arrowSpeed * 2;
		arrow.MaxArrowRange = maxArrowRange * 2;
		yield return null;
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