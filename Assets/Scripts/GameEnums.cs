
using System;
using UnityEngine;


[System.Serializable]
public enum EItemType
{
    Raw, Juice, Jelly
}

[System.Serializable]
public enum EFruitType
{
    Apple, Orange, Pineapple
}

[System.Serializable]
public enum EStandType
{
    Mixer, MixerFinishedProduct, StoreShelf
}

[System.Serializable]
public class FullProductId
{
    [SerializeField]
    public EItemType item;
    [SerializeField]
    public EFruitType fruit;

    public FullProductId(EItemType Item, EFruitType Fruit)
    {
        item = Item;
        fruit = Fruit;
    }

    public override bool Equals(object obj)
    {
        if (obj is not FullProductId other)
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


[System.Serializable]
public class StandId
{
    [SerializeField]
    public EStandType StandType;
    [SerializeField]
    public FullProductId ProductId;
    public StandId(EStandType standType, FullProductId productId)
    {
        StandType = standType;
        ProductId = productId;
    }

    public override bool Equals(object obj)
    {
        if (obj is not StandId other)
        {
            return false;
        }
        return StandType == other.StandType && ProductId == other.ProductId;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(StandType, ProductId);
    }
}

