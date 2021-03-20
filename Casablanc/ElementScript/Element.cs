using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditorInternal;
using ItemStaticProperties.StaticValues;
using ItemRuntimeProperties.RuntimeValues;

public interface IThermodynamics_Unit
{
    float SHC { get; }
    float Temperature { get; }
    void ThermodynamicsAdjustment(float Energy);
}


public interface Element_Thermodynamics_Handler: IThermodynamics_Unit 
{
    
}

public enum ElementState
{
    Solid,
    Liquid,
    Gas,
    Plasma,
}
public class ElementManager:SingletonMono<ElementManager>
{
    public static IEnumerable<Element> Chemical_reaction(IEnumerable<Element> Input) {
        yield return null;
    }
}

[Serializable]
public class ElementStaticValues
{
    public float Temperature_Low;
    public float Temperature_High;
    public float specific_heat_capacity;
}
[Serializable]
public class Element: Element_Thermodynamics_Handler
{
    public float SHC => this.ElementStaticValues.specific_heat_capacity * this.KG;
    private ElementStaticValues ElementStaticValues => StaticPath.ElementLoad[this.ElementType].ElementStaticValues; 
    public ElementState ElementState
    {
        get {
            if (this.IsPlasma) {
                return ElementState.Plasma;
            }
            else if (this.temperature < ElementStaticValues.Temperature_Low) {
                return ElementState.Solid;
            }
            else if (this.temperature > ElementStaticValues.Temperature_High) {
                return ElementState.Gas;
            }
            else {
                return ElementState.Liquid;
            }
        }
    }
    public float Temperature => this.temperature;
    public void ThermodynamicsAdjustment(float Energy) {
        this.temperature += Energy / (this.KG * this.ElementStaticValues.specific_heat_capacity);
    }

    [HideInInspector]
    public bool IsPlasma = false;
    public string ElementType = "无";
    [HideInInspector]
    public float temperature = ThermodynamicsManager.EnvironmentTemperature;
    public float KG = 1f;
}
 





[CustomPropertyDrawer(typeof(Element))]
public class ElementTypeEditor : PropertyDrawer
{
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        int value = 0;
        List<string> elements = new List<string>();
        elements.Add("无");
        for (int i = 0; i < StaticPath.ElementLoad.elementlist.Count; i++) {
            if (StaticPath.ElementLoad.elementlist[i] != null) {
                if(property.FindPropertyRelative("ElementType").stringValue== StaticPath.ElementLoad.elementlist[i].name) {
                    value = i + 1;
                }
                elements.Add(StaticPath.ElementLoad.elementlist[i].name);
            }
        }
        if (value != 0) {
            elements.Remove("无");
            value--;
        }
        using (new EditorGUI.PropertyScope(position, label, property)) {
            EditorGUIUtility.labelWidth = 60;
            position.height = EditorGUIUtility.singleLineHeight;

            value = EditorGUI.Popup(position, value, elements.ToArray());

            Rect KG_Rec = new Rect(position)
            {
                width = 250,
                y = position.y + EditorGUIUtility.singleLineHeight + 2
            };
            EditorGUI.PropertyField(KG_Rec, property.FindPropertyRelative("KG"), new GUIContent("质量/KG"));
            }

            property.FindPropertyRelative("ElementType").stringValue = elements[value];       
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 40;
    }
}
