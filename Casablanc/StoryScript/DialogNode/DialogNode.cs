using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XNode;
using XNodeEditor;



public class DialogNode : DialogNodeStatic, Dialog
{
    [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_Dialog                            @前置对话;
    [Output(dynamicPortList =true,connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_Dialog     @后触对话;
    [TextArea]
    public List<string> Content=new List<string>();
    [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)] public StateMachineInterface                  @状态机;

    public Dialog GetDialog(int num) {
        return new DialogGroup(DialogGroup.GetDialogs(this, num));
    }
    public string this[int x] { 
        get {
            return Content[x];
        }
        set {
            Content[x] = value;
        } 
    }
    public int Count => Content.Count;

    public DialogGroup DialogGroupLize() {
        return new DialogGroup(this.GetDialogs());
    }

}

public class DialogGroup : IEnumerable, Dialog
{
    public int Count {
        get {
            int tmp = 0;
            foreach (var node in dialogNodes) {
                tmp += node.Count;
            }
            return tmp;
        } 
    }
    public List<DialogNode> dialogNodes = new List<DialogNode>();
    public static StringBuilder stringBuilder = new StringBuilder();
    public IEnumerator<DialogNode> GetEnumerator() {
        return this.dialogNodes.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator() {
        return this.dialogNodes.GetEnumerator();
    }

    public DialogGroup(IEnumerable<DialogNode> dialogs) {
        foreach(var Node in dialogs) {
            if (Node != null) {
                dialogNodes.Add(Node);
            }
        }
    }
    public DialogGroup(DialogNode dialog) {
        if (dialog != null) {
            dialogNodes.Add(dialog);
        }
    }
    public Dialog GetDialog(int num) {   
        int i = 0;
        while (num > dialogNodes[i].Content.Count - 1) {
            num = num - dialogNodes[i++].Content.Count;
        }

        return new DialogGroup(GetDialogs(dialogNodes[i], num));
    } 

    public string this[int x] { 
        get {
            int i = 0;
            while (x > dialogNodes[i].Content.Count - 1) {
                x = x - dialogNodes[i++].Content.Count;
            }
            return dialogNodes[i][x];
        } 
        set {
            int i = 0;
            while (x > dialogNodes[i].Content.Count - 1) {
                x = x - dialogNodes[i++].Content.Count;
            }
            dialogNodes[i][x] = value;
        }
    }

    public static IEnumerable<DialogNode> GetDialogs(DialogNode Dialognode,int num) {
        stringBuilder.Clear();
        stringBuilder.Append("后触对话 ");
        stringBuilder.Append(num.ToString());
        var Port = Dialognode.GetPort(stringBuilder.ToString());
        if (Port.ConnectionCount > 0) {
            foreach (var Con in Port.GetConnections()) {
                var Node = Con.node;
                if (((INode)Node).GetNodeType() == NodeType.DialogNode) {
                    ((IDialog)Node).SetUpdateType(Dialog_UpdateType.Enable);
                    if (Node.GetType() == typeof(DialogNode)) {
                        yield return (DialogNode)Node;
                    }
                    if (Node.GetType() == typeof(DialogTriggerNode)) {
                        Dialognode.StoryBlock.UpdateStoryNodes();
                    }
                }
                else {
                    throw new Exception("错误的剧情节点连接!");
                }
            }
        }
    }
    
    

    public static Dialog operator +(DialogGroup dialog1,DialogGroup dialog2) {
        return new DialogGroup(DialogNodeEx.GetDialogs(null, dialog1, dialog2));
    }

    public DialogGroup DialogGroupLize() {
        return this;
    }
}


public static class DialogNodeEx
{
    public static IEnumerable<DialogNode> GetDialogs(this DialogNode dialogNode, params DialogGroup[] dialogGroups) {
        if (dialogNode != null) {
            yield return dialogNode;
        }
        foreach (var Group in dialogGroups) {
            foreach (var Node in Group) {
                if (Node != null) {
                    yield return Node;
                }
            }
        }
    }
}
public interface Dialog
{
    Dialog GetDialog(int num);
    string this[int x] { get; set; }
    int Count { get; }
    DialogGroup DialogGroupLize();
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
