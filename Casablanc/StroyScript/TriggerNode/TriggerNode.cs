using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XNode;


public abstract class TriggerNodeStatic : NodeStatic, ITrigger {
    private bool NeedTrigger = false;
    private Func<bool> Trigger;
    void ITrigger.TriggerBind(Func<bool> func) {
        if (this.NeedTrigger) {
            this.Trigger = func;
        }
        else {
            return;
        }
    }
    protected bool GetTrigger() {
        return this.Trigger.Invoke();
    }
    public override void NodeInit() {
        if (Trigger == null) {
            this.NeedTrigger = true;
        }
    }
}
public interface ITrigger {
    void TriggerBind(Func<bool> func);
}
