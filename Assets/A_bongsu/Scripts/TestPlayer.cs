using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer : MonoBehaviour, IDamagable
{
    [SerializeField] Rigidbody rigid;

    Vector3 moveDir;

    public void TakeDamage(int damage)
    {
        Debug.Log($"�÷��̾ ���� ������ :{damage}");
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
}
