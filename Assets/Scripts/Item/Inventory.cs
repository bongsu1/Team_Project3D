using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventory;
    [SerializeField] bool[] hasKey = new bool[3]; // test

    public void ItmeUse()
    {
        inventory[0].ItemUse();
    }

    public void FillHealpotion(int value = 5) // 상자를 열거나 휴식터에서 쉬면 물약을 채움
    {
        inventory[0].ItemCount += value;
    }

    public void GetKeyItem(int index)
    {
        inventory[index].ItemCount++;
        hasKey[index - 1] = true; // test
    }
}
