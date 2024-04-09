using UnityEngine;

public class ExitMenuUI : PopUpUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void ExitGameScene()
    {
        Debug.Log("게임 나가기");
    }
}
