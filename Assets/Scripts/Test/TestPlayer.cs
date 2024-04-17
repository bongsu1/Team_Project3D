using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer : MonoBehaviour, IDamagable
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Inventory inventory;

    Vector3 moveDir;

    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        TakeDamage(2);
    }

    [ContextMenu("Test FillPotion")]
    public void RestTest()
    {
        inventory.FillHealpotion();
    }

    public void TakeDamage(int damage)
    {
        Manager.Game.PlayerData.Hp -= damage;
        Debug.Log($"�÷��̾ ���� ������ : {damage}\n���� ü��{Manager.Game.PlayerData.Hp}");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigid.velocity = new Vector3(moveDir.x * 5f, rigid.velocity.y, moveDir.z * 5f);
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputDir = value.Get<Vector2>();
        moveDir.x = inputDir.x;
        moveDir.z = inputDir.y;
    }

    private void OnItem1()
    {
        // test..
        Debug.Log("���");
        inventory.ItmeUse();
    }
}
