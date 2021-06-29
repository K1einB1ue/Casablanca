using UnityEngine;
using XNodeEditor;
using XNode;
using UnityEditor;

public class DialogMachineGroupNode : ComponentNodeBase
{
    [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachine @�Ի�״̬��;
    [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachineGroup @ӿ��״̬��;

    public DialogMachineGroupPack @�Ի�Ⱥ��;
    public override void OnCreateConnection(NodePort from, NodePort to) {
        this.ReStruct();
    }
    public override void OnRemoveConnection(NodePort port) {
        this.ReStruct();
    }

    public override object GetValue(NodePort port) {
        if (this.�Ի�Ⱥ��) {
            return this.�Ի�Ⱥ��;
        }
        return null;
    }
    private void Rename() {
        this.name = "NULL";
        if (this.�Ի�Ⱥ��.Count > 0) {
            this.name = this.�Ի�Ⱥ��.ToString();
        }
    }
    private void ReBind() {
        bool SummonNew = false;
        if (!this.�Ի�Ⱥ��) {
            this.�Ի�Ⱥ�� = ScriptableObject.CreateInstance<DialogMachineGroupPack>();
            SummonNew = true;
        
        }
        this.�Ի�Ⱥ��.Clear();
        if (this.GetPort(nameof(�Ի�״̬��)).ConnectionCount > 0) {
            foreach (var Port in this.GetPort(nameof(�Ի�״̬��)).GetConnections()) {
                if (Port.GetOutputValue() is DialogMachine) {
                    this.�Ի�Ⱥ��.StoreAdd((DialogMachine)Port.GetOutputValue());
                }
                else {
                    Debug.LogError("��������");
                }
            }
        }
        if (SummonNew) {
            AssetDatabase.CreateAsset(this.�Ի�Ⱥ��, "Assets/Resources/��/�洢��/�Ի�Ⱥ��/" + this.�Ի�Ⱥ��.ToString() + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    public override void ReStruct() {
        this.ReBind();
        this.Rename();
        if (this.�Ի�Ⱥ��) {
            this.�Ի�Ⱥ��.Rename();
        }
    }
}

[CustomNodeEditor(typeof(DialogMachineGroupNode))]
public class DialogMachineGroupNodeEditor : NodeEditor
{
    public override void OnBodyGUI() {
        var tmp = ((DialogMachineGroupNode)target);
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(tmp.�Ի�Ⱥ��)));
        NodeEditorGUILayout.PortPair(tmp.GetPort(nameof(tmp.�Ի�״̬��)), tmp.GetPort(nameof(tmp.ӿ��״̬��)));
    }
}
