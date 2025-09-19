using UnityEngine;

public class PlayerController : MonoBehaviour, ITriggeredParent
{
    [SerializeField] private PlayerInputHandler PlayerInputHandler;
    [SerializeField] private GameStatsManager GameStatsManager;
    // Chainsaw
    [SerializeField] private GameObject mChainsaw;

    // Movement
    [SerializeField] private float mMoveSpeed = 5f;
    private Vector2 mDirection;

    // Interaction
    private float mRequiredInteractionTime = 2f;
    private GameObject mCurrentInteractionObject;
    private FruitMixer mCurrentFruitMixer = null;
    private Scheduler.ScheduledTask mStandInteractionScheduled = null;
    private Scheduler.ScheduledTask mFruitMixerInteractionScheduled = null;

    // Scheduler
    [SerializeField] private SchedulerManager SchedulerManager;
    private Scheduler Scheduler;

    // Player inventory
    private PlayerInventory mInventory;
    [SerializeField] private AmountTextHandler InventoryAmount;

    [SerializeField] private GameObject PlayerLookGO;

    private void Awake()
    {
        Scheduler = SchedulerManager.CreateScheduler(this);
        mInventory = new PlayerInventory(GameStatsManager.Data.playerInventory);
        mChainsaw.SetActive(false);
        RefreshInventoryCount();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnPressAndHoldUpdate += OnPressAndHoldUpdate;
        PlayerInputHandler.OnPressEnded += OnPressEnded;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnPressAndHoldUpdate -= OnPressAndHoldUpdate;
        PlayerInputHandler.OnPressEnded -= OnPressEnded;
    }
    private void Update()
    {
        transform.Translate(mDirection * mMoveSpeed * Time.deltaTime);
    }

    private void OnPressEnded()
    {
        mDirection = Vector2.zero;
    }

    private void OnPressAndHoldUpdate(Vector2 position, Vector2 direction, float angle)
    {
        mDirection = direction;
        float signAngle = Mathf.Sign(angle);
        if (signAngle != PlayerLookGO.transform.localScale.x)
        {
            PlayerLookGO.transform.localScale = new Vector3(signAngle, 1f, 1f);
        }
        mChainsaw.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void InteractWithMixer()
    {
        if (mCurrentFruitMixer != null)
        {
            if (mInventory.HasItem(mCurrentFruitMixer.rawProductId))
            {
                mCurrentFruitMixer.ReceiveRawItem();
                mInventory.DecrementItem(mCurrentFruitMixer.rawProductId);
                RefreshInventoryCount();
            }
        }
    }

    public void OnChildTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("StandInteraction"))
        {
            Debug.Log("Entered interaction zone of " + other.name);
            if (mCurrentInteractionObject == null)
            {
                mCurrentInteractionObject = other.gameObject;
                mStandInteractionScheduled = Scheduler.Schedule(mRequiredInteractionTime, () => { Debug.Log("Interact with the object pls"); mStandInteractionScheduled = null; });
            }
        }
        else if (other.CompareTag("Shelf"))
        {
            Debug.Log("Entered interaction zone of " + other.name);
            mCurrentFruitMixer = other.gameObject.GetComponentInParent<FruitMixer>();
            if (mCurrentFruitMixer != null)
            {
                mCurrentFruitMixer.StartInteract(this);
                mFruitMixerInteractionScheduled = Scheduler.ScheduleRepeating(0.2f, InteractWithMixer);
            }
        }
        else if (other.CompareTag("FruitArea"))
        {
            mChainsaw.SetActive(true);
        }
    }

    public void OnChildTriggerExit(Collider2D other)
    {
        Debug.Log("Exited interaction zone of " + other.name);
        if (other.CompareTag("StandInteraction") && mCurrentInteractionObject == other.gameObject)
        {
            mCurrentInteractionObject = null;
        }
        else if (other.CompareTag("Shelf"))
        {
            if (mCurrentFruitMixer != null)
            {
                mCurrentFruitMixer.StopInteract();
                mFruitMixerInteractionScheduled.Cancel();
                mFruitMixerInteractionScheduled = null;
            }
        }
        else if (other.CompareTag("FruitArea"))
        {
            mChainsaw.SetActive(false);
        }
    }

    public bool ReceiveItem(FullProductId item)
    {
        bool added = mInventory.AddItem(item, 1);
        if (added)
        {
            Debug.Log($"Added {item.fruit} {item.item} to inventory.");
            RefreshInventoryCount();
        }

        return added;
    }

    private void RefreshInventoryCount()
    {
        InventoryAmount.UpdateAmount(mInventory.GetTotalItemCount());
    }
}