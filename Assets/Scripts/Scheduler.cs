using System;
using System.Collections.Generic;

public class Scheduler
{
    public class ScheduledTask
    {
        public float TimeRemaining;
        public Action Callback;
        public bool Repeat;
        public float Interval;
        public bool Cancelled;

        public void Cancel()
        {
            Cancelled = true;
        }
    }

    private readonly List<ScheduledTask> tasks = new();

    public ScheduledTask Schedule(float delay, Action callback)
    {
        var task = new ScheduledTask { TimeRemaining = delay, Callback = callback, Repeat = false, Interval = delay };
        tasks.Add(task);
        return task;
    }

    public ScheduledTask ScheduleRepeating(float interval, Action callback)
    {
        var task = new ScheduledTask { TimeRemaining = interval, Callback = callback, Repeat = true, Interval = interval };
        tasks.Add(task);
        return task;
    }

    public void Update(float deltaTime)
    {
        for (int i = tasks.Count - 1; i >= 0; i--)
        {
            var task = tasks[i];
            if (task.Cancelled)
            {
                tasks.RemoveAt(i);
                continue;
            }

            task.TimeRemaining -= deltaTime;
            if (task.TimeRemaining <= 0f)
            {
                task.Callback?.Invoke();
                if (task.Repeat && !task.Cancelled)
                {
                    task.TimeRemaining = task.Interval;
                }
                else
                {
                    tasks.RemoveAt(i);
                }
            }
        }
    }
}