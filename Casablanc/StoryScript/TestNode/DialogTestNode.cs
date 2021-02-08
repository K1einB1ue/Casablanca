using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("对话测试节点", menuName = "测试节点/对话测试节点", order = 0)]
public class DialogTestNode : NodeStatic
{
    [Output(ShowBackingValue.Never, connectionType = ConnectionType.Multiple,typeConstraint = TypeConstraint.Strict)] public Link_Dialog @对话测试;
    private bool trigger = false;

    public override void Update() {
            if (!this.StoryBlock.Intest) {
                this.StoryBlock.Intest = true;
                StaticPath.Story_DialogInfo.CurrentDialog = null;
            }
            if (!trigger) {
                var Port = this.GetPort("对话测试");
                if (Port.ConnectionCount > 0) {
                    foreach (var Con in Port.GetConnections()) {
                        if (Con.node.GetType() == typeof(DialogNode)) {
                            trigger = true;
                            if (StaticPath.Story_DialogInfo.CurrentDialog == null) {
                                StaticPath.Story_DialogInfo.CurrentDialog = (DialogNode)Con.node;
                            }
                            else {
                                StaticPath.Story_DialogInfo.CurrentDialog = StaticPath.Story_DialogInfo.CurrentDialog.DialogGroupLize() + ((DialogNode)Con.node).DialogGroupLize();
                            }
                        }
                        else {
                            Debug.LogWarning("请连接正确的DialogNode");
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
