using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

public class DialogMachineNode : ComponentNodeBase
{
    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_DialogMachine @�Ի�״̬��;
    public DialogMachine �Ի�״̬���洢;

    public override object GetValue(NodePort port) {
        return this.�Ի�״̬���洢;
    }
    public override void ReStruct() {
        base.ReStruct();
        this.Rename();
        if (this.�Ի�״̬���洢 != null) {
            this.�Ի�״̬���洢.Dialog = null;
            this.�Ի�״̬���洢.currentNode = null;
            this.�Ի�״̬���洢.currentGroup = null;
        }
    }
    public override NodeType GetNodeType() {
        return NodeType.DialogNode;
    }

    public override void OnCreateConnection(NodePort from, NodePort to) {
        this.Rename();
    }
    public override void OnRemoveConnection(NodePort port) {
        this.Rename();
    }
    private void Rename() {
        this.name = "NULL";
        if (this.�Ի�״̬���洢) {
            this.name = this.�Ի�״̬���洢.name;
        }
    }
}

[CustomNodeEditor(typeof(DialogMachineNode))]
public class DialogMachineNodeEditor : NodeEditor
{
    public override Color GetTint() {
        if (((DialogMachineNode)target).�Ի�״̬���洢) {
            string tmp = ((DialogMachineNode)target).�Ի�״̬���洢.name;
            int hash = tmp.GetHashCode();
            float r = ((hash & 0xFF0000) >> 16) / 255f;
            float g = ((hash & 0x00FF00) >> 8) / 255f;
            float b = (hash & 0x0000FF) / 255f;
            return new Color(r, g, b, 1);
        }
        return base.GetTint(); ;
    }
}