using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Item[] inventoryItem;
    [SerializeField] bool[] hasKey = new bool[3]; // test

    /// <summary>
    /// 0��°�� ����, 1��°���� 3��°���� Ű ������
    /// </summary>
    public Item[] InventoryItem => inventoryItem;

    /// <summary>
    /// ������
    /// </summary>
    public void ItmeUse()
    {
        inventoryItem[0].ItemUse();
    }

    /// <summary>
    ///  count��ŭ ���� ä��� count�� �Է¾��ϸ� 5��
    /// </summary>
    /// <param name="count"></param>
    public void FillHealpotion(int count = 5) // ���ڸ� ���ų� �޽��Ϳ��� ���� ������ ä��
    {
        inventoryItem[0].ItemCount += count;
    }

    /// <summary>
    /// index 1��°���� 3��°���� �Է�
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
