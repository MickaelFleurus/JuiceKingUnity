using System.Collections.Generic;
using UnityEngine;

public class FruitMixer : MonoBehaviour
{
    [SerializeField] private EFruitType FruitType;
    [SerializeField] private EItemType ResultType;
    [SerializeField] private float RawToProductDuration = 5f;
    [SerializeField] private AmountTextHandler RawAmountText;
    [SerializeField] private AmountTextHandler ProductAmountText;
    [SerializeField] private SchedulerManager SchedulerManager;

    private PlayerController Player = null;
    private float CurrentProcessingTime = 0f;

    private Scheduler GiveToPlayerScheduler;
    private Scheduler.ScheduledTask GiveToPlayerSchedulerTask;

    private Dictionary<EItemType, int> ItemCount = new Dictionary<EItemType, int>();
    private FullProductId fullProductId;

    public FullProductId rawProductId { get; private set; }

    public void Awake()
    {
        ItemCount[EItemType.Raw] = 0;
        ItemCount[EItemType.Juice] = 0;
        ItemCount[EItemType.Jelly] = 0;
        fullProductId = new FullProductId(ResultType, FruitType);
        rawProductId = new FullProductId(EItemType.Raw, FruitType);

        GiveToPlayerScheduler = SchedulerManager.CreateScheduler(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemCount[ResultType] = 10;
        ItemCount[EItemType.Raw] = 5;

        RefreshRawItemCount();
        RefreshProductItemCount();
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

                RefreshRawItemCount();
                RefreshProductItemCount();
            }
        }

    }

    private void GiveToPlayer()
    {
        if (Player && ItemCount[ResultType] > 0)
        {
            if (Player.ReceiveItem(fullProductId))
            {
                ItemCount[ResultType]--;
                RefreshProductItemCount();
            }
        }
    }

    public void StartInteract(PlayerController player)
    {
        Player = player;
        GiveToPlayerSchedulerTask = GiveToPlayerScheduler.ScheduleRepeating(0.1f, GiveToPlayer);
    }

    public void StopInteract()
    {
        Player = null;
        GiveToPlayerSchedulerTask.Cancel();
        GiveToPlayerSchedulerTask = null;
    }

    public void ReceiveRawItem()
    {
        ItemCount[EItemType.Raw]++;
        RefreshRawItemCount();
    }

    private void RefreshRawItemCount()
    {
        RawAmountText.UpdateAmount(ItemCount[EItemType.Raw]);
    }

    private void RefreshProductItemCount()
    {
        ProductAmountText.UpdateAmount(ItemCount[ResultType]);
    }
}