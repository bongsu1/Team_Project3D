using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BossScene : BaseScene
{
    [SerializeField] float cutSceneTime;
    [SerializeField] CinemachineVirtualCamera playerFollow;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject bossUI;
    [SerializeField] MeshRenderer[] wallMesh;

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
        mainUI.SetActive(true);
        bossUI.SetActive(true);
        foreach(MeshRenderer mesh in wallMesh)
        {
            mesh.enabled = false;
        }
    }
}
