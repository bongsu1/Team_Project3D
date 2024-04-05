using System.Collections;
using UnityEngine;

public class QuarterVeiw : MonoBehaviour
{
    [SerializeField] LayerMask WallLayer;
    [SerializeField] float transparencyRate;
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
            MeshRenderer mesh = other.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                StartCoroutine(TransparencyRoutine(mesh.material));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & WallLayer) != 0)
        {
            MeshRenderer mesh = other.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                StartCoroutine(OpacityRoutine(mesh.material));
            }
        }
    }

    IEnumerator TransparencyRoutine(Material mat)
    {
        Color normal = mat.color;
        Color transparencyColor = new Color(normal.r, normal.g, normal.b, transparencyRate);
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * 8;
            mat.color = Color.Lerp(normal, transparencyColor, time);
            yield return null;
        }
    }

    IEnumerator OpacityRoutine(Material mat)
    {
        Color transparencyColor = mat.color;
        Color normal = new Color(transparencyColor.r, transparencyColor.g, transparencyColor.b, 1f);
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * 8;
            mat.color = Color.Lerp(transparencyColor, normal, time);
            yield return null;
        }
    }
}
