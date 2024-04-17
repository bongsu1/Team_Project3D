using TMPro;
using UnityEngine;

public class TestHPUI : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;

    private void Start()
    {
        Manager.Game.PlayerData.OnChangeHp += ChangeText;
        hpText.text = Manager.Game.PlayerData.Hp.ToString();
    }

    public void ChangeText(int hp)
    {
        hpText.text = hp.ToString();
    }
}
