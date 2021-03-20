using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "主机输入接口", menuName = "固化接口/主机输入接口")]
public class PlayerInput : InputBase
{
    public InputSettings InputSettings = new InputSettings();
    ActionTimer WheelUpTriggerTimer = new ActionTimer();
    ActionTimer WheelDownTriggerTimer = new ActionTimer();


    protected InputTriggerEvent OEvent = new InputTriggerEvent();
    protected InputTriggerEvent PEvent = new InputTriggerEvent();
    
    private InputRuntimeProperties.RuntimeValues.RuntimeValues_State State => this.input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_State;

    public override void onEnable() {
        WheelUpTriggerTimer.Register(this.WheelUpEvent.Invoke);
        WheelDownTriggerTimer.Register(this.WheelDownEvent.Invoke);
    }
    public override void FixedUpdate() {
        if (Input.GetKey(KeyCode.W))                    { this.MoveUpEvent                          ?.Invoke();
        State.W = true;}else { State.W = false;
        }
        if (Input.GetKey(KeyCode.A))                    { this.MoveLeftEvent                        ?.Invoke(); 
        State.A = true;}else { State.A = false;
        }
        if (Input.GetKey(KeyCode.S))                    { this.MoveDownEvent                        ?.Invoke(); 
        State.S = true;}else { State.S = false;
        }
        if (Input.GetKey(KeyCode.D))                    { this.MoveRightEvent                       ?.Invoke(); 
        State.D = true;}else { State.D = false;
        }
        if (Input.GetKey(KeyCode.F))                    { this.GetUpThingsInUpdateByRayEvent        ?.Invoke(); }
        if (Input.GetKey(KeyCode.G))                    { this.DropItemEvent                        ?.Invoke(); }
        if (Input.GetKey(KeyCode.T))                    { this.UseUpThingsInUpdateByRayEvent        ?.Invoke(); }
        if (Input.GetKey(KeyCode.R))                    { this.Use3Event                            ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha1))               { this.K1Event                              ?.Invoke(); this.K_Event.Invoke(0); }
        if (Input.GetKey(KeyCode.Alpha2))               { this.K2Event                              ?.Invoke(); this.K_Event.Invoke(1); }
        if (Input.GetKey(KeyCode.Alpha3))               { this.K3Event                              ?.Invoke(); this.K_Event.Invoke(2); }
        if (Input.GetKey(KeyCode.Alpha4))               { this.K4Event                              ?.Invoke(); this.K_Event.Invoke(3); }
        if (Input.GetKey(KeyCode.Alpha5))               { this.K5Event                              ?.Invoke(); this.K_Event.Invoke(4); }
        if (Input.GetKey(KeyCode.Alpha6))               { this.K6Event                              ?.Invoke(); this.K_Event.Invoke(5); }
        if (Input.GetKey(KeyCode.Alpha7))               { this.K7Event                              ?.Invoke(); this.K_Event.Invoke(6); }
        if (Input.GetMouseButton(0))                    { this.Use1Event                            ?.Invoke(); this.Use_Event.Invoke(1); }
        if (Input.GetMouseButton(1))                    { this.Use2Event                            ?.Invoke(); this.Use_Event.Invoke(2); }
        if (Input.GetKey(KeyCode.R))                    { this.Use3Event                            ?.Invoke(); this.Use_Event.Invoke(3); }
        if (Input.GetKey(KeyCode.LeftShift))            { this.RunEvent                             ?.Invoke(); }
        if (Input.GetKey(KeyCode.O))                    { this.OEvent                               ?.Invoke(); }
        if (Input.GetKey(KeyCode.P))                    { this.PEvent                               ?.Invoke(); }

        WheelUpTriggerTimer.SetTimer(InputSettings.WheelIntervalTime);
        WheelDownTriggerTimer.SetTimer(InputSettings.WheelIntervalTime);
        float dv = Input.GetAxis("Mouse ScrollWheel");
        if (dv > this.InputSettings.WheelSensor) {
            WheelUpTriggerTimer.Invoke();
        }
        else if (dv < -this.InputSettings.WheelSensor) {
            WheelDownTriggerTimer.Invoke();
        }



    }

    public override void RegisteInput(UnityAction unityAction, InputType inputType, bool Add = true) {
        base.RegisteInput(unityAction, inputType, Add);
        if (Add) {
            switch (inputType) {
                case InputType.P:   PEvent.AddListener(unityAction);    break;
                case InputType.O:   OEvent.AddListener(unityAction);    break;
            }
        }else {
            switch (inputType) {
                case InputType.P:   PEvent.RemoveListener(unityAction); break;
                case InputType.O:   OEvent.RemoveListener(unityAction); break;
            }
        }
    }

    public override void Update() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        this.RaycastHit = Physics.RaycastAll(ray, 30, Mask);
        this.hit = RaycastHit.Length > 0;

    }


}

[Serializable]
public class InputSettings
{
    public float WheelSensor = 0.25f;
    public float WheelIntervalTime = 0.1f;
}


public class NetInput : InputBase
{

}
public class NNInput : InputBase
{

}