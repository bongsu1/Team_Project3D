using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    /// <summary>
    /// damage�� ���� ������ �Է�
    /// ex) hp -= damage;
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage);
}