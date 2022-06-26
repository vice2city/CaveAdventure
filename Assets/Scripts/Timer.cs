using System.Collections.Generic;
using UnityEngine;

internal class TimerNode
{
    public Timer.TimerHandler callback;
    public float repeatRate; // 定时器触发的时间间隔;
    public float time; // 第一次触发要隔多少时间;
    public int repeat; // 你要触发的次数;
    public float passedTime; // 这个Timer过去的时间;
    public bool isRemoved; // 是否已经删除了
    public int timerId; // 标识这个timer的唯一Id号;
}

public class Timer
{
    public delegate void TimerHandler();
    private Dictionary<int, TimerNode> timers;  //存放Timer对象

    private List<TimerNode> removeTimers;   //新增Timer缓存队列
    private List<TimerNode> newAddTimers;    //删除Timer缓存队列

    private int autoIncId = 1;  //每个Timer的唯一标示

    //初始化Timer管理器
    public void Init()
    {
        timers = new Dictionary<int, TimerNode>();
        autoIncId = 1;
        removeTimers = new List<TimerNode>();
        newAddTimers = new List<TimerNode>();
    }

    /// <summary>
    /// 以秒为单位调用方法methodName，然后在每个repeatRate重复调用。
    /// </summary>
    /// <param name="methodName">回调函数</param>
    /// <param name="time">延迟调用</param>
    /// <param name="repeatRate">时间间隔</param>
    /// <param name="repeat">重复调用的次数 小于等于0表示无限触发</param>
    
    public int Schedule(TimerHandler methodName, float time, float repeatRate, int repeat=0)
    {
        var timer = new TimerNode
        {
            callback = methodName,
            repeat = repeat,
            repeatRate = repeatRate,
            time = time,
            passedTime = repeatRate,
            isRemoved = false,
            timerId = autoIncId
        };

        autoIncId++;
        newAddTimers.Add(timer); // 加到缓存队列里面
        return timer.timerId;
    }

    //移除Timers
    public void Unschedule(int timerId)
    {
        if (!timers.ContainsKey(timerId))
        {
            return;
        }
        var timer = timers[timerId];
        timer.isRemoved = true; // 先标记，不直接删除
    }

    //在Update里面调用
    public void Update()
    {
        var dt = Time.deltaTime;
        // 添加新的Timers
        foreach (var t in newAddTimers)
        {
            timers.Add(t.timerId, t);
        }
        newAddTimers.Clear();
        foreach (var timer in timers.Values)
        {
            if (timer.isRemoved)
            {
                removeTimers.Add(timer);
                continue;
            }
            timer.passedTime += dt;
            if (!(timer.passedTime >= (timer.time + timer.repeatRate))) continue;
            // 做一次触发
            timer.callback();
            timer.repeat--;
            timer.passedTime -= (timer.time + timer.repeatRate);
            timer.time = 0;
            if (timer.repeat != 0) continue;
            // 触发次数结束，将该删除的加入队列
            timer.isRemoved = true;
            removeTimers.Add(timer);
        }
        // 清理掉要删除的Timer;
        foreach (var t in removeTimers)
        {
            timers.Remove(t.timerId);
        }
        removeTimers.Clear();
    }
}
