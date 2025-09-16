using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    private PlayerMovements mPlayerMovement;


    // Chainsaw
    [SerializeField]
    private GameObject mChainsaw;

    // Movement
    private bool mIsPressing;
    private Vector2 mInitialPosition;
    public float mMoveSpeed = 5f;

    // Interaction
    private float mInteractionDuration = 0f;
    private float mRequiredInteractionTime = 2f;
    private GameObject mCurrentInteractionObject;

    // Player inventory
    private PlayerInventory mInventory = new PlayerInventory(50);

    private void Awake()
    {
        mPlayerMovement = new PlayerMovements();
        mChainsaw.SetActive(false);
    }

    private void OnEnable()
    {
        mPlayerMovement.InGame.PressAndHold.performed += OnPressPerformed;
        mPlayerMovement.InGame.PressAndHold.canceled += OnPressCanceled;
        mPlayerMovement.InGame.PressAndHold.Enable();


        mPlayerMovement.Debug.ResetPlayerPosition.performed += OnResetPressed;
        mPlayerMovement.Debug.ResetPlayerPosition.Enable();
    }
    private void OnResetPressed(InputAction.CallbackContext context)
    {
        transform.position = Vector3.zero;
    }

    private void OnDisable()
    {
        mPlayerMovement.InGame.PressAndHold.performed -= OnPressPerformed;
        mPlayerMovement.InGame.PressAndHold.canceled -= OnPressCanceled;
        mPlayerMovement.InGame.PressAndHold.Disable();

        mPlayerMovement.Debug.ResetPlayerPosition.performed -= OnResetPressed;
        mPlayerMovement.Debug.ResetPlayerPosition.Disable();
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        mIsPressing = true;
        mInitialPosition = Pointer.current.position.ReadValue();
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        mIsPressing = false;
    }

    private void Update()
    {
        if (mIsPressing) // Only process movement if the player is pressing
        {
            Vector2 currentPosition = Pointer.current.position.ReadValue();
            Vector2 moveDirectionNorm = currentPosition - mInitialPosition;
            moveDirectionNorm.Normalize();
            transform.Translate(moveDirectionNorm * mMoveSpeed * Time.deltaTime);

            if (mChainsaw.activeSelf)
            {
                float angle = Vector2.SignedAngle(Vector2.up, moveDirectionNorm);

                // Set the rotation of the chainsaw around the Z axis
                mChainsaw.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
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

    public bool ReceiveItem(InventoryKey item)
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