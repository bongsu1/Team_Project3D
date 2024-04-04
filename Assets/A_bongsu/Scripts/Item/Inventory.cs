using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventory;

    public void ItmeUse()
    {
        inventory[0].ItemUse();
    }

    public void FillHealpotion(int value = 5) // 상자를 열거나 휴식터에서 쉬면 물약을 채움
    {
        inventory[0].ItemCount += value;
        if (inventory[0].ItemCount > inventory[0].MaxItemCount)
        {
            inventory[0].ItemCount = inventory[0].MaxItemCount;
        }
    }
}
