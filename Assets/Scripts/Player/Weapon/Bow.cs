using System.Collections;
using UnityEngine;

public class Bow : Weapon
{
    [Header("Bow")]
    [SerializeField] Transform arrowShotPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float chargedTime; // ��¡ �ð�
    public float ChargedTime => chargedTime;
    [SerializeField] LayerMask collisionLayer;

    [Header("Arrow")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int BounceCount;
    [Header("Arrow Damage")]
    [SerializeField] int normalDamage; // ȭ����ݷ�
    [SerializeField] int chargedDamage;
    [SerializeField] int fullChargedDamage;
    [Header("Arrow Range")]
    [SerializeField] float normalRange; // ��Ÿ�
    [SerializeField] float chargedRange;
    [Header("Arrow Speed")]
    [SerializeField] float normalSpeed; // ȭ�� �ӵ�
    [SerializeField] float chargedSpeed;

    bool canShot = true;
    public bool CanShot { get { return canShot; } set { canShot = value; } }
    RaycastHit hit;
    Coroutine chagingRoutine;
    float lineRenderRange;
    bool isCharged;

    private void FixedUpdate()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, arrowShotPoint.position);
            if (Physics.Raycast(arrowShotPoint.position, arrowShotPoint.forward, out hit, lineRenderRange, collisionLayer))
            {
                lineRenderer.SetPosition(1, hit.point);

                float amountRange = lineRenderRange - Vector3.Distance(hit.point, arrowShotPoint.position);
                Vector3 hitPoint = hit.point;
                Vector3 nextDir = Vector3.Reflect(arrowShotPoint.forward, hit.normal);
                lineRenderer.positionCount = 3;

                if (Physics.Raycast(hitPoint, nextDir, out hit, amountRange, collisionLayer))
                {
                    lineRenderer.SetPosition(2, hit.point);
                }
                else
                {
                    lineRenderer.SetPosition(2, hitPoint + nextDir * amountRange);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, arrowShotPoint.position + arrowShotPoint.forward * lineRenderRange);
            }
        }
    }

    /// <summary>
    /// onClick ���콺 ��ư �Է¿���
    /// isCharged ���° ��������
    /// </summary>
    /// <param name="onClick"></param>
    /// <param name="charged"></param>
    public void Use(bool onClick, int charged)
    {
        if (onClick)
        {
            lineRenderer.SetPosition(0, arrowShotPoint.position);
            lineRenderer.SetPosition(1, arrowShotPoint.position);

            lineRenderer.enabled = true;
            lineRenderRange = normalRange;
            chagingRoutine = StartCoroutine(ChagingRoutine());
        }
        else
        {
            Cancel();

            Arrow arrow =
                Instantiate(arrowPrefab, arrowShotPoint.position, arrowShotPoint.rotation).GetComponent<Arrow>();
            switch (charged)
            {
                case 0:
                    arrow.ArrowRange = normalRange;
                    arrow.ArrowSpeed = normalSpeed;
                    arrow.Damage = normalDamage;
                    arrow.BounceCount = 0;
                    break;
                case 1:
                    arrow.ArrowRange = chargedRange;
                    arrow.ArrowSpeed = chargedSpeed;
                    arrow.Damage = chargedDamage;
                    arrow.BounceCount = BounceCount;
                    break;
                case 2:
                    arrow.ArrowRange = chargedRange;
                    arrow.ArrowSpeed = chargedSpeed;
                    arrow.Damage = fullChargedDamage;
                    arrow.BounceCount = BounceCount;
                    break;
                default: // do nothing
                    break;
            }
        }
    }

    public void Cancel()
    {
        lineRenderer.enabled = false;
        if (chagingRoutine != null)
        {
            StopCoroutine(chagingRoutine);
            chagingRoutine = null;
        }
        lineRenderer.widthMultiplier = 1;
        lineRenderRange = normalRange;
        isCharged = false;
    }

    IEnumerator ChagingRoutine()
    {
        yield return new WaitForSeconds(chargedTime);
        lineRenderer.widthMultiplier = 2;
        lineRenderRange = chargedRange;
        isCharged = true;
    }
}
