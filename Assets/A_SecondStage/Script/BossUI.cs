using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;

    public void ChangeHPBar(float value)
    {
        hpBar.value = value;
    }
}
