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

    [Header("Clear Test")]
    [SerializeField] PopUpUI clearUI;

    public override IEnumerator LoadingRoutine()
    {
        // 몬스터 스폰 로직 같은 씬로딩때 미리 해야될것들

        // 캐릭터 초기화
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
        monsterCount = monsters.Length; // 이 맵에 있는 몬스터 수
        if (monsterCount == 0) // 몬스터가 없으면 퍼즐 시작
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
            if (statues.Length == 0) // 몬스터를 다 제거 후 석상이 없으면 클리어
            {
                // 클리어 후 다음씬 버튼 활성화
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
                // 클리어 후 다음씬 버튼 활성화
            }
        }
        else
        {
            insertStatueCount--;
        }
    }
}
