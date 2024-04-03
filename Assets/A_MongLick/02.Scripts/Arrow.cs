using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] LayerMask MonsterLayer;
	[SerializeField] LayerMask WallLayer;
	[SerializeField] int damage;
	public int Damage { set { damage = value; } }
	[SerializeField] float arrowSpeed;
	public float ArrowSpeed { set { arrowSpeed = value; } }
	[SerializeField] Rigidbody rigid;
	[SerializeField] float maxArrowRange;
	public float MaxArrowRange { set { maxArrowRange = value; Debug.Log("아무거나 셋"); } }


	private void OnTriggerEnter(Collider other)
	{
		if (MonsterLayer.Contain(other.gameObject.layer))
		{
			IDamagable monster = other.GetComponent<IDamagable>();
			monster?.TakeDamage(damage);
			Destroy(gameObject);
			Debug.Log($" 화살 데미지 : {damage}");
		}
	}


	private void Start()
	{
		StartCoroutine(ArrowTime());
	}

	private void Update()
	{
		rigid.velocity = transform.forward * arrowSpeed;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (WallLayer.Contain(collision.gameObject.layer))
		{
			Destroy(gameObject);
		}
	}

	IEnumerator ArrowTime()
	{
		Debug.Log($"거리 : {maxArrowRange}");
		yield return new WaitForSeconds(maxArrowRange / arrowSpeed);
		Destroy(gameObject);
	}
}
