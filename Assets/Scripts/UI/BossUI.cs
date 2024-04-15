using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;

    public void ChangeHPBar(float value)
    {
        hpBar.value = value;
    }

    public void Close(float value)
    {
        if (value == 0)
            StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
