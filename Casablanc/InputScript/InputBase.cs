using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


public class InputBase : Channel, Input_Interface
{
    Input_Property Input_Interface.Input_Property => this.input_Property;
    public Input_Property input_Property = new Input_Property();
    RaycastHit Input_Interface.RaycastHit => this.RaycastHit;
    protected RaycastHit RaycastHit;   
    int Input_Interface.RaycastMask { get => Mask; set => Mask = value; }
    protected int Mask = ~0;
    bool Input_Interface.Hit => hit;
    protected bool hit = false;


    protected InputTriggerEvent MoveUpEvent                          = new InputTriggerEvent();
    protected InputTriggerEvent MoveLeftEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent MoveRightEvent                       = new InputTriggerEvent();
    protected InputTriggerEvent MoveDownEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent GetUpThingsInUpdateByRayEvent        = new InputTriggerEvent();
    protected InputTriggerEvent UseUpThingsInUpdateByRayEvent        = new InputTriggerEvent();
    protected InputTriggerEvent DropItemEvent                        = new InputTriggerEvent();
    protected InputTriggerEvent RunEvent                             = new InputTriggerEvent();
    protected InputTriggerIntEvent Use_Event                         = new InputTriggerIntEvent();
    protected InputTriggerEvent Use1Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use2Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use3Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use4Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use5Event                            = new InputTriggerEvent();
    protected InputTriggerEvent Use6Event                            = new InputTriggerEvent();
    protected InputTriggerIntEvent K_Event                           = new InputTriggerIntEvent();
    protected InputTriggerEvent K1Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K2Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K3Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K4Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K5Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K6Event                              = new InputTriggerEvent();
    protected InputTriggerEvent K7Event                              = new InputTriggerEvent();
    protected InputTriggerEvent WheelUpEvent                         = new InputTriggerEvent();
    protected InputTriggerEvent WheelDownEvent                       = new InputTriggerEvent();

    public override void onEnable() {
    }



    public void RegisteInput(UnityAction unityAction, InputType inputType,bool Add=true) {
        if (Add) {
            switch (inputType) 
            { 
                case InputType.MoveUp:                                                  MoveUpEvent.AddListener(unityAction); break;                 
                case InputType.MoveLeft:                                                MoveLeftEvent.AddListener(unityAction);break;
                case InputType.MoveRight:                                               MoveRightEvent.AddListener(unityAction);break;
                case InputType.MoveDown:                                                MoveDownEvent.AddListener(unityAction);break;
                case InputType.GetUpThingsByRay:                                GetUpThingsInUpdateByRayEvent.AddListener(unityAction);break;
                case InputType.UseUpThingsByRay:                                UseUpThingsInUpdateByRayEvent.AddListener(unityAction);break;
                case InputType.DropItem:                                                DropItemEvent.AddListener(unityAction);break;                
                case InputType.Run:                                                     RunEvent.AddListener(unityAction); break;
                case InputType.Use1:                                                    Use1Event.AddListener(unityAction); break;
                case InputType.Use2:                                                    Use2Event.AddListener(unityAction); break;
                case InputType.Use3:                                                    Use3Event.AddListener(unityAction); break;
                case InputType.Use4:                                                    Use4Event.AddListener(unityAction); break;
                case InputType.Use5:                                                    Use5Event.AddListener(unityAction); break;
                case InputType.Use6:                                                    Use6Event.AddListener(unityAction); break;
                case InputType.K1:                                                      K1Event.AddListener(unityAction); break;
                case InputType.K2:                                                      K2Event.AddListener(unityAction); break;
                case InputType.K3:                                                      K3Event.AddListener(unityAction); break;
                case InputType.K4:                                                      K4Event.AddListener(unityAction); break;
                case InputType.K5:                                                      K5Event.AddListener(unityAction); break;
                case InputType.K6:                                                      K6Event.AddListener(unityAction); break;
                case InputType.K7:                                                      K7Event.AddListener(unityAction); break;
                case InputType.WheelUp:                                                 WheelUpEvent.AddListener(unityAction);break;
                case InputType.WheelDown:                                               WheelDownEvent.AddListener(unityAction); break;
            }
        }
        else {
            switch (inputType) {
                case InputType.MoveUp:                                                  MoveUpEvent.RemoveListener(unityAction); break;
                case InputType.MoveLeft:                                                MoveLeftEvent.RemoveListener(unityAction); break;
                case InputType.MoveRight:                                               MoveRightEvent.RemoveListener(unityAction); break;
                case InputType.MoveDown:                                                MoveDownEvent.RemoveListener(unityAction); break;
                case InputType.GetUpThingsByRay:                                GetUpThingsInUpdateByRayEvent.RemoveListener(unityAction); break;
                case InputType.UseUpThingsByRay:                                UseUpThingsInUpdateByRayEvent.RemoveListener(unityAction); break;
                case InputType.DropItem:                                                DropItemEvent.RemoveListener(unityAction); break;
                case InputType.Run:                                                     RunEvent.RemoveListener(unityAction); break;
                case InputType.Use1:                                                    Use1Event.RemoveListener(unityAction); break;
                case InputType.Use2:                                                    Use2Event.RemoveListener(unityAction); break;
                case InputType.Use3:                                                    Use3Event.RemoveListener(unityAction); break;
                case InputType.Use4:                                                    Use4Event.RemoveListener(unityAction); break;
                case InputType.Use5:                                                    Use5Event.RemoveListener(unityAction); break;
                case InputType.Use6:                                                    Use6Event.RemoveListener(unityAction); break;
                case InputType.K1:                                                      K1Event.RemoveListener(unityAction); break;
                case InputType.K2:                                                      K2Event.RemoveListener(unityAction); break;
                case InputType.K3:                                                      K3Event.RemoveListener(unityAction); break;
                case InputType.K4:                                                      K4Event.RemoveListener(unityAction); break;
                case InputType.K5:                                                      K5Event.RemoveListener(unityAction); break;
                case InputType.K6:                                                      K6Event.RemoveListener(unityAction); break;
                case InputType.K7:                                                      K7Event.RemoveListener(unityAction); break;
                case InputType.WheelUp:                                                 WheelUpEvent.RemoveListener(unityAction); break;
                case InputType.WheelDown:                                               WheelDownEvent.RemoveListener(unityAction); break;
            }
        }
        
    }
    public void RegisteInput(UnityAction<int> unityAction,InputType inputType,bool Add = true) {
        if (Add) {
            switch (inputType) {
                case InputType.K:                                                       K_Event.AddListener(unityAction);break;
                case InputType.Use:                                                     Use_Event.AddListener(unityAction);break;
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

public interface Input_Interface {
    int RaycastMask { get; set; }
    Input_Property Input_Property { get; }
    RaycastHit RaycastHit { get; }
    bool Hit { get; }
    void RegisteInput(UnityAction unityAction, InputType inputType, bool Add = true);
    void RegisteInput(UnityAction<int> unityAction, InputType inputType, bool Add = true);
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
public class InputTriggerIntEvent : UnityEvent<int> { }


[Serializable]
public class Input_Property
{
    public InputRuntimeProperties.InputRuntimeProperties InputRuntimeProperties;

    public Input_Property() {
        this.InputRuntimeProperties = new InputRuntimeProperties.InputRuntimeProperties();
    }
    public Input_Property(InputRuntimeProperties.InputRuntimeProperties inputRuntimeProperties) {
        this.InputRuntimeProperties = inputRuntimeProperties;
    }
}


namespace InputRuntimeProperties
{
    [Serializable]
    public class InputRuntimeProperties
    {
        public InputRuntimeValues InputRuntimeValues = new InputRuntimeValues();
    }
    [Serializable]
    public class InputRuntimeValues
    {
        public RuntimeValues.RuntimeValues_State RuntimeValues_State = new RuntimeValues.RuntimeValues_State();
        public RuntimeValues.RuntimeValues_Vector RuntimeValues_Vector = new RuntimeValues.RuntimeValues_Vector();
    }
    namespace RuntimeValues
    {
        [Serializable]
        public class RuntimeValues_State
        {
            public bool W;
            public bool A;
            public bool S;
            public bool D;
        }
        [Serializable]
        public class RuntimeValues_Vector
        {
            public Vector2 Heading = Vector2.up;
            public Vector2 Handing = Vector2.up;
        }
    }
}



