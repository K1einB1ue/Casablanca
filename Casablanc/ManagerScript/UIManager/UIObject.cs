using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "新界面", menuName = "界面/新界面")]
public class UIObject : ScriptableObject
{
    [SerializeField]
    public GameObject UI;
    [SerializeField]
    public int ID;
    [SerializeField]
    public int Size;
}
