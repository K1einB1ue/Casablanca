using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;
using XNode;

[CustomNodeGraphEditor(typeof(StoryBlock))]
public class StoryBlockEditor : NodeGraphEditor
{
    public override void OnGUI() {
        Rect WinRect = new Rect(20, 20, 200, 200);
        base.OnGUI();
        StoryBlock storyBlock = target as StoryBlock;
        if (GUILayout.Button("重构", GUILayout.Width(60))) {
            foreach (var node in target.nodes) {
                ((INode)node).ReStruct();
            }
            storyBlock.LoadEntry = false;
            storyBlock.Intest = false;
        }
        if (GUILayout.Button("块演算", GUILayout.Width(60))) {
            storyBlock.UpdateStoryNodes();
        }
        if (GUILayout.Button("步演算", GUILayout.Width(60))) {
            ((StaticINode)storyBlock).StaticUpdate();
        }

        if (GUILayout.Button("测试", GUILayout.Width(60))) {
            foreach (var node in target.nodes) {
                if (((INode)node).GetNodeType() == NodeType.TestNode) {
                    ((INode)node).Update();
                }
            }       
            storyBlock.Intest = true;
        }
        
        if (storyBlock.Intest) {
            if (StaticPath.StoryInfoDialogInfo.CurrentDialog != null) {
                for (int i = 0; i < StaticPath.StoryInfoDialogInfo.CurrentDialog.Content.Count; i++) {
                    if (GUILayout.Button(StaticPath.StoryInfoDialogInfo.CurrentDialog.Content[i], GUILayout.Width(600))) {
                        StaticPath.StoryInfoDialogInfo.CurrentDialog = StaticPath.StoryInfoDialogInfo.CurrentDialog.GetDiaglog(i);
                        if (StaticPath.StoryInfoDialogInfo.CurrentDialog == null) {
                            break;
                        }
                    }
                }
            }
        }
        
    }

}


