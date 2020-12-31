using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverGroup
{
    private List<GameObject> gameObjects = new List<GameObject>();
    public CoverRuntimePack coverRuntimePack = new CoverRuntimePack();

    public void Enter(GameObject gameObject) {
        this.gameObjects.Add(gameObject);
    }
    public void CoverOff(Func<GameObject, bool> Func) {
        for(int i = 0; i < gameObjects.Count; i++) {
            if (Func(gameObjects[i])) {
                gameObjects[i].GetComponent<Renderer>().material.SetFloat("Cover", 0.0f);
            }
        }
    }
    public void CoverOn(Func<GameObject, bool> Func) {
        for (int i = 0; i < gameObjects.Count; i++) {
            if (Func(gameObjects[i])) {
                gameObjects[i].GetComponent<Renderer>().material.SetFloat("Cover", 1.0f);
            }
        }
    }
}

public class CoverRuntimePack
{
    public int hash = -1;
    public int count = 0;

}


