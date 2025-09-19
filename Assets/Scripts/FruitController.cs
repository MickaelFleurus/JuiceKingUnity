using UnityEngine;

public class FruitController : MonoBehaviour, ITriggeredParent
{
    private FullProductId fruitId;
    private void Awake()
    {
        fruitId = new FullProductId(EItemType.Raw, EFruitType.Apple);
    }

    public void OnChildTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null && player.ReceiveItem(fruitId))
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnChildTriggerExit(Collider2D other)
    {

    }

    public void Show(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }

}
