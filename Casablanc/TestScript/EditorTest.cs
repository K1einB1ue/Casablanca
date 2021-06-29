using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class EditorTest : MonoBehaviour
{
    public string Content;
    public List<string> Test = new List<string>();
}


[CustomEditor(typeof(EditorTest))]
public class EditorTestEditor : Editor
{
    VisualElement Root;
    TextField Content;
    Button AddButton;
    Button DecButton;
    Foldout Foldout;
    EditorTest editorTest;
    VisualElement FoldContent;

    ListFieldContainer List;

    public override VisualElement CreateInspectorGUI() {
        editorTest = target as EditorTest;

        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/UXML/对话节点条");
        Root = visualTreeAsset.CloneTree();
        AddButton = Root.Q<Button>("AddButton");
        DecButton = Root.Q<Button>("DecButton");
        Foldout = Root.Q<Foldout>("Foldout");
        FoldContent = Root.Q<VisualElement>("FoldContent");
        Content = Root.Q<TextField>("Content");

        List = new ListFieldContainer(FoldContent, Root, serializedObject.FindProperty(nameof(editorTest.Test)), () => { return Foldout.value; });

        Content.bindingPath = nameof(editorTest.Content);
        AddButton.RegisterCallback<ClickEvent>(evt => {
            List.Add("Null");
            List.Refresh();
            
        });
        DecButton.RegisterCallback<ClickEvent>(evt => {
            List.RemoveAt(List.Count - 1);
            List.Refresh();
        });
        Foldout.RegisterValueChangedCallback(evt => {
            List.Refresh();
        });
        List.Refresh();
        


        return Root;
        
    }

   
}



public class ListFieldContainer
{
    public ListFieldContainer(VisualElement visualElement,VisualElement root, SerializedProperty Array,Func<bool> visiable) {
        this.This = visualElement;
        this.Root = root;
        this.Content = Array;
        this.Visiable = visiable;
        this.VisiableOn();     
    }

    private Func<bool> Visiable;
    public int Count => Content.arraySize;
    private SerializedProperty Content;
    private VisualElement This;
    private VisualElement Root;
    private List<TextField> List;
    private bool Layout = false;


    private void VisiableOn() {
        if (!Layout) {
            if (this.List == null) {
                this.List = new List<TextField>();
                for (int i = 0; i < Count; i++) {
                    TextField tmp = new TextField();
                    var property = Content.GetArrayElementAtIndex(i);
                    tmp.BindProperty(property);
                    this.List.Add(tmp);
                    this.This.Add(tmp);
                }
                this.Layout = true;
            }
            else {
                for (int i = 0; i < Count; i++) {
                    this.This.Add(this.List[i]);
                }
                this.Layout = true;
            }
            this.Refresh();
        }
    }
    public void Add(string index) {
        VisiableOn();
        Content.InsertArrayElementAtIndex(Content.arraySize);
        Content.serializedObject.Update();
        
        TextField tmp = new TextField();
        var property = Content.GetArrayElementAtIndex(Content.arraySize - 1);
        tmp.BindProperty(property);
        property.stringValue = index;
        this.List.Add(tmp);
        this.This.Add(tmp);
        this.Refresh();
        
    }

    public void RemoveAt(int index) {
        VisiableOn();
        Content.DeleteArrayElementAtIndex(index);
        Content.serializedObject.Update();
        try {
            this.List.RemoveAt(index);
            this.This.RemoveAt(index);
        }
        catch {
            Debug.Log("");
        }
        this.Refresh();
    }
    public void Refresh() {
        VisiableOn();
        if (this.Count == 0) {
            this.This.visible = false;
            this.This.Clear();
            this.Layout = false;
        }
        else {
            if (this.Visiable.Invoke()) {
                this.This.visible = true;
                this.Root.MarkDirtyRepaint();
            }
            else {
                this.This.visible = false;
                this.This.Clear();
                this.Layout = false;
            }
        }
    }

    private void VisiableOff() {

    }


}




//[Serializable]
//public class DialogContentPack
//{
//    public string Content;
//}

//[CustomPropertyDrawer(typeof(DialogContentPack))]
//public class DialogContentPackPropertyDrawer : PropertyDrawer
//{
//    VisualElement Root;
//    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
//        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("UI/UXML/对话节点条.uxml");
//        Root = visualTreeAsset.CloneTree();
//        var AddButton = Root.Q<Button>("AddButton");
//        var DecButton = Root.Q<Button>("DecButton");



//        return Root;
//    }

//}



