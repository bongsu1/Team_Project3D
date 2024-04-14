using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] GameObject[] hitEffects;
    [SerializeField] AudioClip hitSound;

    public void PlayHitEffect(Vector3 hitPoint)
    {
        foreach (GameObject hitEffect in hitEffects)
        {
            if (!hitEffect.activeSelf)
            {
                StartCoroutine(PlayRoutine(hitEffect, hitPoint));
                return;
            }
        }
    }

    public void PlayHitSound()
    {
        Manager.Sound.PlaySFX(hitSound);
    }

    IEnumerator PlayRoutine(GameObject hitEffect, Vector3 hitPoint)
    {
        ParticleSystem effect = hitEffect.GetComponent<ParticleSystem>();
        hitEffect.transform.parent = null;
        hitEffect.transform.position = hitPoint;
        hitEffect.SetActive(true);
        PlayHitSound();
        yield return new WaitUntil(() => !effect.isPlaying);
        hitEffect.SetActive(false);
        hitEffect.transform.parent = transform;
    }
}
