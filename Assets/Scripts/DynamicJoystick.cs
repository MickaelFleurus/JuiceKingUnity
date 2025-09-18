using UnityEngine;

public class DynamicJoystick : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler PlayerInputHandler;
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    public Canvas canvas;
    private Vector2 joystickStartPosition;


    private void Awake()
    {
        joystickBackground.gameObject.SetActive(false);
        PlayerInputHandler.OnPressStarted += OnPressPerformed;
        PlayerInputHandler.OnPressAndHoldUpdate += OnPressAndHoldUpdate;
        PlayerInputHandler.OnPressEnded += OnPressCanceled;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnPressStarted -= OnPressPerformed;
        PlayerInputHandler.OnPressAndHoldUpdate -= OnPressAndHoldUpdate;
        PlayerInputHandler.OnPressEnded -= OnPressCanceled;
    }

    public void OnPressPerformed(Vector2 touchPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                touchPosition,
                canvas.worldCamera,
                out joystickStartPosition
            );

        joystickBackground.anchoredPosition = joystickStartPosition;
        joystickBackground.gameObject.SetActive(true);
    }

    private void OnPressAndHoldUpdate(Vector2 position, Vector2 direction, float angle)
    {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                position,
                canvas.worldCamera,
                out localPosition
            );
        Vector2 localDirection = localPosition - joystickStartPosition;
        float radius = joystickBackground.sizeDelta.x / 2;
        localDirection = Vector2.ClampMagnitude(localDirection, radius);
        joystickHandle.anchoredPosition = localDirection;
    }

    public void OnPressCanceled()
    {
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBackground.gameObject.SetActive(false);
    }
}
