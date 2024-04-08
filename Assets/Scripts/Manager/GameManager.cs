using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerData playerData;
    public PlayerData PlayerData => playerData;

    [SerializeField] Inventory inventory;
    public Inventory Inventory => inventory;
}
