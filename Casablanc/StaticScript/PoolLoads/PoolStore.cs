using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "新物品池原型", menuName = "静态加载/新物品池原型")]
public class PoolStore : ScriptableObject
{
    public GameObject Origin;
    public int ID;
}
