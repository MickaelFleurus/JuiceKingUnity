using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler PlayerInputHandler;
    [SerializeField] private GameStatsManager GameStatsManager;
    // Chainsaw
    [SerializeField] private GameObject mChainsaw;

    // Movement
    [SerializeField] private float mMoveSpeed = 5f;

    // Interaction
    private float mInteractionDuration = 0f;
    private float mRequiredInteractionTime = 2f;
    private GameObject mCurrentInteractionObject;

    // Player inventory
    private PlayerInventory mInventory;

    private void Awake()
    {
        mInventory = new PlayerInventory(GameStatsManager.Data.playerInventory);
        mChainsaw.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnPressAndHoldUpdate += OnPressAndHoldUpdate;
    }


    private void OnDisable()
    {
        PlayerInputHandler.OnPressAndHoldUpdate -= OnPressAndHoldUpdate;
    }

    private void OnPressAndHoldUpdate(Vector2 position, Vector2 direction, float angle)
    {
        transform.Translate(direction * mMoveSpeed * Time.deltaTime);
        transform.localScale = new Vector3(Mathf.Sign(angle), 1f, 1f);
        if (mChainsaw.activeSelf)
        {
            mChainsaw.transform.rotation = Quaternion.Euler(0, 0, angle);
        }


        if (mCurrentInteractionObject != null)
        {
            if (mInteractionDuration >= mRequiredInteractionTime)
            {
                if ((bool)(mCurrentInteractionObject.GetComponent<Interactible>()?.Interact()))
                {
                    mCurrentInteractionObject = null; // Clear current interaction object after successful interaction
                }
            }
            else
            {
                mInteractionDuration += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StandInteraction"))
        {
            Debug.Log("Entered interaction zone of " + other.name);
            if (mCurrentInteractionObject == null)
            {
                mCurrentInteractionObject = other.gameObject;
                mInteractionDuration = 0f; // Reset interaction duration for new object
            }
        }
        else if (other.CompareTag("Shelf"))
        {
            Debug.Log("Entered interaction zone of " + other.name);
            Shelf shelf = other.gameObject.GetComponent<Shelf>();
            if (shelf != null)
            {
                shelf.StartInteract();
            }
        }
        else if (other.CompareTag("FruitArea"))
        {
            mChainsaw.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exited interaction zone of " + other.name);
        if (other.CompareTag("StandInteraction") && mCurrentInteractionObject == other.gameObject)
        {
            mCurrentInteractionObject = null;
        }
        else if (other.CompareTag("Shelf"))
        {
            Debug.Log("Entered interaction zone of " + other.name);
            Shelf shelf = other.gameObject.GetComponent<Shelf>();
            if (shelf != null)
            {
                shelf.StopInteract();
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
        }
        else
        {
            Debug.Log("Failed to add item to inventory. Inventory might be full.");
        }
        return added;
    }
}