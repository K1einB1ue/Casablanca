using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XNode;
using XNodeEditor;



public class DialogNode : DialogNodeStatic
{
    [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)] public Link_Dialog                            @前置对话;
    [Output(dynamicPortList =true,connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)] public Link_Dialog     @后触对话;
    [TextArea]
    public List<string> Content=new List<string>();
    [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)] public StateMachineInterface                  @状态机;

    public DialogNode GetDiaglog(int num) {
        stringBuilder.Clear();
        stringBuilder.Append("后触对话 ");
        stringBuilder.Append(num.ToString());
        var Port = this.GetPort(stringBuilder.ToString());
        if (Port.ConnectionCount > 0) {


            var Node = Port.Connection.node;
            if (((INode)Node).GetNodeType() == NodeType.DialogNode) {
                ((IDialog)Node).SetUpdateType(Dialog_UpdateType.Enable);
                if (Node.GetType() == typeof(DialogNode)) {
                    return (DialogNode)Node;
                }
                if (Node.GetType() == typeof(DialogTriggerNode)) {
                    this.StoryBlock.UpdateStoryNodes();
                }
                return null;
            }
            else {
                throw new Exception("错误的剧情节点连接!");
            }
        }
        return null;
    }

}




[CustomNodeEditor(typeof(DialogNode))]
public class DialogNodeEditor : NodeEditor
{
    public static StringBuilder StringBuilder = new StringBuilder();
    public static List<string> PortsHas = new List<string>();
    public override void OnBodyGUI() {
        DialogNode node = target as DialogNode;
        base.OnBodyGUI();
        StringBuilder.Clear();
        foreach (NodePort nodePort in node.DynamicOutputs) {
            PortsHas.Add(nodePort.fieldName);
        }
        for (int i = 0; i < node.Content.Count; i++) {
            StringBuilder.Clear();
            StringBuilder.Append("后触对话 ");
            StringBuilder.Append(i.ToString());
            if (!PortsHas.Remove(StringBuilder.ToString())) {
                node.AddDynamicOutput(typeof(DialogNode), Node.ConnectionType.Override, Node.TypeConstraint.Strict, StringBuilder.ToString());
            }
        }
        if (PortsHas.Count > 0) {
            for (int i = PortsHas.Count - 1; i > 0; i--) {
                node.RemoveDynamicPort(PortsHas[i]);
                PortsHas.RemoveAt(i);
            }
        }

    }
}



public interface IDialog {
    Dialog_UpdateType GetUpdateType();
    void SetUpdateType(Dialog_UpdateType updateType);

}
[Serializable]
public abstract class DialogNodeStatic : NodeStatic,IDialog
{
    [SerializeField]
    private Dialog_UpdateType updateType = Dialog_UpdateType.Unable;
    public static StringBuilder stringBuilder = new StringBuilder();
    public Dialog_UpdateType GetUpdateType() { return this.updateType; }
    void IDialog.SetUpdateType(Dialog_UpdateType updateType) { this.updateType = updateType; }
    public override NodeType GetNodeType() {
        return NodeType.DialogNode;
    }
    public override void ReStruct() {
        this.updateType = Dialog_UpdateType.Unable;
    }
}

public enum Dialog_UpdateType {
    Unable,
    Enable
}

[Serializable]
public class Link_Dialog { }

[Serializable]
public class StateMachineInterface {
    

}
