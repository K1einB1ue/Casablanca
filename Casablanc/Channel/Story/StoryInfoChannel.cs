using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "剧情接口", menuName = "固化接口/剧情接口")]
public class StoryInfoChannel : InfoChannel
{
    [SerializeField]
    private List<DialogMachineGroup> DialogMachineGroups = new List<DialogMachineGroup>();

    [SerializeField]
    private List<DialogNode> DialogNodesTable = new List<DialogNode>();



    private Dictionary<DialogMachineGroup, LinkedList<DialogNode>> DialogEnableMap;
    private List<DialogMachine> DisableTable = new List<DialogMachine>();


    public StoryBlock Main;








    public void UpdateDialog(DialogMachineGroup Group) {
        this.ClearDialogMachine();
        this.DisableTable.Clear();
        foreach(var pair in DialogEnableMap) {
            if (pair.Key.Match(Group)) {
                pair.Value.First.Value.DialogMachine.Dialog = pair.Value.First.Value;
                this.DisableTable.Add(pair.Value.First.Value.DialogMachine);
            }
        }
    }
    
    public void EnableANode(DialogNode dialogNode) {
        if(DialogEnableMap.TryGetValue(dialogNode.DialogMachineGroup,out var dialogNodes)) {
            dialogNodes ??= new LinkedList<DialogNode>();
            dialogNodes.AddLast(dialogNode);
        }
        else {
            var tmp = new LinkedList<DialogNode>();
            DialogEnableMap.Add(dialogNode.DialogMachineGroup, tmp);
            tmp.AddLast(dialogNode);
        }
    }
    public void DisAbleANode(DialogNode dialogNode) {
        if (DialogEnableMap[dialogNode.DialogMachineGroup].Remove(dialogNode)) {
            if (DialogEnableMap[dialogNode.DialogMachineGroup].Count==0) {
                DialogEnableMap.Remove(dialogNode.DialogMachineGroup);
            }
        }
        else {
            Debug.LogError("可能发生了错误");
        }
    }
    public override void onEnable() {
        this.DialogEnableMap ??= new Dictionary<DialogMachineGroup, LinkedList<DialogNode>>();
        int mark = 0;
        for (int i = 0; i < DialogMachineGroups.Count; i++) {
            var tmp = new LinkedList<DialogNode>();
            this.DialogEnableMap.Add(DialogMachineGroups[i], tmp);
            while (DialogNodesTable[mark] != null) {
                tmp.AddLast(DialogNodesTable[mark]);
                mark++;
            }
            mark++;
        }
        foreach (var tmp in DialogMachineGroups) {
            tmp.Init();
        }
    }

    public override void onDisable() {
        

        this.ClearDialogMachine();


        this.DialogMachineGroups.Clear();
        this.DialogNodesTable.Clear();
        foreach (var pair in DialogEnableMap) {
            this.DialogMachineGroups.Add(pair.Key);

            var ptr = pair.Value.First;
            while (ptr.Next != null) {
                this.DialogNodesTable.Add(ptr.Value);
                ptr = ptr.Next;
            }
            this.DialogNodesTable.Add(ptr.Value);
            this.DialogNodesTable.Add(null);
        }
        foreach (var tmp in DialogMachineGroups) {
            tmp.Save();
        }

        base.onDisable();
        EditorUtility.SetDirty(this);
    }

    private void ClearDialogMachine() {
        for (int i = 0; i < DisableTable.Count; i++) {
            DisableTable[i].Clear();
        }
    }
    public void ReStruct() {
        this.DialogMachineGroups.Clear();
        this.DialogNodesTable.Clear();
        EditorUtility.SetDirty(this);
    }
}

[CustomEditor(typeof(StoryInfoChannel))]
public class StoryInfoChannelEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("重构")) {
            ((StoryInfoChannel)target).ReStruct();
            ((StoryInfoChannel)target).Main.ReStruct();
        }
    }
}