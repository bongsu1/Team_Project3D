using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventory;
    [SerializeField] bool[] hasKey = new bool[3]; // test

    public void ItmeUse()
    {
        inventory[0].ItemUse();
    }

    public void FillHealpotion(int value = 5) // ���ڸ� ���ų� �޽��Ϳ��� ���� ������ ä��
    {
        inventory[0].ItemCount += value;
    }

    public void GetKeyItem(int index)
    {
        inventory[index].ItemCount++;
        hasKey[index - 1] = true; // test
    }
}
