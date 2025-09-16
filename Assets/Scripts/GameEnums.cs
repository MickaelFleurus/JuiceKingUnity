
using System;
using UnityEngine;

public enum EItemType
{
    Raw, Juice, Jelly
}
public enum EFruitType
{
    Apple, Orange, Pineapple
}

[System.Serializable]
public class InventoryKey
{
    [SerializeField]
    public EItemType item;
    [SerializeField]
    public EFruitType fruit;

    public InventoryKey(EItemType Item, EFruitType Fruit)
    {
        item = Item;
        fruit = Fruit;
    }

    public override bool Equals(object obj)
    {
        if (obj is not InventoryKey other)
        {
            return false;
        }
        return item == other.item && fruit == other.fruit;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(item, fruit);
    }
}