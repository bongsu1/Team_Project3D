using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameScene : BaseScene
{
    [Header("Puzzle")]
    [SerializeField] Statue[] statues;
    int insertStatueCount; // ������ ������ ����

    [Header("Monster")]
    [SerializeField] Monster[] monsters;
    int monsterCount;

    [Header("Next Stage Button")]
    [SerializeField] NextStageButton nextStageButton;

    public UnityEvent<int> OnMonsterCountChange;

    public override IEnumerator LoadingRoutine()
    {
        // test..
        isEditor = false;

        // ĳ���� �ʱ�ȭ
        Manager.Game.PlayerData.Hp = Manager.Game.PlayerData.MaxHp;
        Manager.Game.Inventory.InventoryItem[0].ItemCount = Manager.Game.Inventory.InventoryItem[0].MaxItemCount;
        yield return null;

        nextStageButton = FindObjectOfType<NextStageButton>();
        yield return null;

        statues = FindObjectsOfType<Statue>();
        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].OnInsert += PuzzleClear;
        }
        yield return null;

        monsters = FindObjectsOfType<Monster>();
        monsterCount = monsters.Length; // �� �ʿ� �ִ� ���� ��
        OnMonsterCountChange?.Invoke(monsterCount);
        for (int i = 0; i < monsterCount; i++)
        {
            monsters[i].OnDead += PuzzleStart;
        }
        yield return null;

        if (monsterCount == 0) // ���Ͱ� ������ ���� ����
        {
            monsterCount++;
            PuzzleStart();
        }
    }

    // test..
    public bool isEditor;
    IEnumerator EditorRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (isEditor)
            yield return LoadingRoutine();
    }
    private void Start()
    {
        if (isEditor)
        {
            StartCoroutine(EditorRoutine());
        }
    }

    private void PuzzleStart()
    {
        monsterCount--;
        OnMonsterCountChange?.Invoke(monsterCount);
        if (monsterCount == 0)
        {
            if (statues.Length == 0) // ���͸� �� ���� �� ������ ������ Ŭ����
            {
                nextStageButton.CanPush = true;
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
                nextStageButton.CanPush = true;
            }
        }
        else
        {
            insertStatueCount--;
        }
    }
}
