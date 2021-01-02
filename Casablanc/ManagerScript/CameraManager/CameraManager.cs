using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMono<CameraManager>
{
    private static GameObject CamGroup;
    private static bool Zoom = false;

    private void Awake() {
        if (CamGroup == null) {
            CamGroup = GameObject.Instantiate(StaticPath.CamGroup);
        }
    }
    private void Update() {
        CamUpdate();
    }
    public static void CamBind(Transform transform) {
        if (CamGroup.transform.Find("CamNear").TryGetComponent<Cinemachine.CinemachineVirtualCamera>(out Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera1)) {
            cinemachineVirtualCamera1.Follow = transform;
            cinemachineVirtualCamera1.LookAt = transform;
        }
        if (CamGroup.transform.Find("CamFar").TryGetComponent<Cinemachine.CinemachineVirtualCamera>(out Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera2)) {
            cinemachineVirtualCamera2.Follow = transform;
            cinemachineVirtualCamera2.LookAt = transform;
        }
    }

    private void CamUpdate() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (!Zoom) Zoom = true;
            else Zoom = false;
        }
        if (Zoom) {
            CamGroup.transform.Find("CamNear").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;
            CamGroup.transform.Find("CamFar").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 10;
        }
        else {
            CamGroup.transform.Find("CamNear").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 10;
            CamGroup.transform.Find("CamFar").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 9;
        }
    }
}
