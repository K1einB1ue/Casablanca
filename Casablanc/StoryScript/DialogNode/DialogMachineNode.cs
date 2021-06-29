using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

public class DialogMachineNode : ComponentNodeBase
{
    [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] public Link_DialogMachine @对话状态机;
    public DialogMachine 对话状态机存储;

    public override object GetValue(NodePort port) {
        return this.对话状态机存储;
    }
    public override void ReStruct() {
        base.ReStruct();
        this.Rename();
        if (this.对话状态机存储 != null) {
            this.对话状态机存储.Dialog = null;
            this.对话状态机存储.currentNode = null;
            this.对话状态机存储.currentGroup = null;
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
        if (this.对话状态机存储) {
            this.name = this.对话状态机存储.name;
        }
    }
}

[CustomNodeEditor(typeof(DialogMachineNode))]
public class DialogMachineNodeEditor : NodeEditor
{
    public override Color GetTint() {
        if (((DialogMachineNode)target).对话状态机存储) {
            string tmp = ((DialogMachineNode)target).对话状态机存储.name;
            int hash = tmp.GetHashCode();
            float r = ((hash & 0xFF0000) >> 16) / 255f;
            float g = ((hash & 0x00FF00) >> 8) / 255f;
            float b = (hash & 0x0000FF) / 255f;
            return new Color(r, g, b, 1);
        }
        return base.GetTint(); ;
    }
}