using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField]
    private EFruitType FruitType;
    [SerializeField]
    private EItemType ResultType;
    [SerializeField]
    private PlayerMovementHandler Player;
    [SerializeField]
    private float RawToProductDuration = 5f;
    private float CurrentProcessingTime = 0f;

    private bool IsInteractingWithPlayer = false;
    private float CooldownTime = 0f;
    private float CooldownDuration = 0.5f;

    private Dictionary<EItemType, int> ItemCount = new Dictionary<EItemType, int>();
    private readonly InventoryKey inventoryKey;

    public Shelf()
    {
        ItemCount[EItemType.Raw] = 0;
        ItemCount[EItemType.Juice] = 0;
        ItemCount[EItemType.Jelly] = 0;
        inventoryKey = new InventoryKey(ResultType, FruitType);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemCount[ResultType] = 10;
        ItemCount[EItemType.Raw] = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (ItemCount[EItemType.Raw] > 0)
        {
            CurrentProcessingTime += Time.deltaTime;
            if (CurrentProcessingTime >= RawToProductDuration)
            {
                CurrentProcessingTime = 0f;
                ItemCount[EItemType.Raw]--;
                ItemCount[ResultType]++;
                Debug.Log("Produce one: " + inventoryKey);
            }
        }
        if (IsInteractingWithPlayer && ItemCount[ResultType] > 0)
        {
            if (CooldownTime <= 0f)
            {
                if (Player.ReceiveItem(inventoryKey))
                {
                    Debug.Log("Gave item to player: " + inventoryKey);
                    ItemCount[ResultType]--;
                }
                CooldownTime = CooldownDuration;
            }
            else
            {
                CooldownTime -= Time.deltaTime;
            }
        }
    }


    public void StartInteract()
    {
        IsInteractingWithPlayer = true;
        CooldownTime = CooldownDuration;

    }

    public void StopInteract()
    {
        IsInteractingWithPlayer = false;
    }
}