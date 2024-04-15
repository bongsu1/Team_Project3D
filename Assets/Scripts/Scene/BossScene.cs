using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossScene : BaseScene
{
    [SerializeField] float cutSceneTime;
    [SerializeField] CinemachineVirtualCamera playerFollow;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject quaterView;
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject bossUI;

    public override IEnumerator LoadingRoutine()
    {
        Manager.Game.PlayerData.Hp = Manager.Game.PlayerData.MaxHp;
        Manager.Game.Inventory.InventoryItem[0].ItemCount = Manager.Game.Inventory.InventoryItem[0].MaxItemCount;
        yield return null;
    }

    private void Start()
    {
        playerInput.enabled = false;
        StartCoroutine(BossCutSceneRoutine());
    }

    IEnumerator BossCutSceneRoutine()
    {
        yield return new WaitForSeconds(cutSceneTime);
        playerInput.enabled = true;
        playerFollow.Priority = 20;
        quaterView.SetActive(true);
        mainUI.SetActive(true);
        bossUI.SetActive(true);
    }
}
