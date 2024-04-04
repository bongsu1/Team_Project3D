using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventory;

    public void ItmeUse()
    {
        inventory[0].ItemUse();
    }

    public void FillHealpotion(int value = 5) // ���ڸ� ���ų� �޽��Ϳ��� ���� ������ ä��
    {
        inventory[0].ItemCount += value;
        if (inventory[0].ItemCount > inventory[0].MaxItemCount)
        {
            inventory[0].ItemCount = inventory[0].MaxItemCount;
        }
    }
}
