using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("�Ի����Խڵ�", menuName = "���Խڵ�/�Ի����Խڵ�", order = 0)]
public class DialogTestNode : NodeBase
{
    [Output(ShowBackingValue.Never, connectionType = ConnectionType.Multiple,typeConstraint = TypeConstraint.Strict)] public string @�Ի�����;
    private bool trigger = false;

    public override void Update() {
        if (!this.StoryBlock.Intest) {
            this.StoryBlock.Intest = true;
            StaticPath.Story_DialogInfo.CurrentDialog = null;
        }
        if (!trigger) {
            var Port = this.GetPort("�Ի�����");
            if (Port.ConnectionCount > 0) {
                foreach (var Con in Port.GetConnections()) {
                    if (((INode)Con.node).GetNodeType() == NodeType.DialogNode) {
                        trigger = true;
                        ((IDialog)Con.node).SetUpdateType(Dialog_UpdateType.Enable);
                        if (Con.node.GetType() == typeof(DialogNode)) {
                            if (StaticPath.Story_DialogInfo.CurrentDialog == null) {
                                StaticPath.Story_DialogInfo.CurrentDialog = (DialogNode)Con.node;
                            }
                            else {
                                StaticPath.Story_DialogInfo.CurrentDialog = StaticPath.Story_DialogInfo.CurrentDialog.Combine((DialogNode)Con.node);
                            }
                            ((DialogNode)Con.node).OnPreSelected();
                        }
                    }
                    else {
                        Debug.LogWarning("��������ȷ��DialogNode");
                    }
                }
            }
        }
    }
    public override NodeType GetNodeType() {
        return NodeType.TestNode;
    }
    public override void ReStruct() {
        this.trigger = false;
    }
}
