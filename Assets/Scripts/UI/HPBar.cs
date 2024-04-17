using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HPBar : BillboardUI
{
    [SerializeField] Slider hpBar;
    [SerializeField] Monster monster;
    [SerializeField] Vector3 offset;

    private void OnEnable()
    {
        NavMeshAgent agent = transform.parent.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            offset = new Vector3(0f, agent.height * transform.parent.localScale.y + offset.y, 0f);
        }
    }

    public Monster Monster
    {
        get { return monster; }
        set
        {
            monster = value;
            monster.OnChangeHP += ChangeHP;
            transform.position = transform.parent.position + offset;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// hp = curHP / maxHP
    /// </summary>
    /// <param name="hp"></param>
    private void ChangeHP(float hp)
    {
        hpBar.value = hp;
        if (hp <= 0)
        {
            StartCoroutine(InvisibleRoutine());
        }
    }

    IEnumerator InvisibleRoutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
