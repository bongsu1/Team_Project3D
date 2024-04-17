using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    public UnityAction<int> OnChangeHp;
    public UnityAction<int> OnChangeMaxHp;

    [SerializeField] int hp;
    [SerializeField] int maxHp;
    public int Hp
    {
        get { return hp; }
        set { hp = value; if (hp > maxHp) hp = maxHp; else if (hp < 0) hp = 0; OnChangeHp?.Invoke(hp); }
    }
    public int MaxHp { get { return maxHp; } set { maxHp = value; OnChangeMaxHp?.Invoke(value); } }
}
