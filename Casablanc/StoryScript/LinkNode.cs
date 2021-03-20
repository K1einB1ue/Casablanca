using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[CreateNodeMenu("链接节点",menuName ="链接节点/链接节点")]
public class LinkNode : StoryNodeBase 
{
	[Input] public Link_Story @前置事件;
	[Output] public Link_Dialog @后置对话;

	public override void PreloadUpdate() {
		if (((IStory)this).GetUpdateType() == Story_UpdateType.PreEnable) { 
			((IStory)this).SetUpdateType(Story_UpdateType.Disable);
			foreach (var PIN in Outputs) {
				if(PIN.Connection.ConnectionCount > 0) {
					foreach(var Port in PIN.GetConnections()) {
						if(Port.node is DialogNode) {
							((DialogNode)Port.node).OnPreSelected();
                        }
                    }
                }
			}
		}
	}
	public override void Update() {
		base.Update();
	}
	public override void Update(out bool change) {
		base.Update(out change);
	}

	public override object GetValue(NodePort port) {
		return port.GetInputValue<bool>();
	}
}