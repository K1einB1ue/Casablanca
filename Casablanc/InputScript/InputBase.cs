using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


public class InputBase : Channel, Input_Interface
{
    public event UnityAction MoveUp                             = new UnityAction(() => { });
    public event UnityAction MoveLeft                           = new UnityAction(() => { });
    public event UnityAction MoveRight                          = new UnityAction(() => { });
    public event UnityAction MoveDown                           = new UnityAction(() => { });
    public event UnityAction GetUpThingsInUpdateByRay           = new UnityAction(() => { });
    public event UnityAction UseUpThingsInUpdateByRay           = new UnityAction(() => { });
    public event UnityAction DropItem                           = new UnityAction(() => { });
    public event UnityAction Run                                = new UnityAction(() => { });
    public event UnityAction Use1                               = new UnityAction(() => { });
    public event UnityAction Use2                               = new UnityAction(() => { });
    public event UnityAction Use3                               = new UnityAction(() => { });
    public event UnityAction Use4                               = new UnityAction(() => { });
    public event UnityAction Use5                               = new UnityAction(() => { });
    public event UnityAction Use6                               = new UnityAction(() => { });
    public event UnityAction K1                                 = new UnityAction(() => { });
    public event UnityAction K2                                 = new UnityAction(() => { });
    public event UnityAction K3                                 = new UnityAction(() => { });
    public event UnityAction K4                                 = new UnityAction(() => { });
    public event UnityAction K5                                 = new UnityAction(() => { });
    public event UnityAction K6                                 = new UnityAction(() => { });
    public event UnityAction K7                                 = new UnityAction(() => { });

    protected InputTriggerEvent MoveUpEvent                          = new InputTriggerEvent();
    protected InputTriggerEvent MoveLeftEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent MoveRightEvent                       = new InputTriggerEvent();
    protected InputTriggerEvent MoveDownEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent GetUpThingsInUpdateByRayEvent        = new InputTriggerEvent();
    protected InputTriggerEvent UseUpThingsInUpdateByRayEvent        = new InputTriggerEvent();
    protected InputTriggerEvent DropItemEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent RunEvent                             = new InputTriggerEvent();
    protected InputTriggerEvent Use1Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use2Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use3Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use4Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use5Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use6Event                            = new InputTriggerEvent();
    protected InputTriggerEvent K1Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K2Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K3Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K4Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K5Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K6Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K7Event                              = new InputTriggerEvent();

    public override void onEnable() {
        this.Init();
    }
    public virtual void Init() {
        MoveUpEvent.AddListener(MoveUp);
        MoveLeftEvent.AddListener(MoveLeft);
        MoveRightEvent.AddListener(MoveRight);
        MoveDownEvent.AddListener(MoveDown);
        GetUpThingsInUpdateByRayEvent.AddListener(GetUpThingsInUpdateByRay);
        UseUpThingsInUpdateByRayEvent.AddListener(UseUpThingsInUpdateByRay);
        DropItemEvent.AddListener(DropItem);
        RunEvent.AddListener(Run);
        Use1Event.AddListener(Use1);
        Use2Event.AddListener(Use2);
        Use3Event.AddListener(Use3);
        Use4Event.AddListener(Use4);
        Use5Event.AddListener(Use5);
        Use6Event.AddListener(Use6);
        K1Event.AddListener(K1);
        K2Event.AddListener(K2);
        K3Event.AddListener(K3);
        K4Event.AddListener(K4);
        K5Event.AddListener(K5);
        K6Event.AddListener(K6);
        K7Event.AddListener(K7);
    }


    public void RegisteInput(UnityAction unityAction, InputType inputType,bool Add=true) {
        if (Add) {
            switch (inputType) {
                case InputType.MoveUp: this.MoveUp += unityAction; break;
                case InputType.MoveLeft: this.MoveLeft += unityAction; break;
                case InputType.MoveRight: this.MoveRight += unityAction; break;
                case InputType.MoveDown: this.MoveDown += unityAction; break;
                case InputType.GetUpThingsInUpdateByRay: this.GetUpThingsInUpdateByRay += unityAction; break;
                case InputType.UseUpThingsInUpdateByRay: this.UseUpThingsInUpdateByRay += unityAction; break;
                case InputType.DropItem: this.DropItem += unityAction; break;
                case InputType.Run: this.Run += unityAction; break;
                case InputType.Use1: this.Use1 += unityAction; break;
                case InputType.Use2: this.Use2 += unityAction; break;
                case InputType.Use3: this.Use3 += unityAction; break;
                case InputType.Use4: this.Use4 += unityAction; break;
                case InputType.Use5: this.Use5 += unityAction; break;
                case InputType.Use6: this.Use6 += unityAction; break;
                case InputType.K1: this.K1 += unityAction; break;
                case InputType.K2: this.K2 += unityAction; break;
                case InputType.K3: this.K3 += unityAction; break;
                case InputType.K4: this.K4 += unityAction; break;
                case InputType.K5: this.K5 += unityAction; break;
                case InputType.K6: this.K6 += unityAction; break;
                case InputType.K7: this.K7 += unityAction; break;
            }
        }
        else { 
            switch (inputType) {
                case InputType.MoveUp: this.MoveUp -= unityAction; break;
                case InputType.MoveLeft: this.MoveLeft -= unityAction; break;
                case InputType.MoveRight: this.MoveRight -= unityAction; break;
                case InputType.MoveDown: this.MoveDown -= unityAction; break;
                case InputType.GetUpThingsInUpdateByRay: this.GetUpThingsInUpdateByRay -= unityAction; break;
                case InputType.UseUpThingsInUpdateByRay: this.UseUpThingsInUpdateByRay -= unityAction; break;
                case InputType.DropItem: this.DropItem -= unityAction; break;
                case InputType.Run: this.Run -= unityAction; break;
                case InputType.Use1: this.Use1 -= unityAction; break;
                case InputType.Use2: this.Use2 -= unityAction; break;
                case InputType.Use3: this.Use3 -= unityAction; break;
                case InputType.Use4: this.Use4 -= unityAction; break;
                case InputType.Use5: this.Use5 -= unityAction; break;
                case InputType.Use6: this.Use6 -= unityAction; break;
                case InputType.K1: this.K1 -= unityAction; break;
                case InputType.K2: this.K2 -= unityAction; break;
                case InputType.K3: this.K3 -= unityAction; break;
                case InputType.K4: this.K4 -= unityAction; break;
                case InputType.K5: this.K5 -= unityAction; break;
                case InputType.K6: this.K6 -= unityAction; break;
                case InputType.K7: this.K7 -= unityAction; break;
            }
        }
        
    }

}
public abstract class Channel : MonoScriptableObject { }
public abstract class MonoScriptableObject : ScriptableObject, IScriptable_Mono
{

    public virtual void Update() { }
    private void OnEnable() {
        ScriptobjectManager.scriptable_Monos.Add(this);
        this.onEnable();
    }
    private void OnDisable() { }
    public virtual void onEnable() { }

    public virtual void onDisable() { }

    public virtual void FixedUpdate() { }

    public virtual void Start() { }
}
public interface Output_Interface {
    void Init();
}
public interface Input_Interface {
    void Init();
    void Update();
}
public interface IScriptable_Mono
{
    void onEnable();
    void onDisable();
    void FixedUpdate();
    void Start();
    void Update();
}

public class InputTriggerEvent : UnityEvent { }
