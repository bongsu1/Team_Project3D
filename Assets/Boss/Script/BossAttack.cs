using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] LayerMask playerlayer;
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerlayer) != 0)
        {
            IDamagable player = other.GetComponent<IDamagable>();
            player?.TakeDamage(damage);
            Debug.Log(other.name);
            Debug.Log(damage);
            Debug.Log($"플레이어 남은 체력 :{Manager.Game.PlayerData.Hp}");
        }
    }
}
