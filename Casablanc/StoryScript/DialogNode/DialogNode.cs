using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using XNode;
using XNodeEditor;


[NodeWidth(300)]
public class DialogNode : DialogNodeStatic, Dialog
{
    [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public string                                  @前置对话;
    [TextArea(4, 20)]
    [Output(dynamicPortList =true,connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public List<string>     @后触对话;
    
    [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachine                  @对话状态机;
    [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachine                 @对话状态机_;
    [Input(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachineGroup             @涌现状态机;
    [Output(connectionType = ConnectionType.Multiple, typeConstraint = TypeConstraint.Strict)] public Link_DialogMachineGroup            @涌现状态机_;



   

    public DialogMachine DialogMachine {
        get {
            if (this.GetPort(nameof(对话状态机)).ConnectionCount > 0) {
                return (DialogMachine)this.GetPort(nameof(对话状态机)).Connection.GetOutputValue();
            }
            return null;
        }
    }

    public DialogMachineGroup DialogMachineGroup {
        get {
            DialogMachineGroup dialogMachineGroup = null;
            foreach (var Port in this.GetPort(nameof(涌现状态机)).GetConnections()) {
                if (Port.GetOutputValue() is DialogMachine) {
                    dialogMachineGroup ??= new DialogMachineGroup();
                    dialogMachineGroup.PackUp((DialogMachine)Port.GetOutputValue());
                }
                else if (Port.GetOutputValue() is DialogMachineGroup) {
                    if (dialogMachineGroup != null) {
                        Debug.LogError("错误的链接方式");
                    }
                    return ((DialogMachineGroup)Port.GetOutputValue());
                }
            }
            return dialogMachineGroup;
        }
    }
    public Dialog GetDialog(int num) {
        this.SelectDialog(num);
        return new DialogGroup(DialogGroup.GetDialogs(this, num));
    }
    public void SelectDialog(int num) {
        DialogGroup.SelectDialog(this, num);
    }
    public string this[int x] { 
        get {
            return 后触对话[x];
        }
        set {
            后触对话[x] = value;
        } 
    }
    public int Count => 后触对话.Count;

    public DialogGroup DialogGroupLize() {
        return new DialogGroup(this.GetDialogs());
    }
    public void OnPreSelected() {
        if (DialogMachineGroup!=null) {
            if (StoryChannelManager.Instance) {
                StoryChannelManager.Instance.InfoChannel.EnableANode(this);
            }
            
        }
        if (((IDialog)this).GetUpdateType() == Dialog_UpdateType.Unable) {
            ((IDialog)this).SetUpdateType(Dialog_UpdateType.PreEnable);
        }
    }

    public override object GetValue(NodePort port) {
        if (port.fieldName == nameof(对话状态机_)) {
            if (this.GetPort(nameof(对话状态机)).ConnectionCount > 0) {
                return this.GetPort(nameof(对话状态机)).Connection.GetOutputValue();
            }
        }
        else if (port.fieldName == nameof(涌现状态机_)) {
            if (this.GetPort(nameof(涌现状态机)).ConnectionCount > 0) {
                DialogMachineGroup dialogMachineGroup = null;
                foreach (var Port in this.GetPort(nameof(涌现状态机)).GetConnections()) {                 
                    if(Port.GetOutputValue() is DialogMachine) {
                        dialogMachineGroup ??= new DialogMachineGroup();
                        dialogMachineGroup.PackUp((DialogMachine)Port.GetOutputValue());
                    }
                    else if(Port.GetOutputValue() is DialogMachineGroup) {
                        if (dialogMachineGroup != null) {
                            Debug.LogError("错误的链接方式");
                        }
                        return Port.GetOutputValue();
                    }
                }
                return dialogMachineGroup;
            }
        }
        return null;
    }

    public override void OnCreateConnection(NodePort from, NodePort to) {
        this.Rename();
    }
    public override void OnRemoveConnection(NodePort port) {
        this.Rename();
    }
    public override void ReStruct() {
        base.ReStruct();
        this.Rename();
    }
    private void Rename() {
        this.name = "NULL";
        if (this.GetPort(nameof(对话状态机)).ConnectionCount > 0) {
            if (this.GetPort(nameof(对话状态机)).Connection.GetOutputValue() != null) {
                this.name = ((DialogMachine)this.GetPort(nameof(对话状态机)).Connection.GetOutputValue()).name;
            }
        }
        if (this.GetPort(nameof(涌现状态机)).ConnectionCount > 0) {
            DialogMachineGroup dialogMachineGroup = null;
            foreach (var Port in this.GetPort(nameof(涌现状态机)).GetConnections()) {
                if (Port.GetOutputValue() is DialogMachine) {
                    dialogMachineGroup ??= new DialogMachineGroup();
                    dialogMachineGroup.PackUp((DialogMachine)Port.GetOutputValue());
                }
                else if (Port.GetOutputValue() is DialogMachineGroup) {
                    if (dialogMachineGroup != null) {
                        Debug.LogError("错误的链接方式");
                    }
                    this.name += ((DialogMachineGroup)Port.GetOutputValue()).ToString();
                    return;
                }
            }
            this.name += dialogMachineGroup.ToString();
        }
    }
}

public class DialogGroup : ScriptableObject, IEnumerable, Dialog
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
        while (num > dialogNodes[i].后触对话.Count - 1) {
            num = num - dialogNodes[i++].后触对话.Count;
        }

        return new DialogGroup(GetDialogs(dialogNodes[i], num));
    }
    public void SelectDialog(int num) {
        int i = 0;
        while (num > dialogNodes[i].后触对话.Count - 1) {
            num = num - dialogNodes[i++].后触对话.Count;
        }

        SelectDialog(dialogNodes[i], num);
    }
    public static void SelectDialog(DialogNode Dialognode, int num) {
        stringBuilder.Clear();
        stringBuilder.Append("后触对话 ");
        stringBuilder.Append(num.ToString());

        StoryChannelManager.SelectDialogEvent?.Invoke(Dialognode.后触对话[num]);

        var Port = Dialognode.GetPort(stringBuilder.ToString());
        if (Port.ConnectionCount > 0) {
            foreach (var Con in Port.GetConnections()) {
                var Node = Con.node;
                if (((INode)Node).GetNodeType() == NodeType.DialogNode) {
                    ((IDialog)Node).SetUpdateType(Dialog_UpdateType.Enable);
                    if (Node.GetType() == typeof(DialogNode)) {
                        ((DialogNode)Node).OnPreSelected();
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
    public string this[int x] { 
        get {
            int i = 0;
            while (x > dialogNodes[i].后触对话.Count - 1) {
                x = x - dialogNodes[i++].后触对话.Count;
            }
            return dialogNodes[i][x];
        } 
        set {
            int i = 0;
            while (x > dialogNodes[i].后触对话.Count - 1) {
                x = x - dialogNodes[i++].后触对话.Count;
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
    void SelectDialog(int num);
    string this[int x] { get; set; }
    int Count { get; }
    DialogGroup DialogGroupLize();
}

public static class DialogEx
{
    public static Dialog Combine(this Dialog dialog,params Dialog[] dialogs) {
        var temp = dialog.DialogGroupLize();
        for (int i = 0; i < dialogs.Length; i++) {
            temp = (temp + dialogs[i].DialogGroupLize()).DialogGroupLize();
        }
        return temp;
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
    Enable,
    PreEnable,
}

[Serializable]
public class Link_Dialog { }

[Serializable]
public class Link_DialogMachine { }

[Serializable]
public class Link_DialogMachineGroup { }

[Serializable]
public class Link_NodePatch { }
