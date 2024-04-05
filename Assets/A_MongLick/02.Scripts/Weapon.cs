using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Re")]
    [SerializeField] 
	float attackDelay; // 검 공격은 선 딜레이 활 공격은 쿨타임
	public float AttackDelay => attackDelay;

	[Header("Component")]
	public BoxCollider swordArea;//... 지워 (소드 전용)
	public GameObject weaponArrowPrefab;//... 지워 (보우 전용)
    public Rigidbody arrowRigid;//... 지워

    [Header("Spec")]
	[SerializeField] int damage;//... 지워 (소드 전용)
    public int thirdAttackDamage;//... 지워 (소드 전용)
	public float arrowRate;//... 따로있을 필요가 있나?
	[SerializeField] float thirdRate;//... 지워 (소드 전용)
	[SerializeField] float weaponAttackTime;//... 지워 (소드 전용)
    [SerializeField] bool firstAttack = false;//... 지워 (소드 전용) 스테이트머신에서 구현
    [SerializeField] bool secondAttack = false;//... 지워 (소드 전용) 스테이트머신에서 구현
    [SerializeField] bool thirdAttack = false;//... 지워 (소드 전용) 스테이트머신에서 구현
    [SerializeField] bool canAttack = true;//... 지워
    [SerializeField] float arrowSize;//...
	[SerializeField] float arrowChargingSize;//...
	[SerializeField] float maxArrowRange;//... 지워 (보우 전용)
	[SerializeField] float arrowSpeed;//... 지워 (보우 전용)
    [SerializeField] Transform arrowTransform;//... 지워 (보우 전용)
    [SerializeField] bool isArrowShot;//... 지워 (보우 전용) 스테이트머신에서 구현
    [SerializeField] bool isArrowChargingShot;//... 지워 (보우 전용) 스테이트머신에서 구현
    [SerializeField] bool canArrow = true;//... 지워 (보우 전용) 스테이트머신에서 구현

    [SerializeField] bool bowFirstTime = false;//... 지워 (보우 전용) 스테이트머신에서 구현
    public bool BowFirstTime => bowFirstTime;
    [SerializeField] bool bowSecondTime = false;//... 지워 (보우 전용) 스테이트머신에서 구현
    public bool BowSecondTime => bowSecondTime;
	[SerializeField] float bowTime;//... 지워 (보우 전용) 스테이트머신에서 구현
    public int bowFirstDamage = 10;//... 지워 (보우 전용)
    public int bowSecondDamag = 20;//... 지워 (보우 전용)
    public int bowThirdDamage = 30;//... 지워 (보우 전용)

    private Coroutine firstCoroutine;
	private Coroutine secondCoroutine;

	private Coroutine bowCoroutine;

	[Header("Weapon")]
	[SerializeField] LayerMask weaponMonsterLayer;//... 지워 (소드 전용)
    public LayerMask WeaponMonsterLayer => weaponMonsterLayer;


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
		yield return new WaitForSeconds(attackDelay);
		canAttack = true;
		swordArea.enabled = false;
		yield return new WaitForSeconds(weaponAttackTime);
		firstAttack = false;
	}

	IEnumerator secondAttackSwing()
	{
		secondAttack = true;
		swordArea.enabled = true;
		yield return new WaitForSeconds(attackDelay);
		canAttack = true;
		swordArea.enabled = false;
		yield return new WaitForSeconds(weaponAttackTime);
		firstAttack = false;
		secondAttack = false;
	}

	IEnumerator thirdAttackSwing()
	{
		thirdAttack = true;
		firstAttack = false;
		secondAttack = false;
		swordArea.enabled = true;
		yield return new WaitForSeconds(attackDelay);
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

			if (bowCoroutine != null)
			{
				StopCoroutine(bowCoroutine);
				bowSecondTime = false;
				bowFirstTime = false;
			}
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
		Arrow arrow = Instantiate(weaponArrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.ArrowRange = maxArrowRange;
		arrow.Damage = bowFirstDamage;
		arrow.ArrowSpeed = arrowSpeed;
		yield return null;
	}

	IEnumerator ChargingShootArrow()
	{
		Arrow arrow = Instantiate(weaponArrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.ArrowRange = maxArrowRange * 2;
		arrow.Damage = bowSecondDamag;
		arrow.ArrowSpeed = arrowSpeed * 2;

		yield return null;
	}

	IEnumerator ChargingShootArrow2()
	{
		Arrow arrow = Instantiate(weaponArrowPrefab, arrowTransform.position, arrowTransform.rotation).GetComponent<Arrow>();
		Rigidbody arrowRigidbody = arrow.gameObject.GetComponent<Rigidbody>();
		arrow.Damage = bowThirdDamage;
		arrow.ArrowSpeed = arrowSpeed * 2;
		arrow.ArrowRange = maxArrowRange * 2;
		yield return null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (weaponMonsterLayer.Contain(other.gameObject.layer))
		{
			IDamagable damagable = other.GetComponent<IDamagable>();
			if (damagable != null)
			{
				damagable.TakeDamage(thirdAttack ? thirdAttackDamage : damage);
			}
		}
	}

	public void Cancel()
	{
		if (bowCoroutine != null)
		{
			StopCoroutine(bowCoroutine);
		}
		canArrow = false;
		StartCoroutine(BowDelays());
	}
}