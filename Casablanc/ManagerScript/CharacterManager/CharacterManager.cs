using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonMono<CharacterManager>
{
    public static Character Main { get => main; set { MainChange = true; main = value; } }
    private static Character main;
    private static bool MainChange = false;

    private void Update() {
        if (MainChange) {
            CameraManager.CamBind(Main.Instance.transform);
            MainChange = false;
        }
    }

}
