using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("�Ի����Խڵ�", menuName = "���Խڵ�/�Ի����Խڵ�", order = 0)]
public class DialogTestNode : NodeStatic
{
    [Output(ShowBackingValue.Never, connectionType = ConnectionType.Override,typeConstraint = TypeConstraint.Strict)] public Link_Dialog @�Ի�����;
    private bool trigger = false;
    public override void Update() {
        if (!trigger) {
            var Port = this.GetPort("�Ի�����");
            if (Port.ConnectionCount > 0) {             
                if (Port.Connection.node.GetType() == typeof(DialogNode)) {
                    trigger = true;
                    StaticPath.Story_DialogInfo.CurrentDialog = (DialogNode)Port.Connection.node;
                }
                else {
                    Debug.LogWarning("��������ȷ��DialogNode");
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
