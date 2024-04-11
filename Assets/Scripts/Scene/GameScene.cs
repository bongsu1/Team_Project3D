using System.Collections;
using UnityEngine;

public class GameScene : BaseScene
{
    [Header("Puzzle")]
    [SerializeField] Statue[] statues;
    int insertStatueCount; // ������ ������ ����

    [Header("Monster")]
    [SerializeField] Monster[] monsters;
    int monsterCount;

    [Header("Clear Test")]
    [SerializeField] PopUpUI clearUI;

    public override IEnumerator LoadingRoutine()
    {
        // ���� ���� ���� ���� ���ε��� �̸� �ؾߵɰ͵�

        // ĳ���� �ʱ�ȭ
        Manager.Game.PlayerData.Hp = Manager.Game.PlayerData.MaxHp;
        Manager.Game.Inventory.InventoryItem[0].ItemCount = Manager.Game.Inventory.InventoryItem[0].MaxItemCount;
        yield return null;
    }

    private void OnEnable()
    {
        statues = FindObjectsOfType<Statue>();
        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].OnInsert += PuzzleClear;
        }
        monsters = FindObjectsOfType<Monster>();
        monsterCount = monsters.Length; // �� �ʿ� �ִ� ���� ��
        if (monsterCount == 0) // ���Ͱ� ������ ���� ����
        {
            monsterCount++;
            PuzzleStart();
        }
        for (int i = 0; i < monsterCount; i++)
        {
            monsters[i].OnDead += PuzzleStart;
        }
    }

    private void PuzzleStart()
    {
        monsterCount--;
        if (monsterCount == 0)
        {
            if (statues.Length == 0) // ���͸� �� ���� �� ������ ������ Ŭ����
            {
                // Ŭ���� �� ������ ��ư Ȱ��ȭ
                return;
            }

            for (int i = 0; i < statues.Length; i++)
            {
                statues[i].enabled = true;
            }
        }
    }

    public void PuzzleClear(bool isInsert)
    {
        if (isInsert)
        {
            insertStatueCount++;
            if (insertStatueCount == statues.Length)
            {
                // Ŭ���� �� ������ ��ư Ȱ��ȭ
            }
        }
        else
        {
            insertStatueCount--;
        }
    }
}
