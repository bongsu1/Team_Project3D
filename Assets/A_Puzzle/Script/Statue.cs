using System.Collections;
using UnityEngine;

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

    bool isInsert;
    public bool IsInsert { get { return isInsert; } set { isInsert = value; } }
    Coroutine moveRoutine;

    private void OnEnable()
    {
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

    IEnumerator MoveRoutine(Vector3 moveDir)
    {
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
