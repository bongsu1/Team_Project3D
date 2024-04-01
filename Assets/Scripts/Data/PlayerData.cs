using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] int hp;
	[SerializeField] int maxHp;
	public int Hp { get { return hp; } set { hp = value; } }
	public int MaxHp { get { return maxHp; } set { maxHp = value; } }
}
