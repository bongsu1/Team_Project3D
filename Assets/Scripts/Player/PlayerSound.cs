using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public enum S_Type { FirstAttack, ThirdAttack, Bowstring, ArrowShot, Dash, UseHealPotion, Respawn }
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] S_Type type;

    public void PlaySound(S_Type sound)
    {
        Manager.Sound.PlaySFX(audioClips[(int)sound]);
    }
}
