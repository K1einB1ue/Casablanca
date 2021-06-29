using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(150)]
public class BindNode : BindNodeBase
{

    public override bool IsGroup(NodeBase Target) {
        var OutPort = this.GetPort(nameof(绑定块));
        if (OutPort.GetConnections().Count > 0) {
            foreach (var con in OutPort.GetConnections()) {
                if (object.ReferenceEquals(Target, con.node)) {
                    return true;
                }
            }
        }
        return false;
    }

    public override void GroupAction(Action<NodeBase> action) {     
        var OutPort = this.GetPort(nameof(绑定块));
        if (OutPort.GetConnections().Count > 0) {
            foreach (var con in OutPort.GetConnections()) {
                if (con.node is NodeBase) {
                    if (!BindNodeStatic.NodeActioned.Contains(((NodeBase)con.node)) && !(BindNodeStatic.NodeNoNeedAction.Contains(((NodeBase)con.node)))) {
                        action.Invoke(((NodeBase)con.node));
                        BindNodeStatic.NodeActioned.Add((NodeBase)con.node);
                    }
                }
            }
        }
    }

    
}

public static class BindNodeStatic
{
    public static HashSet<NodeBase> NodeActioned = new HashSet<NodeBase>();
    public static HashSet<NodeBase> NodeNoNeedAction = new HashSet<NodeBase>();
    public static void GroupAction(this NodeBase This, Action<NodeBase> action, Action<HashSet<NodeBase>> NoNeedActionSet) {
        NodeActioned.Clear();
        NodeNoNeedAction.Clear();
        NoNeedActionSet.Invoke(NodeNoNeedAction);
        if (This is NodeBase) {
            NodePort port = This.GetInputPort("节点统一化");
            if (port != null) {
                if (port.ConnectionCount > 0) {
                    foreach (var con in port.GetConnections()) {
                        ((BindNodeBase)con.node).GroupAction(action);
                    }
                }
            }
            else {
                Debug.LogError("错误的接口");
            }
        }
        else {
            Debug.LogError("错误的节点");
        }
    }
    public static void GroupAction(this NodeBase This, Action<NodeBase> action) {
        NodeActioned.Clear();
        NodePort port = This.GetInputPort("节点统一化");
        if (port != null) {
            if (port.ConnectionCount > 0) {
                foreach (var con in port.GetConnections()) {
                    ((BindNodeBase)con.node).GroupAction(action);
                }
            }
        }
        else {
            Debug.LogError("错误的接口");
        }
    }
    public static bool IsGroup(this NodeBase This, NodeBase Target) {
        NodePort port = This.GetInputPort("节点统一化");
        if (port != null) {
            if (port.ConnectionCount > 0) {
                foreach (var con in port.GetConnections()) {
                    if (((BindNodeBase)con.node).IsGroup(Target)) {
                        return true;
                    }
                }
            }
        }
        else {
            Debug.LogError("错误的接口");
        }
        return false;
    }
}
