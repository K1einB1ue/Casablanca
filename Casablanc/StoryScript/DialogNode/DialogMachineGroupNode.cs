using UnityEngine;
using XNodeEditor;
using XNode;
using UnityEditor;

public class DialogMachineGroupNode : ComponentNodeBase
{
    [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachine @对话状态机;
    [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachineGroup @涌现状态机;

    public DialogMachineGroupPack @对话群包;
    public override void OnCreateConnection(NodePort from, NodePort to) {
        this.ReStruct();
    }
    public override void OnRemoveConnection(NodePort port) {
        this.ReStruct();
    }

    public override object GetValue(NodePort port) {
        if (this.对话群包) {
            return this.对话群包;
        }
        return null;
    }
    private void Rename() {
        this.name = "NULL";
        if (this.对话群包.Count > 0) {
            this.name = this.对话群包.ToString();
        }
    }
    private void ReBind() {
        bool SummonNew = false;
        if (!this.对话群包) {
            this.对话群包 = ScriptableObject.CreateInstance<DialogMachineGroupPack>();
            SummonNew = true;
        
        }
        this.对话群包.Clear();
        if (this.GetPort(nameof(对话状态机)).ConnectionCount > 0) {
            foreach (var Port in this.GetPort(nameof(对话状态机)).GetConnections()) {
                if (Port.GetOutputValue() is DialogMachine) {
                    this.对话群包.StoreAdd((DialogMachine)Port.GetOutputValue());
                }
                else {
                    Debug.LogError("错误链接");
                }
            }
        }
        if (SummonNew) {
            AssetDatabase.CreateAsset(this.对话群包, "Assets/Resources/池/存储器/对话群包/" + this.对话群包.ToString() + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    public override void ReStruct() {
        this.ReBind();
        this.Rename();
        if (this.对话群包) {
            this.对话群包.Rename();
        }
    }
}

[CustomNodeEditor(typeof(DialogMachineGroupNode))]
public class DialogMachineGroupNodeEditor : NodeEditor
{
    public override void OnBodyGUI() {
        var tmp = ((DialogMachineGroupNode)target);
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(tmp.对话群包)));
        NodeEditorGUILayout.PortPair(tmp.GetPort(nameof(tmp.对话状态机)), tmp.GetPort(nameof(tmp.涌现状态机)));
    }
}
