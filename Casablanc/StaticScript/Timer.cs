using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    
    public float IntervalTime;
    private float TimeBef = 0;

    public bool TimerOn { 
        get {
            try {
                return TimerFo;
            }
            finally {
                if (TimerFo) {
                    TimerFo = false;
                }
            }
        }    
        set {
            TimerFo = value;
        }
    }
    private bool TimerFo = false;
    public Timer(float IntervalTime) {
        this.IntervalTime = IntervalTime;
    }
    public Timer() {
        this.IntervalTime = 0.5f;
    }

    public void SetTimer(float IntervalTime) {
        this.IntervalTime = IntervalTime;
    }

    public IEnumerator TimingOnce(Action action) {
        yield return new WaitForSeconds(this.IntervalTime);
        action();
    }
    public IEnumerator TimingOnce(Action<bool> action,bool flag) {
        yield return new WaitForSeconds(this.IntervalTime);
        action(flag);
    }
    public void TimeingLoop(Action action) {
        if(this.IntervalTime + this.TimeBef <= Time.fixedTime) {
            this.TimeBef = Time.fixedTime;
            action();
        }
    }
    public void TimeingLoop(Action action, ref bool trigger) {
        if ((this.IntervalTime + this.TimeBef <= Time.fixedTime)&&trigger) {
            this.TimeBef = Time.fixedTime;
            action();
            trigger = false;
        }
    }
    public static IEnumerator TimingOnce(float time,Action action) {
        yield return new WaitForSeconds(time);
        action();
    }

   
}

public class ActionTimer
{
    private float IntervalTime;
    private float TimeBef = 0;
    private bool BindRef = false;
    private Func<float> intervalTime;
    private UseEvent UseEvent = new UseEvent();
    public ActionTimer() { }
    public void SetTimer(float IntervalTime) {
        this.IntervalTime = IntervalTime;
        this.BindRef = false;
    }
    public void SetTimer(Func<float> IntervalTime) {
        this.intervalTime = IntervalTime;
        this.BindRef = true;
    }
    public void Register(UnityAction unityAction) {
        this.UseEvent.AddListener(unityAction);
    }
    public void Invoke() {
        if (!BindRef) {
            if ((this.IntervalTime + this.TimeBef <= Time.fixedTime)) {
                this.TimeBef = Time.fixedTime;
                UseEvent?.Invoke();
            }
        }
        else {
            if ((this.intervalTime.Invoke() + this.TimeBef <= Time.fixedTime)) {
                this.TimeBef = Time.fixedTime;
                UseEvent?.Invoke();
            }
        }
    }
    public void ForceInvoke() {
        UseEvent?.Invoke();
    }
}


public class UseEvent : UnityEvent { }
public class UseEvent<T, B> : UnityEvent<T, B> { }