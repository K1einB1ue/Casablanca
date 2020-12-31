using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XNode;

[CreateNodeMenu("剧情节点", menuName = "剧情节点/剧情节点")]
public class StoryNode : StoryNodeBase
{
    [Input(ShowBackingValue.Never,dynamicPortList =true,connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]   public Link_Story @前置事件;
    [Output]                                                public Link_Story @后触事件;
    [Input(ShowBackingValue.Never,typeConstraint = TypeConstraint.Strict)]                         public bool        @触发完成;
    [Input(ShowBackingValue.Never,typeConstraint = TypeConstraint.Strict)]                         public bool        @触发启用;

    [HideInInspector]
    public int Hash = -1;
    //[TextArea] public string 大致描述;


    public override void ReStruct() {
        ((IStory)this).SetUpdateType(Story_UpdateType.Unable);
    }

    public override object GetValue(NodePort port) {
        bool flag = true;
        if (((IStory)this).GetUpdateType() == Story_UpdateType.Enable) {
            foreach (var PIN in Inputs) {
                if (PIN.fieldName.Contains("前置事件") && PIN.fieldName != "前置任务") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
                else if (PIN.fieldName == "触发完成") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
                else if (PIN.fieldName == "触发启用") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
                else {
                    throw new Exception("错误的触发管线");
                }
            }
            if (flag) {
                ((IStory)this).SetUpdateType(Story_UpdateType.Disable);
            }
        }
        else if (((IStory)this).GetUpdateType() == Story_UpdateType.Disable) {
            return flag;
        }
        else if (((IStory)this).GetUpdateType() == Story_UpdateType.Unable) {
            return false;
        }
        else if (((IStory)this).GetUpdateType() == Story_UpdateType.PreEnable) {
            return false;
        }
        else {
            return false;
        }
        return false;
    }

    public override void Update() {
        bool flag = true;
        if (((IStory)this).GetUpdateType() == Story_UpdateType.PreEnable) {
            foreach (var PIN in Inputs) {
                if (PIN.fieldName.Contains("前置事件") && PIN.fieldName != "前置任务") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
                else if (PIN.fieldName == "触发启用") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
            }
            if (flag) {
#if !UNITY_EDITOR
                this.StoryBlock.LoadoffStoryNode(this);
#endif
                ((IStory)this).SetUpdateType(Story_UpdateType.Enable);
                foreach (var POUT in Outputs) {
                    if (POUT.fieldName == "后触事件") {
                        foreach (var Pout in POUT.GetConnections()) {
#if !UNITY_EDITOR
                            this.StoryBlock.LoadStoryNode((StoryNode)Pout.node);
#endif
                            ((IStory)Pout.node).SetUpdateType(Story_UpdateType.PreEnable);                           
                        }
                    }
                }
            }
        }
    }


    public override void Update(out bool change) {
        bool flag = true;
        if (((IStory)this).GetUpdateType() == Story_UpdateType.PreEnable) {
            foreach (var PIN in Inputs) {
                if (PIN.fieldName.Contains("前置事件") && PIN.fieldName != "前置任务") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
                else if (PIN.fieldName == "触发启用") {
                    if (PIN.ConnectionCount > 0) {
                        flag = PIN.GetInputValue<bool>() && flag;
                    }
                }
            }
            if (flag) {
                change = true;
#if !UNITY_EDITOR
                this.StoryBlock.LoadoffStoryNode(this);
#endif
                ((IStory)this).SetUpdateType(Story_UpdateType.Enable);
                foreach (var POUT in Outputs) {
                    if (POUT.fieldName == "后触事件") {
                        foreach (var Pout in POUT.GetConnections()) {
#if !UNITY_EDITOR
                            this.StoryBlock.LoadStoryNode((StoryNode)Pout.node);
#endif
                            ((IStory)Pout.node).SetUpdateType(Story_UpdateType.PreEnable);
                        }
                    }
                }
                return;
            }
        }
        change = false;
    }
}



public enum Story_UpdateType {
    Unable,
    PreEnable,
    Enable,
    Disable,
}
public interface INode {
    void NodeInit();
    void Update();
    NodeType GetNodeType();
    void ReStruct();
}
public interface IStory {
    void Update(out bool change);
    Story_UpdateType GetUpdateType();
    void SetUpdateType(Story_UpdateType updateType);

}
[Serializable]
public abstract class StoryNodeBase : NodeStatic, IStory {
    [SerializeField]
    private Story_UpdateType updateType = Story_UpdateType.Unable;
    public virtual void Update(out bool change) { change = false; }
    public override NodeType GetNodeType() { return NodeType.StoryNode; }
    public Story_UpdateType GetUpdateType() { return this.updateType; }
    void IStory.SetUpdateType(Story_UpdateType updateType) { this.updateType = updateType; }
}
public abstract class NodeStatic : Node, INode
{
    [HideInInspector]
    public StoryBlock StoryBlock => (StoryBlock)this.graph;
    public virtual void NodeInit() { }
    public virtual void Update() { }
    public virtual NodeType GetNodeType() { return NodeType.Node; }
    public virtual void ReStruct() { }
}
[Serializable]
public class Link_Story { }

public enum NodeType {
    Node,
    StoryNode,
    TriggerNode,
    DialogNode,
    TestNode,
    StoryBlockNode,
    Store,
}