using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventoryItem;
    [SerializeField] bool[] hasKey = new bool[3]; // test

    /// <summary>
    /// 0번째는 물약, 1번째부터 3번째까지 키 아이템
    /// </summary>
    public Item[] InventoryItem => inventoryItem;

    /// <summary>
    /// 물약사용
    /// </summary>
    public void ItmeUse()
    {
        inventoryItem[0].ItemUse();
    }

    /// <summary>
    ///  count만큼 물약 채우기 count를 입력안하면 5개
    /// </summary>
    /// <param name="count"></param>
    public void FillHealpotion(int count = 5) // 상자를 열거나 휴식터에서 쉬면 물약을 채움
    {
        inventoryItem[0].ItemCount += count;
    }

    /// <summary>
    /// index 1번째부터 3번째까지 입력
    /// </summary>
    /// <param name="index"></param>
    public void GetKeyItem(int index)
    {
        if (index <= 0 || index >= inventoryItem.Length)
            return;

        inventoryItem[index].ItemCount++;
        hasKey[index - 1] = true; // test
    }
}
