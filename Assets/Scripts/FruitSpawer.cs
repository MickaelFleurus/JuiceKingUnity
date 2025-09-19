using System.Collections.Generic;
using UnityEngine;

public class FruitSpawer : MonoBehaviour
{
    [SerializeField] private EFruitType FruitType;
    [SerializeField] private GameObject FruitPrefab;
    [SerializeField] private SchedulerManager SchedulerManager;
    private Scheduler ShowFruitAgain;
    private Scheduler.ScheduledTask ShowFruitAgainTask;

    private Vector2 Min;
    private Vector2 Max;

    private List<FruitController> spawnedFruits = new List<FruitController>();


    private void Awake()
    {
        ShowFruitAgain = SchedulerManager.CreateScheduler(this);
        // Get the bounding box from Collider2D
        var collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("FruitSpawer requires a Collider2D on the same GameObject.");
            return;
        }

        Bounds bounds = collider.bounds;
        Min = bounds.min;
        Max = bounds.max;

        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(Min.x, Max.x),
                Random.Range(Min.y, Max.y)
            );

            GameObject clone = Instantiate(FruitPrefab, randomPos, Quaternion.identity);
            FruitController fruitController = clone.GetComponent<FruitController>();
            spawnedFruits.Add(fruitController);
        }
        ShowFruitAgainTask = ShowFruitAgain.ScheduleRepeating(3.0f, SpawnFruitIfAnyAvailable);
    }

    private void SpawnFruitIfAnyAvailable()
    {
        FruitController inactiveFruit = spawnedFruits.Find(fruit => !fruit.gameObject.activeSelf);
        if (inactiveFruit != null)
        {
            Vector2 randomPos = new Vector2(
               Random.Range(Min.x, Max.x),
               Random.Range(Min.y, Max.y)
           );
            inactiveFruit.Show(randomPos);
        }
    }
}
