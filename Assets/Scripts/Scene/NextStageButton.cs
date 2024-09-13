using UnityEngine;

public class NextStageButton : MonoBehaviour, IInteractable
{
    private enum Scene { Title, FirstStage, SecondStage, BossStage };
    [Header("NextScene")]
    [SerializeField] Scene loadScene;
    [SerializeField] LayerMask playerLayer;

    [Header("Test")]
    [SerializeField] bool isButton;

    private bool canPush;
    public bool CanPush
    {
        set
        {
            canPush = value;
            if (value)
                GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }

    // 밟거나 누르면 다음 씬으로
    [ContextMenu("Next Scene")]
    private void LoadNextStage()
    {
        switch (loadScene)
        {
            case Scene.Title:
                Manager.Scene.LoadScene("TitleScene");
                break;
            case Scene.FirstStage:
                Manager.Scene.LoadScene("FirstStageScene");
                break;
            case Scene.SecondStage:
                Manager.Scene.LoadScene("SecondStageScene");
                break;
            case Scene.BossStage:
                Manager.Scene.LoadScene("BossStageScene");
                break;
            default: // do nothing
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canPush)
            return;

        if (isButton)
            return;

        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            canPush = false;
            LoadNextStage();
        }
    }

    public void Interact()
    {
        if (!canPush)
            return;

        if (!isButton)
            return;

        canPush = false;
        LoadNextStage();
    }
}
