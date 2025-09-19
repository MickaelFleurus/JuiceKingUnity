using UnityEngine;

public class ParentTriggerer : MonoBehaviour
{
    private ITriggeredParent target;
    private void Awake()
    {
        target = GetComponentInParent<ITriggeredParent>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        target?.OnChildTriggerEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        target?.OnChildTriggerExit(other);
    }
}
