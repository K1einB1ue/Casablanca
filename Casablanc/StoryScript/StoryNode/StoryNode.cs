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
        return;
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
                ((IStory)this).SetUpdateType(Story_UpdateType.Enable);
                foreach (var POUT in Outputs) {
                    if (POUT.fieldName == "后触事件") {
                        foreach (var Pout in POUT.GetConnections()) {
                            ((IStory)Pout.node).SetUpdateType(Story_UpdateType.PreEnable);
                            ((IStory)Pout.node).PreloadUpdate();
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
    NodePipelineInstance NodePipelineFactory1 { get; }
    void Update();
    NodeType GetNodeType();
    void ReStruct();
}
public interface IStory {
    void Update(out bool change);
    void PreloadUpdate();
    Story_UpdateType GetUpdateType();
    void SetUpdateType(Story_UpdateType updateType);

}
[Serializable]
public abstract class StoryNodeBase : NodeBase, IStory {
    [SerializeField]
    private Story_UpdateType updateType = Story_UpdateType.Unable;
    public virtual void Update(out bool change) { change = false; }
    public override void Update() { }
    public virtual void PreloadUpdate() { }
    public override NodeType GetNodeType() { return NodeType.StoryNode; }
    public Story_UpdateType GetUpdateType() { return this.updateType; }
    void IStory.SetUpdateType(Story_UpdateType updateType) { this.updateType = updateType; }
    public override void ReStruct() {
        this.updateType = Story_UpdateType.Unable;
    }
}
public abstract class ComponentNodeBase : NodeBase
{
    public override NodeType GetNodeType() {
        return NodeType.Component;
    }
}
public abstract class NodeBase : Node, INode
{
    public virtual NodePipelineInstance NodePipelineFactory1 { get { Debug.LogError("错误地调用了工厂"); return nodePipelineFactory1; } }
    public static NodePipelineInstance nodePipelineFactory1;
    public virtual NodePipelineInstance NodePipelineFactory2 { get { Debug.LogError("错误地调用了工厂"); return nodePipelineFactory2; } }
    public static NodePipelineInstance nodePipelineFactory2;
    public virtual NodePipelineInstance NodePipelineFactory3 { get { Debug.LogError("错误地调用了工厂"); return nodePipelineFactory3; } }
    public static NodePipelineInstance nodePipelineFactory3;
    [HideInInspector]
    public StoryBlock StoryBlock => (StoryBlock)this.graph;
    public virtual void Update() { }
    public virtual NodeType GetNodeType() { return NodeType.Node; }
    public virtual void ReStruct() { }
}

public static class NodeStatic
{

    public const string CommonPortName = "节点统一化";
    public static IEnumerable<NodeBase> Node_On_Port(this NodeBase This, string PortParam = CommonPortName) {
        NodePort port = This.GetInputPort(PortParam);
        if (port != null) {
            if (port.ConnectionCount > 0) {
                foreach (var con in port.GetConnections()) {
                    if (con.node is NodeBase) {
                        yield return ((NodeBase)con.node);
                    }
                    else {
                        Debug.LogError("错误的节点");
                    }
                }
            }
        }
        else {
            Debug.LogError("错误的接口");
        }
    }

}

[Serializable]
public class Link_Story { }

public enum NodeType {
    Node,
    StoryNode,
    TriggerNode,
    DialogNode,
    TestNode,
    Store,
    Component,
}