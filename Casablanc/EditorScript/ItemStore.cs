using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "新物品", menuName = "物品/新物品")]
[System.Serializable]
public class ItemStore : ScriptableObject
{
    [Header("物品原型")]
    public GameObject ItemOrigin;
    [Header("物品栏贴图")]
    public Sprite sprite;

    [Header("静态物品配置")]
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;

}

[Serializable]
public class ItemRandom
{
    public float heldrate;

    public static int Intmax(int held,float index) {
        return (int)(held * (1.0f / index));
    }
}
