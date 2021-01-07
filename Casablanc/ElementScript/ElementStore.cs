using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="新元素", menuName ="元素/新元素")]
public class ElementStore : ScriptableObject
{
    //public ElementType ElementType = ElementType.None;
    public ElementStaticValues ElementStaticValues = new ElementStaticValues();
}
