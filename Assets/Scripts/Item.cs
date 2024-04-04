using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] int maxItemCount;
    [SerializeField] protected int itemCount;
    public int MaxItemCount { get { return maxItemCount; } set { maxItemCount = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }

    public virtual void ItemUse()
    {

    }
}
