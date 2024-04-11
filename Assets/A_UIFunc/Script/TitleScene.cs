using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField] GameObject settingUI;

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void GameStart()
    {
        Manager.Scene.LoadScene("FirstStageScene");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void OpenSettingWindow()
    {
        // test..
        settingUI.SetActive(true);
    }
}
