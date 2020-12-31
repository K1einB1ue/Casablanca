using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemStateManager:SingletonMono<SystemStateManager>
{
    public static SystemState systemState = SystemState.PlayMode;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("按下了");
            if (systemState == SystemState.PlayMode) {
                systemState = SystemState.Debug;
            }
            else if (systemState == SystemState.Debug) {
                systemState = SystemState.ViewMode;
            }
            else if (systemState == SystemState.ViewMode) {
                systemState = SystemState.PlayMode;
            }
        }
    }
}

public enum SystemState
{
    Debug,
    PlayMode,
    ViewMode
}
