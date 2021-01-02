using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "新物品", menuName = "物品/新物品")]
public class ItemStore : ScriptableObject
{

    [Header("静态物品配置")]
    public ItemStaticProperties.ItemStaticProperties ItemStaticProperties;

}

