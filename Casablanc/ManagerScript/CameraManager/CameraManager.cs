using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class CameraManager : SingletonMono<CameraManager>
{
    public static GameObject CamGroup;
    private static bool Zoom = false;

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

    public static void Init() {
        CamGroup ??= GameObject.Instantiate(StaticPath.CamGroup);
    }
    public static void CamBindMainCharacter() {
        CamBind(CharacterManager.Main.Instance.transform);
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
