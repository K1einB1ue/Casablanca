﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[CreateNodeMenu("剧情出口节点", menuName = "剧情节点/剧情出口节点")]
public class StoryExitNode : StoryNodeBase
{
    [Input(ShowBackingValue.Never,typeConstraint = TypeConstraint.Strict)] public Link_Story @剧情块出口;
    public List<StoryBlock> @触发剧情块 = new List<StoryBlock>();
    protected override void Init() {
        this.StoryBlock.storyExitNodes.Add(this);
    }

    public override object GetValue(NodePort port) {
        bool flag = true;
        foreach (var PIN in Inputs) {
            if (PIN.fieldName == "剧情块出口") {
                if (PIN.ConnectionCount > 0) {
                    foreach (var mark in PIN.GetInputValues<bool>()) {
                        flag = mark && flag;
                    }
                    if (flag) {
                        ((IStory)PIN.Connection.node).SetUpdateType(Story_UpdateType.Disable);
                        return flag;
                    }
                }

            }
        }
        return false;
    }

    public override void Update() {
        bool flag = true;
        foreach (var PIN in Inputs) {
            if (PIN.fieldName == "剧情块出口") {
                if (PIN.ConnectionCount > 0) {
                    foreach (var mark in PIN.GetInputValues<bool>()) {
                        flag = mark && flag;
                    }
                    if (flag) {
                        ((IStory)PIN.Connection.node).SetUpdateType(Story_UpdateType.Disable);
                        return;
                    }
                }

            }
        }
    }

    public override void Update(out bool change) {
        bool flag = true;
        change = false;
        foreach (var PIN in Inputs) {
            if (PIN.fieldName == "剧情块出口") {
                if (PIN.ConnectionCount > 0) {
                    foreach (var mark in PIN.GetInputValues<bool>()) {
                        flag = mark && flag;
                    }
                    if (flag) {
                        ((IStory)PIN.Connection.node).SetUpdateType(Story_UpdateType.Disable);
                        return;
                    }
                }
            }
        }
        
    }
}
    

