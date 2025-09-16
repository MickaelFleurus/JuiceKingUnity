using System;
using System.Collections.Generic;

[System.Serializable] // Makes the class visible in the Inspector if used in a MonoBehaviour
public class PlayerInventory
{
    private Dictionary<InventoryKey, int> mStorage;
    public int mCapacity { get; private set; }

    public PlayerInventory(int capacity)
    {
        mCapacity = capacity;
        mStorage = new Dictionary<InventoryKey, int>();

        foreach (EItemType itemType in Enum.GetValues(typeof(EItemType)))
        {
            foreach (EFruitType fruitType in Enum.GetValues(typeof(EFruitType)))
            {
                var key = new InventoryKey(itemType, fruitType);
                mStorage[key] = 0;
            }
        }
    }



    private int GetTotalItemCount()
    {
        int total = 0;
        foreach (var count in mStorage.Values)
        {
            total += count;
        }
        return total;
    }

    public bool AddItem(InventoryKey item, int count)
    {
        int currentItemCount = GetTotalItemCount();
        if (currentItemCount == mCapacity || count <= 0)
        {
            return false;
        }

        if (currentItemCount + count > mCapacity)
        {
            count = mCapacity - currentItemCount;
        }

        mStorage[item] += count;
        return true;
    }

    public bool HasItem(InventoryKey inventoryKey) => mStorage[inventoryKey] > 0;
    public bool IsFull() => GetTotalItemCount() >= mCapacity;

    public bool IncrementItem(InventoryKey item)
    {
        if (IsFull())
        {
            return false;
        }
        mStorage[item]++;
        return true;
    }

    public bool DecrementItem(InventoryKey item)
    {
        if (mStorage[item] <= 0) { return false; }
        mStorage[item]--;
        return true;
    }
}