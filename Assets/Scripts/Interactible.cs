using UnityEngine;

public class Interactible : MonoBehaviour, IInteractible
{

    public bool Interact()
    {
        Debug.Log("Yoyoyo");
        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
