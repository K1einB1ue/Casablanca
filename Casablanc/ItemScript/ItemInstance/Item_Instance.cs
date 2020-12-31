using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Instance : SingletonMono<Item_Instance>
{
    Item AK_47;
    Item NormalBackpack;

    GameObject[] gameObjects;
    void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Drop");
        for(int i = 0; i < gameObjects.Length; i++) {
            AK_47 = new AllGun.AK_47();
            NormalBackpack = new AllContainer.NormalBackpack();
            AK_47.Drop(gameObjects[i].transform.position);
            NormalBackpack.Drop(gameObjects[i].transform.position);
        }
    }
}
