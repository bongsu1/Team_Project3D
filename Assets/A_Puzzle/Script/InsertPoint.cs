using UnityEngine;

public class InsertPoint : MonoBehaviour
{
    [Header("Insert Material Test")] // test..
    [SerializeField] Material normalMat;
    [SerializeField] Material insertMat;
    [SerializeField] MeshRenderer mesh;

    [Header("Insert Number")]
    [SerializeField] int insertNumber;
    [Header("Statue Layer")]
    [SerializeField] LayerMask statueLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & statueLayer) != 0)
        {
            Statue statue = other.GetComponent<Statue>();
            if (statue != null)
            {
                if (statue.StatueNumber == insertNumber)
                {
                    mesh.material = insertMat;
                    statue.IsInsert = true;
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & statueLayer) != 0)
        {
            Statue statue = other.GetComponent<Statue>();
            if (statue != null)
            {
                if (statue.StatueNumber == insertNumber)
                {
                    mesh.material = normalMat;
                    statue.IsInsert = false;
                }
            }
        }
    }
}
