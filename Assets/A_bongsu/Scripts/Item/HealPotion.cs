using System.Collections;
using UnityEngine;

public class HealPotion : Item
{
    [Header("HealPotion")]
    [SerializeField] int healValue;
    [SerializeField] float healDelay;
    [SerializeField] float coolTime;

    Coroutine healRoutine;
    private bool canUse;

    private void Start()
    {
        canUse = true;
    }

    public override void ItemUse()
    {
        if (!canUse || (Manager.Game.PlayerData.Hp == Manager.Game.PlayerData.MaxHp) || itemCount == 0)
            return;

        healRoutine = StartCoroutine(HealRoutine());
    }

    IEnumerator HealRoutine()
    {
        canUse = false;
        yield return new WaitForSeconds(healDelay);
        Manager.Game.PlayerData.Hp += healValue;
        itemCount--;
        Debug.Log($"물약 회복량 : {healValue}\n플레이어 체력 {Manager.Game.PlayerData.Hp}");
        yield return new WaitForSeconds(coolTime);
        canUse = true;
    }

    public void StopHealRoutine()
    {
        StopCoroutine(healRoutine);
        StartCoroutine(CancelRoutine());
    }

    IEnumerator CancelRoutine()
    {
        yield return new WaitForSeconds(coolTime);
        canUse = true;
    }
}
