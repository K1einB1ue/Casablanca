using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XNode;


public abstract class TriggerNodeStatic : NodeBase, ITrigger {
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
}
public interface ITrigger {
    void TriggerBind(Func<bool> func);
}
