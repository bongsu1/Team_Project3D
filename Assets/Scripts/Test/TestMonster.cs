using UnityEngine;

public class TestMonster : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Debug.Log($"���� ������ : {damage}");
    }
}
