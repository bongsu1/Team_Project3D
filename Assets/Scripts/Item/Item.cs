using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] int maxItemCount;
    [SerializeField] int itemCount;

    public UnityAction<int> OnChangeItemCount;

    public int MaxItemCount { get { return maxItemCount; } set { maxItemCount = value; } }
    public int ItemCount
    {
        get { return itemCount; }
        set
        {
            itemCount = value;
            if (itemCount > maxItemCount) itemCount = maxItemCount;
            OnChangeItemCount?.Invoke(itemCount);
        }
    }

    public virtual void ItemUse()
    {

    }
}
