using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] int key;
    private bool isOpen;

    public void Interact(Inventory inventory)
    {
        if (isOpen)
            return;

        isOpen = true;
        StartCoroutine(OpenRoutine(top.position));
        inventory.GetKeyItem(key);
        int potion = Random.Range(0, 5);
        inventory.FillHealpotion(potion);
    }

    // test
    [SerializeField] Transform top;
    IEnumerator OpenRoutine(Vector3 curPos)
    {
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * 0.25f;
            top.position = Vector3.Lerp(curPos, curPos + Vector3.up, time);
            yield return null;
        }
        // 끝나면 아이템 나옴
    }
}
