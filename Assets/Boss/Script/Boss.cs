using UnityEngine;

public class Boss : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    public int Hp { get { return hp; } set { hp = value; } }
    int maxHp;
    private void Start()
    {
        maxHp = hp;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"보스 남은 체력 : {hp}");
    }
}
