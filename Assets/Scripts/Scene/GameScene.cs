using System.Collections;
using UnityEngine;

public class GameScene : BaseScene
{
    [Header("Puzzle")]
    [SerializeField] Statue[] statues;
    int insertStatueCount; // 맞춰진 스태추 개수

    [Header("Monster")]
    [SerializeField] Monster[] monsters;
    int monsterCount;

    [Header("Next Stage Button")]
    [SerializeField] NextStageButton nextStageButton;

    public override IEnumerator LoadingRoutine()
    {
        // test..
        isEditor = false;

        // 캐릭터 초기화
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
        monsterCount = monsters.Length; // 이 맵에 있는 몬스터 수
        for (int i = 0; i < monsterCount; i++)
        {
            monsters[i].OnDead += PuzzleStart;
        }
        yield return null;

        if (monsterCount == 0) // 몬스터가 없으면 퍼즐 시작
        {
            monsterCount++;
            PuzzleStart();
        }
    }

    // test..
    public bool isEditor;
    private void OnEnable()
    {
        if (isEditor)
        {
            nextStageButton = FindObjectOfType<NextStageButton>();
            statues = FindObjectsOfType<Statue>();
            for (int i = 0; i < statues.Length; i++)
            {
                statues[i].OnInsert += PuzzleClear;
            }
            monsters = FindObjectsOfType<Monster>();
            monsterCount = monsters.Length; // 이 맵에 있는 몬스터 수
            for (int i = 0; i < monsterCount; i++)
            {
                monsters[i].OnDead += PuzzleStart;
            }
            if (monsterCount == 0) // 몬스터가 없으면 퍼즐 시작
            {
                monsterCount++;
                PuzzleStart();
            }
        }
    }

    private void PuzzleStart()
    {
        monsterCount--;
        if (monsterCount == 0)
        {
            if (statues.Length == 0) // 몬스터를 다 제거 후 석상이 없으면 클리어
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
