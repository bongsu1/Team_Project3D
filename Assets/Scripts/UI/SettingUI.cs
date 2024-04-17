using UnityEngine;

public class SettingUI : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;

    public void Close()
    {
        Manager.Sound.PlaySFX(buttonSound);
        gameObject.SetActive(false);
    }
}
