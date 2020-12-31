using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "主机输入接口", menuName = "固化接口/主机输入接口")]
public class PlayerInput : InputBase
{
    public override void Update() {
        if (Input.GetKey(KeyCode.W))                    { this.MoveUpEvent                          ?.Invoke(); }
        if (Input.GetKey(KeyCode.A))                    { this.MoveLeftEvent                        ?.Invoke(); }
        if (Input.GetKey(KeyCode.S))                    { this.MoveDownEvent                        ?.Invoke(); }
        if (Input.GetKey(KeyCode.D))                    { this.MoveRightEvent                       ?.Invoke(); }
        if (Input.GetKey(KeyCode.F))                    { this.GetUpThingsInUpdateByRayEvent        ?.Invoke(); }
        if (Input.GetKey(KeyCode.G))                    { this.DropItemEvent                        ?.Invoke(); }
        if (Input.GetKey(KeyCode.T))                    { this.UseUpThingsInUpdateByRayEvent        ?.Invoke(); }
        if (Input.GetKey(KeyCode.R))                    { this.Use3Event                            ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha1))               { this.K1Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha2))               { this.K2Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha3))               { this.K3Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha4))               { this.K4Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha5))               { this.K5Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha6))               { this.K6Event                              ?.Invoke(); }
        if (Input.GetKey(KeyCode.Alpha7))               { this.K7Event                              ?.Invoke(); }
        if (Input.GetMouseButton(0))                    { this.Use1Event                            ?.Invoke(); }
        if (Input.GetMouseButton(1))                    { this.Use2Event                            ?.Invoke(); }
        if (Input.GetKey(KeyCode.LeftShift))            { this.RunEvent                             ?.Invoke(); }
    }
}