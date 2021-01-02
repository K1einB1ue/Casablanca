using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("对话测试节点", menuName = "测试节点/对话测试节点", order = 0)]
public class DialogTestNode : NodeStatic
{
    [Output(ShowBackingValue.Never, connectionType = ConnectionType.Override,typeConstraint = TypeConstraint.Strict)] public Link_Dialog @对话测试;
    private bool trigger = false;
    public override void Update() {
        if (!trigger) {
            var Port = this.GetPort("对话测试");
            if (Port.ConnectionCount > 0) {             
                if (Port.Connection.node.GetType() == typeof(DialogNode)) {
                    trigger = true;
                    StaticPath.Story_DialogInfo.CurrentDialog = (DialogNode)Port.Connection.node;
                }
                else {
                    Debug.LogWarning("请连接正确的DialogNode");
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
