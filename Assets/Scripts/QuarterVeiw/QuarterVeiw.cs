using UnityEngine;

public class QuarterVeiw : MonoBehaviour
{
    [SerializeField] LayerMask WallLayer;
    [SerializeField] Transform follow;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        transform.position = follow.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & WallLayer) != 0)
        {
            MeshRenderer[] meshs = other.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & WallLayer) != 0)
        {
            MeshRenderer[] meshs = other.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.enabled = true;
            }
        }
    }
}
