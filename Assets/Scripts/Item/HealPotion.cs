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
        if (!canUse || (Manager.Game.PlayerData.Hp == Manager.Game.PlayerData.MaxHp) || ItemCount == 0)
            return;

        healRoutine = StartCoroutine(HealRoutine());
    }

    IEnumerator HealRoutine()
    {
        canUse = false;
        yield return new WaitForSeconds(healDelay);
        Manager.Game.PlayerData.Hp += healValue;
        --ItemCount;
        Debug.Log($"물약 갯수:{ItemCount}");
        Debug.Log($"물약 회복량 : {healValue}\n플레이어 체력 {Manager.Game.PlayerData.Hp}");
        yield return new WaitForSeconds(coolTime);
        canUse = true;
    }

    public void StopHealRoutine()
    {
        if (healRoutine != null)
        {
            StopCoroutine(healRoutine);
            StartCoroutine(CancelRoutine());
            healRoutine = null;
        }
    }

    IEnumerator CancelRoutine()
    {
        yield return new WaitForSeconds(coolTime);
        canUse = true;
    }
}
