using System;
using System.Collections.Generic;
using UnityEngine;

public class SchedulerManager : MonoBehaviour
{
    private class SchedulerEntry
    {
        public WeakReference Owner;
        public Scheduler Scheduler;
    }

    private readonly List<SchedulerEntry> schedulers = new();

    public Scheduler CreateScheduler(object owner)
    {
        var scheduler = new Scheduler();
        schedulers.Add(new SchedulerEntry { Owner = new WeakReference(owner), Scheduler = scheduler });
        return scheduler;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        for (int i = schedulers.Count - 1; i >= 0; i--)
        {
            if (!schedulers[i].Owner.IsAlive)
            {
                schedulers.RemoveAt(i);
                continue;
            }
            schedulers[i].Scheduler.Update(deltaTime);
        }
    }
}