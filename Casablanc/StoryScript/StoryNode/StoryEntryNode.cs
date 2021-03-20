using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("剧情入口节点", menuName = "剧情节点/剧情入口节点")]
public class StoryEntryNode : StoryNodeBase
{
    [Output(typeConstraint = TypeConstraint.Strict)] public Link_Story @剧情块入口;
    protected override void Init() {
        this.StoryBlock.StoryEntryNodes.Add(this);
    }

    public override object GetValue(NodePort port) {
        return true;
    }
    public override void ReStruct() {
        base.ReStruct();
    }
    public override void Update() {
        if (this.GetUpdateType() == Story_UpdateType.Unable) {
            foreach (var port in Ports) {
                if (port.fieldName == "剧情块入口") {
                    foreach (var pout in port.GetConnections()) {
                        if (((IStory)pout.node).GetUpdateType() == Story_UpdateType.Unable) {
                            ((IStory)pout.node).SetUpdateType(Story_UpdateType.PreEnable);
                            ((IStory)pout.node).PreloadUpdate();
                        }
                    }
                }
            }
            ((IStory)this).SetUpdateType(Story_UpdateType.Disable);
        }
    }
}