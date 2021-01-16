using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "主机输入接口", menuName = "固化接口/主机输入接口")]
public class PlayerInput : InputBase
{
    private InputRuntimeProperties.RuntimeValues.RuntimeValues_State State => this.input_Property.InputRuntimeProperties.InputRuntimeValues.RuntimeValues_State;
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
        if (Input.GetKeyDown(KeyCode.T))                { this.UseUpThingsInUpdateByRayEvent        ?.Invoke(); }
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
        if (Input.GetKeyDown(KeyCode.R))                { this.Use3Event                            ?.Invoke(); this.Use_Event.Invoke(3); }
        if (Input.GetKey(KeyCode.LeftShift))            { this.RunEvent                             ?.Invoke(); }

        
    }
    public override void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        this.hit = Physics.Raycast(ray, out this.RaycastHit, 100, Mask);
    }
}





public class NetInput : InputBase
{

}
public class NNInput : InputBase
{

}