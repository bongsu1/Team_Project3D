using System.Collections;
using UnityEngine;

public class TestMonster : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        //Debug.Log($"���� ������ : {damage}");

        StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mesh.material.color = Color.white;
    }
}
