using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptobjectManager : SingletonMono<ScriptobjectManager>
{
    public static List<IScriptable_Mono> scriptable_Monos=new List<IScriptable_Mono>();
    private void Awake() {
        
    }
    private void Update() {
        foreach (var obj in scriptable_Monos) {
            obj.Update();
        }
    }
    private void FixedUpdate() {
        foreach(var obj in scriptable_Monos) {
            obj.FixedUpdate();
        }
    }
}


