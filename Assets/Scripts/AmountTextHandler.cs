using TMPro;
using UnityEngine;

public class AmountTextHandler : MonoBehaviour
{
    [SerializeField] private GameObject ParentGameObject;
    [SerializeField] private bool ShouldHideWhenEmpty = true;
    private TextMeshPro Text;


    private void Awake()
    {
        Text = GetComponent<TextMeshPro>();
    }

    public void UpdateAmount(int amount)
    {
        Text.text = amount.ToString();
        if (ShouldHideWhenEmpty)
        {
            if (ParentGameObject.activeSelf == false && amount > 0)
            {
                ParentGameObject.SetActive(true);
            }
            else if (ParentGameObject.activeSelf == true && amount <= 0)
            {
                ParentGameObject.SetActive(true);
            }
        }
    }
}
