using System.Collections;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public enum E_Type { FirstAttack, SecondAttack, ThirdAttack, Dash, ChargedBow, ChargedShot, FullChargeShot, UseHealPotion, Respawn }
    [SerializeField] GameObject[] effects;
    [SerializeField] E_Type type;

    [Header("Charged Effect")]
    [SerializeField] float multiSize;
    Vector3 chargeEffectNormalSize; // 차지 이펙트 원래 크기

    private void Start()
    {
        chargeEffectNormalSize = effects[(int)E_Type.ChargedBow].transform.localScale;
    }

    public void PlayEffect(E_Type effect)
    {
        StartCoroutine(PlayRoutine(effect));
    }

    public void StopChargeEffect()
    {
        ParticleSystem chargeEffect = effects[(int)E_Type.ChargedBow].GetComponent<ParticleSystem>();
        chargeEffect.Stop();
        chargeEffect.transform.localScale = chargeEffectNormalSize;
    }

    public void FullChargeEffect()
    {
        ParticleSystem chargeEffect = effects[(int)E_Type.ChargedBow].GetComponent<ParticleSystem>();
        chargeEffect.transform.localScale *= multiSize;
    }

    IEnumerator PlayRoutine(E_Type effectType)
    {
        int curIndex = (int)effectType;
        ParticleSystem curEffect = effects[curIndex].GetComponent<ParticleSystem>();
        switch (effectType)
        {
            case E_Type.FirstAttack:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => !curEffect.isPlaying);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.SecondAttack:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => !curEffect.isPlaying);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.ThirdAttack:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => !curEffect.isPlaying);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.Dash:
                effects[curIndex].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.ChargedBow:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => curEffect.isStopped);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.ChargedShot:
                effects[curIndex].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.FullChargeShot:
                effects[curIndex].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.UseHealPotion:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => !curEffect.isPlaying);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            case E_Type.Respawn:
                effects[curIndex].SetActive(true);
                yield return new WaitUntil(() => !curEffect.isPlaying);
                curEffect = null;
                effects[curIndex].SetActive(false);
                break;

            default: // do nothing
                break;
        }
    }
}
