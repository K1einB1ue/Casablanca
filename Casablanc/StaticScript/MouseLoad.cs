using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLoad : SingletonMono<MouseLoad>
{
    private void Start() {
        MouseTracker.SetCam(Camera.main);
    }
    private void Update() {
        MouseTracker.GetUpdate();
    }
}
public static class MouseTracker
{
    public static Vector2 MousePos;
    public static Camera Camera;

    public static void GetUpdate() {
        MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    public static void SetCam(Camera camera) {
        Camera = camera;
    }
}