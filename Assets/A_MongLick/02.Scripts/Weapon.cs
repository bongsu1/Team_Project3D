using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Re")]
    [SerializeField] 
	float attackDelay; // �� ������ �� ������ Ȱ ������ ��Ÿ��
	public float AttackDelay => attackDelay;

	[Header("Component")]
	public BoxCollider swordArea;//... ���� (�ҵ� ����)
	public GameObject weaponArrowPrefab;//... ���� (���� ����)
    public Rigidbody arrowRigid;//... ����

    [Header("Spec")]
	[SerializeField] int damage;//... ���� (�ҵ� ����)
    public int thirdAttackDamage;//... ���� (�ҵ� ����)
	public float arrowRate;//... �������� �ʿ䰡 �ֳ�?
	[SerializeField] float thirdRate;//... ���� (�ҵ� ����)
	[SerializeField] float weaponAttackTime;//... ���� (�ҵ� ����)
    [SerializeField] bool firstAttack = false;//... ���� (�ҵ� ����) ������Ʈ�ӽſ��� ����
    [SerializeField] bool secondAttack = false;//... ���� (�ҵ� ����) ������Ʈ�ӽſ��� ����
    [SerializeField] bool thirdAttack = false;//... ���� (�ҵ� ����) ������Ʈ�ӽſ��� ����
    [SerializeField] bool canAttack = true;//... ����
    [SerializeField] float arrowSize;//...
	[SerializeField] float arrowChargingSize;//...
	[SerializeField] float maxArrowRange;//... ���� (���� ����)
	[SerializeField] float arrowSpeed;//... ���� (���� ����)
    [SerializeField] Transform arrowTransform;//... ���� (���� ����)
    [SerializeField] bool isArrowShot;//... ���� (���� ����) ������Ʈ�ӽſ��� ����
    [SerializeField] bool isArrowChargingShot;//... ���� (���� ����) ������Ʈ�ӽſ��� ����
    [SerializeField] bool canArrow = true;//... ���� (���� ����) ������Ʈ�ӽſ��� ����

    [SerializeField] bool bowFirstTime = false;//... ���� (���� ����) ������Ʈ�ӽſ��� ����
    public bool BowFirstTime => bowFirstTime;
    [SerializeField] bool bowSecondTime = false;//... ���� (���� ����) ������Ʈ�ӽſ��� ����
    public bool BowSecondTime => bowSecondTime;
	[SerializeField] float bowTime;//... ���� (���� ����) ������Ʈ�ӽſ��� ����
    public int bowFirstDamage = 10;//... ���� (���� ����)
    public int bowSecondDamag = 20;//... ���� (���� ����)
    public int bowThirdDamage = 30;//... ���� (���� ����)

    private Coroutine firstCoroutine;
	private Coroutine secondCoroutine;

	private Coroutine bowCoroutine;

	[Header("Weapon")]
	[SerializeField] LayerMask weaponMonsterLayer;//... ���� (�ҵ� ����)
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