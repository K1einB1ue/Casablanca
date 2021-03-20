using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLeader : MonoBehaviour, ObjectOnTheGround
{
    object ObjectOnTheGround.Object { 
        get {
            if (ITryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
                return objectOnTheGround.Object;
            }
            Debug.LogWarning("未捕捉到接口");
            return null;
        }
    }
    Object_Values_Handler ObjectOnTheGround.Object_Values_Handler { 
        get { 
            if(ITryGetComponent<ObjectOnTheGround>(out var objectOnTheGround)) {
                return objectOnTheGround.Object_Values_Handler;
            }
            Debug.LogWarning("未捕捉到接口");
            return null;       
        } 
    }
    public GameObject Target;
    public bool ShaderTrigger = false;
    public bool ITryGetComponent<T>(out T component) {
        if(this.Target.TryGetComponent<T>(out T c)) {
            component = c;
            return true;
        }
        component = default(T);
        return false;
    }
}
