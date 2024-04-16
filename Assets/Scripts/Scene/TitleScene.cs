using System.Collections;
using UnityEngine;

public class TitleScene : BaseScene
{
    [Header("UI")]
    [SerializeField] GameObject settingUI;
    [Header("Sound")]
    [SerializeField] AudioClip bgm;
    [SerializeField] AudioClip buttonSound;

    private bool doLoading;

    private void Start()
    {
        Manager.Sound.PlayBGM(bgm);
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }

    public void GameStart()
    {
        if (doLoading)
            return;

        Manager.Sound.PlaySFX(buttonSound);
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
        Manager.Sound.PlaySFX(buttonSound);
        settingUI.SetActive(true);
    }

    [ContextMenu("Stage2")]
    public void DebugStage2()
    {
        if (doLoading)
            return;

        Manager.Sound.PlaySFX(buttonSound);
        doLoading = true;
        Manager.Scene.LoadScene("SecondStageScene");
    }
}
