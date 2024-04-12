using System.Collections;
using UnityEngine;

public class Bow : Weapon
{
    [Header("Bow")]
    [SerializeField] Transform arrowShotPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField]
    float chargedTime; // 차징 시간
    public float ChargedTime => chargedTime;
    [SerializeField] LayerMask collisionLayer;

    [Header("Arrow")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] int BounceCount;
    [Header("Arrow Damage")]
    [SerializeField] int normalDamage; // 화살공격력
    [SerializeField] int chargedDamage;
    [SerializeField] int fullChargedDamage;
    [Header("Arrow Range")]
    [SerializeField] float normalRange; // 사거리
    [SerializeField] float chargedRange;
    [Header("Arrow Speed")]
    [SerializeField] float normalSpeed; // 화살 속도
    [SerializeField] float chargedSpeed;

    bool canShot = true;
    public bool CanShot { get { return canShot; } set { canShot = value; } }
    bool isRayHit;
    RaycastHit hit;
    Coroutine chagingRoutine;
    float lineRenderRange;

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (lineRenderer.enabled)
        {
            lineRenderer.SetPosition(0, arrowShotPoint.position);
            if (isRayHit)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, arrowShotPoint.position + arrowShotPoint.forward * lineRenderRange);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, arrowShotPoint.position);
            lineRenderer.SetPosition(1, arrowShotPoint.position);
        }
    }

    private void FixedUpdate()
    {
        if (lineRenderer.enabled)
        {
            if (Physics.Raycast(arrowShotPoint.position, arrowShotPoint.forward, out hit, lineRenderRange, collisionLayer))
            {
                isRayHit = true;
            }
            else
            {
                isRayHit = false;
            }
        }
    }

    /// <summary>
    /// onClick 마우스 버튼 입력여부
    /// charged 몇번째 충전인지
    /// </summary>
    /// <param name="onClick"></param>
    /// <param name="charged"></param>
    public void Use(bool onClick, int charged)
    {
        if (onClick)
        {
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
    }

    IEnumerator ChagingRoutine()
    {
        yield return new WaitForSeconds(chargedTime);
        lineRenderer.widthMultiplier = 2;
        lineRenderRange = chargedRange;
    }
}
