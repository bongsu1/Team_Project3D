using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField]
    float attackDelay; // �� ������ �� ������ Ȱ ������ ��Ÿ��
    public float AttackDelay => attackDelay;
}