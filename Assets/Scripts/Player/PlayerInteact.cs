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
            for (int i = 0; i < size; i++)
            {
                IInteractable interactable = others[i].GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(inventory);
                    return;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactPoint.position, interactRange);
    }
}
