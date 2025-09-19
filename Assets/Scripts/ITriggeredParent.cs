using UnityEngine;

public interface ITriggeredParent
{
    void OnChildTriggerEnter(Collider2D other);
    void OnChildTriggerExit(Collider2D other);
}