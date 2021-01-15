using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "新存储", menuName = "角色/动静态混合存储")]
public class CharacterInfoStore : ScriptableObject
{
    public CharacterNodeStatic characterNodeStatic;
    public CharacterNodeDynamic characterNodeDynamic;

    

    public bool save = false;
    public bool init = false;


    public Character GetCharacter() {
        if (save) {
            if (!this.init) {
                this.init = true;
                return this.characterNodeStatic.GetCharacter();
            }
            else {
                return this.characterNodeDynamic.GetCharacter();
            }           
        }
        else {
            return this.characterNodeStatic.GetCharacter();
        }
    }


    public void Save(Character character) {
        CharacterNodeDynamic node = new CharacterNodeDynamic(character);
        this.characterNodeDynamic = node;
    }
}


[CustomEditor(typeof(CharacterInfoStore))]
public class CharacterInfoStoreEditor : Editor
{
    public override void OnInspectorGUI() {
        CharacterInfoStore infoStore = target as CharacterInfoStore;
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("save"), new GUIContent("是否保存"));
        if (infoStore.save) {
            if (infoStore.init) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("characterNodeStatic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("characterNodeDynamic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("init"), new GUIContent("是否已初始化"));
            }
            else {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("characterNodeStatic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("init"), new GUIContent("是否已初始化"));
            }
        }
        else {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("characterNodeStatic"));
        }

        if (EditorGUI.EndChangeCheck()) {
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}

