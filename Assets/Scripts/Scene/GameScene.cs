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
        monsterCount = monsters.Length;
        if (monsterCount == 0)
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
            for (int i = 0; i < statues.Length; i++)
            {
                statues[i].enabled = true;
                Debug.Log("몬스터를 다 죽임 퍼즐 시작");
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
                Debug.Log("퍼즐 클리어");
                StartCoroutine(ClearRoutine());
            }
        }
        else
        {
            insertStatueCount--;
        }
    }

    IEnumerator ClearRoutine()
    {
        yield return new WaitForSeconds(2f);
        Manager.UI.ShowPopUpUI(clearUI);
    }
}
