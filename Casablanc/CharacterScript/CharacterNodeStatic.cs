using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "新静态角色节点", menuName = "角色/静态角色节点")]
public class CharacterNodeStatic : ScriptableObject, CharacterNode
{
    public CharacterStaticInfoPackage CharacterStaticInfoPackage;
    public CharacterPreInstanceInfoPackage CharacterPreInstanceInfoPackage;
    public Character GetCharacter() {
        Character character;
        if (this.CharacterStaticInfoPackage.characterStaticDescribeWays == CharacterStaticDescribeWays.CharacterStore) {
            character = Characters.GetCharacterByCharacterID(this.CharacterStaticInfoPackage.CharacterStore.CharacterStaticProperties.CharacterID);
        }
        else if (this.CharacterStaticInfoPackage.characterStaticDescribeWays== CharacterStaticDescribeWays.CharacterID) {
            character = Characters.GetCharacterByCharacterID(this.CharacterStaticInfoPackage.CharacterStaticProperties.CharacterID);
        }
        else {
            character = null;
        }


        if (this.CharacterStaticInfoPackage.characterStaticDescribeWays == CharacterStaticDescribeWays.CharacterStore) {
            character.Info_Handler.Binding(new Character_Property(this.CharacterStaticInfoPackage.CharacterStore.CharacterStaticProperties.CharacterID, this.CharacterPreInstanceInfoPackage.CharacterPreInstanceProperties));
        }
        else if (this.CharacterStaticInfoPackage.characterStaticDescribeWays == CharacterStaticDescribeWays.CharacterID) {
            character.Info_Handler.Binding(new Character_Property(this.CharacterStaticInfoPackage.CharacterStaticProperties.CharacterID, this.CharacterPreInstanceInfoPackage.CharacterPreInstanceProperties));
        }
        else {
            character = null;
        }

        
        return character;
    }
}


public interface CharacterNode
{
    Character GetCharacter();
}

[Serializable]
public class CharacterNodeDynamic : CharacterNode 
{
    public CharacterRuntimeInfoPackage CharacterRuntimeInfoPackage;
    public Character GetCharacter() {
        Character tmp = Characters.GetCharacterByCharacterID(CharacterRuntimeInfoPackage.CharacterRuntimeProperties.CharacterID);
        tmp.Info_Handler.Binding(new Character_Property(this.CharacterRuntimeInfoPackage.CharacterRuntimeProperties));
        return tmp;
    }
}


public enum CharacterStaticDescribeWays
{
    CharacterStore,
    CharacterID,

}
[Serializable]
public class CharacterStaticInfoPackage
{
    public CharacterStaticDescribeWays characterStaticDescribeWays = CharacterStaticDescribeWays.CharacterStore;
    public CharacterStore CharacterStore;
    public CharacterStaticProperties.CharacterStaticProperties CharacterStaticProperties;

    public CharacterStaticProperties.CharacterStaticProperties GetCharacterStaticProperty() {
        if(this.characterStaticDescribeWays== CharacterStaticDescribeWays.CharacterStore) {
            return StaticPath.CharacterLoad[this.CharacterStore.CharacterStaticProperties.CharacterID].CharacterStaticProperties;

        }
        if(this.characterStaticDescribeWays== CharacterStaticDescribeWays.CharacterID) {
            return this.CharacterStaticProperties;
        }
        return null;
    }

}
[Serializable]
public class CharacterRuntimeInfoPackage
{
    public CharacterRuntimeProperties.CharacterRuntimeProperties CharacterRuntimeProperties;

    public CharacterRuntimeInfoPackage(Character character) {
        CharacterRuntimeProperties = character.GetCharacterProperty().CharacterRuntimeProperties;
    }

}
[Serializable]
public class CharacterPreInstanceInfoPackage
{
    public CharacterPreInstanceProperties.CharacterPreInstanceProperties CharacterPreInstanceProperties = new CharacterPreInstanceProperties.CharacterPreInstanceProperties();

}


[CustomEditor(typeof(CharacterNodeStatic))]
public class CharacterNodeStaticEditor : Editor
{
    SerializedProperty CharacterStaticInfoPackage;
    SerializedProperty characterStaticDescribeWays;
    SerializedProperty CharacterStore;
    SerializedProperty CharacterStaticProperties;
    SerializedProperty CharacterPreInstanceInfoPackage;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        CharacterNodeStatic characterNodeStatic = target as CharacterNodeStatic;
        CharacterStaticInfoPackage = serializedObject.FindProperty("CharacterStaticInfoPackage");
        characterStaticDescribeWays = CharacterStaticInfoPackage.FindPropertyRelative("characterStaticDescribeWays");
        CharacterStore = CharacterStaticInfoPackage.FindPropertyRelative("CharacterStore");
        CharacterStaticProperties = CharacterStaticInfoPackage.FindPropertyRelative("CharacterStaticProperties");
        CharacterPreInstanceInfoPackage = serializedObject.FindProperty("CharacterPreInstanceInfoPackage");


        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(characterStaticDescribeWays);
        if (characterNodeStatic.CharacterStaticInfoPackage.characterStaticDescribeWays ==  CharacterStaticDescribeWays.CharacterStore) {
            EditorGUILayout.PropertyField(CharacterStore);
        }
        else if (characterNodeStatic.CharacterStaticInfoPackage.characterStaticDescribeWays == CharacterStaticDescribeWays.CharacterID) {
            EditorGUILayout.PropertyField(CharacterStaticProperties.FindPropertyRelative("CharacterID"));
        }
        EditorGUILayout.PropertyField(CharacterPreInstanceInfoPackage);

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        //base.OnInspectorGUI();
    }


}