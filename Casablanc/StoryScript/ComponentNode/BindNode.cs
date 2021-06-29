using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(150)]
public class BindNode : BindNodeBase
{

    public override bool IsGroup(NodeBase Target) {
        var OutPort = this.GetPort(nameof(�󶨿�));
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
        var OutPort = this.GetPort(nameof(�󶨿�));
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
            NodePort port = This.GetInputPort("�ڵ�ͳһ��");
            if (port != null) {
                if (port.ConnectionCount > 0) {
                    foreach (var con in port.GetConnections()) {
                        ((BindNodeBase)con.node).GroupAction(action);
                    }
                }
            }
            else {
                Debug.LogError("����Ľӿ�");
            }
        }
        else {
            Debug.LogError("����Ľڵ�");
        }
    }
    public static void GroupAction(this NodeBase This, Action<NodeBase> action) {
        NodeActioned.Clear();
        NodePort port = This.GetInputPort("�ڵ�ͳһ��");
        if (port != null) {
            if (port.ConnectionCount > 0) {
                foreach (var con in port.GetConnections()) {
                    ((BindNodeBase)con.node).GroupAction(action);
                }
            }
        }
        else {
            Debug.LogError("����Ľӿ�");
        }
    }
    public static bool IsGroup(this NodeBase This, NodeBase Target) {
        NodePort port = This.GetInputPort("�ڵ�ͳһ��");
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
            Debug.LogError("����Ľӿ�");
        }
        return false;
    }
}
