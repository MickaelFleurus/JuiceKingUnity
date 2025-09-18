using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovements mPlayerMovement;
    public bool IsPressing { get; private set; }
    public Vector2 InitialPosition { get; private set; }
    private Vector2 LastPosition;

    public event System.Action<Vector2> OnPressStarted;
    public event System.Action OnPressEnded;
    public event System.Action<Vector2, Vector2, float> OnPressAndHoldUpdate;

    private void Awake()
    {
        mPlayerMovement = new PlayerMovements();
        mPlayerMovement.InGame.PressAndHold.performed += OnPressPerformed;
        mPlayerMovement.InGame.PressAndHold.canceled += OnPressCanceled;
        mPlayerMovement.InGame.PressAndHold.Enable();
    }

    private void OnPressPerformed(InputAction.CallbackContext context)
    {
        IsPressing = true;
        InitialPosition = Pointer.current.position.ReadValue();
        LastPosition = InitialPosition;
        OnPressStarted?.Invoke(InitialPosition);
    }

    private void Update()
    {
        if (IsPressing)
        {
            Vector2 touchPosition = Pointer.current.position.ReadValue();
            if (touchPosition != LastPosition)
            {
                Vector2 moveDirectionNorm = touchPosition - InitialPosition;
                moveDirectionNorm.Normalize();
                float angle = Vector2.SignedAngle(Vector2.up, moveDirectionNorm);
                OnPressAndHoldUpdate?.Invoke(touchPosition, moveDirectionNorm, angle);
                LastPosition = touchPosition;
            }
        }
    }

    private void OnPressCanceled(InputAction.CallbackContext context)
    {
        IsPressing = false;
        OnPressEnded?.Invoke();
    }

    public void Dispose()
    {
        mPlayerMovement.InGame.PressAndHold.performed -= OnPressPerformed;
        mPlayerMovement.InGame.PressAndHold.canceled -= OnPressCanceled;
        mPlayerMovement.InGame.PressAndHold.Disable();
    }
}
