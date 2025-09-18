using System;

[System.Serializable] // Makes the class visible in the Inspector if used in a MonoBehaviour
public class PlayerInventory
{
    private GameData.PlayerInventoryData PlayerInventoryData;

    public PlayerInventory(GameData.PlayerInventoryData playerInventoryData)
    {
        PlayerInventoryData = playerInventoryData;
        foreach (EItemType itemType in Enum.GetValues(typeof(EItemType)))
        {
            foreach (EFruitType fruitType in Enum.GetValues(typeof(EFruitType)))
            {
                var key = new FullProductId(itemType, fruitType);
                PlayerInventoryData.content[key] = 0;
            }
        }
    }



    private int GetTotalItemCount()
    {
        int total = 0;
        foreach (var count in PlayerInventoryData.content.Values)
        {
            total += count;
        }
        return total;
    }

    public bool AddItem(FullProductId item, int count)
    {
        int currentItemCount = GetTotalItemCount();
        if (currentItemCount >= PlayerInventoryData.capacity || count <= 0)
        {
            return false;
        }

        if (currentItemCount + count > PlayerInventoryData.capacity)
        {
            count = PlayerInventoryData.capacity - currentItemCount;
        }

        PlayerInventoryData.content[item] += count;
        return true;
    }

    public bool HasItem(FullProductId inventoryKey) => PlayerInventoryData.content[inventoryKey] > 0;
    public bool IsFull() => GetTotalItemCount() >= PlayerInventoryData.capacity;

    public bool IncrementItem(FullProductId item)
    {
        if (IsFull())
        {
            return false;
        }
        PlayerInventoryData.content[item]++;
        return true;
    }

    public bool DecrementItem(FullProductId item)
    {
        if (PlayerInventoryData.content[item] <= 0) { return false; }
        PlayerInventoryData.content[item]--;
        return true;
    }
}