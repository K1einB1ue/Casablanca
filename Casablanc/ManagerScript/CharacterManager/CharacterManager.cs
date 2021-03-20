using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterManager : SingletonMono<CharacterManager>
{
    public static Character Main { get => main; 
        set { 
            if (main != value) { 
                main = value;
                CameraManager.Init();
                EventManager.CharacterManager.OnMainCharacterChange.AddListenerOnce(CameraManager.CamBindMainCharacter);
                EventManager.CharacterManager.OnMainCharacterChange?.Invoke(); 
            } 
            else { 
                Debug.Log("进行了多余赋值"); 
            } 
        } 
    }

    private static Character main;


    






}
public static class CharacterEx
{
    public static void InputInterFace_Swap(this Character character1, Character character2) {

    }
}



