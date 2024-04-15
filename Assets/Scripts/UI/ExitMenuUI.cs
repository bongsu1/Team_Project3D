using UnityEngine;
using UnityEngine.InputSystem;

public class ExitMenuUI : PopUpUI
{
    [SerializeField] AudioClip buttonSound;
    PlayerInput playerInput;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        playerInput = FindObjectOfType<Player>().GetComponentInChildren<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.enabled = true;
    }

    public void ExitGameScene()
    {
        Manager.Scene.LoadScene("TitleScene");
        Manager.UI.ClearPopUpUI();
    }

    public void PlayButtonClickSound()
    {
        Manager.Sound.PlaySFX(buttonSound);
    }
}
