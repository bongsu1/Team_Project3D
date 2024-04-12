using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] GameObject[] hitEffects;

    public void PlayHitEffect(Vector3 hitPoint)
    {
        foreach(GameObject hitEffect in hitEffects)
        {
            if (!hitEffect.activeSelf)
            {
                StartCoroutine(PlayRoutine(hitEffect, hitPoint));
                return;
            }
        }
    }

    IEnumerator PlayRoutine(GameObject hitEffect, Vector3 hitPoint)
    {
        ParticleSystem effect = hitEffect.GetComponent<ParticleSystem>();
        hitEffect.transform.parent = null;
        hitEffect.transform.position = hitPoint;
        hitEffect.SetActive(true);
        yield return new WaitUntil(() => !effect.isPlaying);
        hitEffect.SetActive(false);
        hitEffect.transform.parent = transform;
    }
}
