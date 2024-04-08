using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    [Header("HP UI")]
    [SerializeField] Image[] hpImage;

    [Header("Potion UI")]
    [SerializeField] TMP_Text potionText;
    [SerializeField] Image blackPotionImage; // 포션을 다 쓰면 검은색으로 바뀜

    private void OnEnable()
    {
        Manager.Game.PlayerData.OnChangeHp += ChangeHPBar;
        Manager.Game.Inventory.InventoryItem[0].OnChangeItemCount += ChangePotionCount;
    }

    private void Start()
    {
        StartCoroutine(LoadingRoutine()); // test..
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

    public override IEnumerator LoadingRoutine()
    {
        ChangeHPBar(Manager.Game.PlayerData.Hp);
        ChangePotionCount(Manager.Game.Inventory.InventoryItem[0].ItemCount);
        yield return null;
    }
}
