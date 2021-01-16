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

    public static IEnumerator FadeOut(float time,CanvasGroup canvasGroup) {
        bool Lock = false;
        AlphaLoopDEC(ref Lock, canvasGroup);
        yield return new WaitForSeconds(time);
        Lock = true;
        canvasGroup.gameObject.SetActive(false);
        canvasGroup.alpha = 0;
    }
    private static void AlphaLoopDEC(ref bool Lock, CanvasGroup canvasGroup) {
        while (!Lock) {
            canvasGroup.alpha -= canvasGroup.alpha / 2f;
        }
    }
   
}

public class ActionTimer
{
    public float IntervalTime;
    private float TimeBef = 0;
    private UseEvent UseEvent = new UseEvent();
    public ActionTimer() { }
    public void SetTimer(float IntervalTime) {
        this.IntervalTime = IntervalTime;
    }
    public void Registe(UnityAction unityAction) {
        this.UseEvent.AddListener(unityAction);
    }
    public void Invoke() {
        if ((this.IntervalTime + this.TimeBef <= Time.fixedTime)) {
            this.TimeBef = Time.fixedTime;
            UseEvent?.Invoke();
        }
    }
}


public class UseEvent : UnityEvent { }