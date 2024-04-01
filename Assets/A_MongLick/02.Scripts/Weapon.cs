using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type { Sword, Bow };
	public Type type;

	[Header("Component")]
	public BoxCollider swordArea;

	[Header("Spec")]
	public int damage;
	[SerializeField] float rate;
	public float Rate => rate;

	[Header("Attack")]
	[SerializeField] LayerMask monsterLayer;

	public void Use()
	{
		if (type == Type.Sword)
		{
			StopCoroutine("Swing");
			StartCoroutine("Swing");
		}
	}

	IEnumerator Swing()
	{
		swordArea.enabled = true;
		yield return new WaitForSeconds(rate);
		swordArea.enabled = false;
		yield break;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(monsterLayer.Contain(other.gameObject.layer))
		{
			IDamagable damagable = other.GetComponent<IDamagable>();
			if(damagable != null)
			{
				damagable.TakeDamage(damage);
			}
		}
	}
}