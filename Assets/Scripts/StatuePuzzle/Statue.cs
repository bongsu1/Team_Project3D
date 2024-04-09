using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Statue : MonoBehaviour, IDamagable
{
    [Header("Insert Number")]
    [SerializeField] int statueNumber;
    public int StatueNumber => statueNumber;

    [Header("Move")]
    [SerializeField] Transform player;
    [SerializeField] Transform[] hitPoint;
    [SerializeField] float moveDistance;
    [SerializeField] float moveTime;
    [SerializeField] LayerMask wallLayer;

    [Header("Component")]
    [SerializeField] Light soptLight;

    public UnityAction<bool> OnInsert; // 게임씬에 맞춰 졌다고 전달

    bool isInsert;
    public bool IsInsert { get { return isInsert; } set { isInsert = value; OnInsert?.Invoke(value); } }
    Coroutine moveRoutine;

    private void OnEnable()
    {
        soptLight.enabled = true;
        player = FindObjectOfType<Player>().transform;
    }

    // test..
    public bool isStatic;
    public void TakeDamage(int damage)
    {
        if (isStatic)
        {
            if (isInsert)
                return;
        }

        if (moveRoutine != null)
            return;

        float distance = float.MaxValue;
        int index = 0;
        for (int i = 0; i < hitPoint.Length; i++)
        {
            if (distance > Vector3.Distance(player.position, hitPoint[i].position))
            {
                distance = Vector3.Distance(player.position, hitPoint[i].position);
                index = i;
            }
        }
        moveRoutine = StartCoroutine(MoveRoutine((transform.position - hitPoint[index].position).normalized));
    }

    Vector3 nextVec;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 2f, transform.position + nextVec * moveDistance + Vector3.up * 2f);
    }

    IEnumerator MoveRoutine(Vector3 moveDir)
    {
        nextVec = moveDir;
        if (Physics.Raycast(transform.position + Vector3.up * 2f, moveDir, moveDistance, wallLayer))
        {
            moveDir = Vector3.zero;
        }
        Vector3 curPoint = transform.position;
        Vector3 nextPoint = transform.position + moveDir * moveDistance;
        float time = 0;
        while (time <= 1)
        {
            transform.position = Vector3.Lerp(curPoint, nextPoint, time);
            time += Time.deltaTime / moveTime;
            yield return null;
        }
        moveRoutine = null;
    }
}
