using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Dictionary<StandId, GameObject> InGameUIGO;


    private void Awake()
    {
        InGameUIGO = new Dictionary<StandId, GameObject>();
    }


    public void RegisterInGameUI(StandId standId, GameObject uiElement)
    {
        if (!InGameUIGO.ContainsKey(standId))
        {
            InGameUIGO[standId] = uiElement;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
