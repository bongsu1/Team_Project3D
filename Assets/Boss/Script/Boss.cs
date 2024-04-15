using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour, IDamagable
{
    [SerializeField] int hp;
    public int Hp => hp;
    int maxHp;

    public UnityEvent<float> OnTakeDamage;

    private void Start()
    {
        maxHp = hp;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"보스 남은 체력 : {hp}");

        if (hp <= 0)
            hp = 0;

        OnTakeDamage?.Invoke((float)hp / maxHp);
    }
}
