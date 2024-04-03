using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonster : MonoBehaviour, IDamagable
{
	public void TakeDamage(int damage)
	{
		Debug.Log($"몬스터 데미지 : {damage}");
	}
}
