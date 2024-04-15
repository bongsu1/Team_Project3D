using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : BaseUI
{
    [Header("HP UI")]
    [SerializeField] Image[] hpImage;

    [Header("Potion UI")]
    [SerializeField] TMP_Text potionText;
    [SerializeField] Image blackPotionImage; // 포션을 다 쓰면 검은색으로 바뀜

    [Header("ExitMenu UI")]
    [SerializeField] ExitMenuUI exitMenuUI;

    [Header("Player Dead")]
    [SerializeField] GameObject deadUI;

    [Header("Monster Count")]
    [SerializeField] TMP_Text monsterCount;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        exitMenuUI = Manager.Resource.Load<ExitMenuUI>("UI/End(2)");
        Manager.Game.PlayerData.OnChangeHp += ChangeHPBar;
        Manager.Game.Inventory.InventoryItem[0].OnChangeItemCount += ChangePotionCount;
    }

    private void Start()
    {
        ChangeHPBar(Manager.Game.PlayerData.Hp);
        ChangePotionCount(Manager.Game.Inventory.InventoryItem[0].ItemCount);
    }

    private void OnDisable()
    {
        Manager.Game.PlayerData.OnChangeHp -= ChangeHPBar;
        Manager.Game.Inventory.InventoryItem[0].OnChangeItemCount -= ChangePotionCount;
    }

    private void ChangeHPBar(int hp)
    {
        for (int i = 0; i < hpImage.Length; i++)
        {
            if (i >= hp)
                hpImage[i].enabled = false;
            else
                hpImage[i].enabled = true;
        }
    }

    private void ChangePotionCount(int count)
    {
        potionText.text = count.ToString();
        if (count == 0)
        {
            blackPotionImage.enabled = true;
        }
        else
        {
            blackPotionImage.enabled = false;
        }
    }

    public void ShowDeadUI()
    {
        StartCoroutine(ShowDeadUIRoutine());
    }

    IEnumerator ShowDeadUIRoutine()
    {
        yield return new WaitForSeconds(2f);
        deadUI.SetActive(true);
    }

    public void OnEscape()
    {
        Manager.UI.ShowPopUpUI(exitMenuUI);
    }

    public void ChangeMonsterCount(int count)
    {
        monsterCount.text = count.ToString();
    }
}
