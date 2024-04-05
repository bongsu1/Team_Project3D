using UnityEngine;

public class TestBaseCampRenderer : MonoBehaviour
{
    [SerializeField] BoxCollider baseCampRange;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, baseCampRange.size);
    }
}
