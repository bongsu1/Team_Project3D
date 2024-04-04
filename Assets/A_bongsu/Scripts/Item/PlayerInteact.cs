using UnityEngine;

public class PlayerInteact : MonoBehaviour
{
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Transform interactPoint;
    [SerializeField] Inventory inventory;
    [SerializeField] float interactRange;

    Collider[] others = new Collider[20];
    private void OnInteraction()
    {
        int size = Physics.OverlapSphereNonAlloc(interactPoint.position, interactRange, others, interactableLayer);
        if (size > 0)
        {
            IInteractable interactable = others[0].GetComponent<IInteractable>();
            interactable?.Interact(inventory);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactPoint.position, interactRange);
    }
}
