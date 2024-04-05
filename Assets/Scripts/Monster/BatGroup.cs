using UnityEngine;
using UnityEngine.Events;

public class BatGroup : MonoBehaviour
{
    [SerializeField] SphereCollider searchRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float searchDistance;
    [SerializeField] Bat[] bats;

    public UnityEvent<Transform> OnFindPlayer;

    private void Awake()
    {
        searchRange.radius = searchDistance;
    }

    private void Start()
    {
        bats = GetComponentsInChildren<Bat>();
        if (bats.Length != 0)
        {
            foreach (Bat bat1 in bats)
            {
                bat1.SearchDistance = searchDistance;
                foreach (Bat bat2 in bats)
                {
                    if (bat1 == bat2)
                        continue;

                    Physics.IgnoreCollision(bat1.GetComponent<Collider>(), bat2.GetComponent<Collider>());
                }
            }
        }
    }

    int triggerCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (triggerCount == 1)
            return;

        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            triggerCount++;
            searchRange.enabled = false;
            OnFindPlayer?.Invoke(other.transform);
        }
    }
}
