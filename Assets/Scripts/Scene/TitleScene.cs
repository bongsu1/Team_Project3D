using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField] GameObject settingUI;

    private bool doLoading;

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void GameStart()
    {
        if (doLoading)
            return;

        doLoading = true;
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
