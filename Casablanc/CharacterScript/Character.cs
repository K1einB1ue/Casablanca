using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public abstract class CharacterBase : MonoBehaviour
{   
    public CharacterInfoStore CharacterInfoStore;




    private void OnEnable() {
        this.onEnable();
    }  
    private void OnDisable() {
        this.OnDisable();
    }
    private void Update() {
        this.update();
    }
    public virtual void update() {
    
    }

    public virtual void onDisbale() {
    
    }
    public virtual void onEnable() {
        
    }

}

[AttributeUsage(AttributeTargets.Method,Inherited =true,AllowMultiple =true)]
public class InputRegisterAttribute : Attribute {
    public InputType InputType;
    InputRegisterAttribute(InputType inputType) {
        this.InputType = inputType;
    }
}
public enum InputType {
    MoveUp,
    MoveLeft,
    MoveRight,
    MoveDown,
    GetUpThingsInUpdateByRay,
    UseUpThingsInUpdateByRay,
    DropItem,
    Run,
    Use1,
    Use2,
    Use3,
    Use4,
    Use5,
    Use6,
    K1,
    K2,
    K3,
    K4,
    K5,
    K6,
    K7
}

[CreateAssetMenu(fileName = "ÐÂ´æ´¢", menuName = "½ÇÉ«/¶¯¾²Ì¬»ìºÏ´æ´¢")]
public class CharacterInfoStore {
    public CharacterNodeStatic CharacterNodeStatic;
    public CharacterNodeDynamic characterNodeDynamic;
    public ItemInfoStore ItemInfoStore;
    public InputBase Input;
}


namespace CharacterRuntimeProperties
{
    [Serializable]
    public class CharacterRuntimeProperties
    {

    }
    namespace RuntimeValues
    {


    }

}

namespace CharacterStaticProperties
{

    [Serializable]
    public class CharacterStaticProperties
    {


    }
    namespace StaticValues
    {


    }

}
namespace CharacterPreInstanceProperties
{

    [Serializable]
    public class CharacterPreInstanceProperties
    {


    }
}