using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    float attackDelay; // 검 공격은 선 딜레이 활 공격은 쿨타임
    public float AttackDelay => attackDelay;
}