using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "故事节点存储",menuName ="剧情/故事节点存储")]
public class StoryInfoStore : ScriptableObject
{
    public List<StoryNode> storyNodesEnable = new List<StoryNode>();

    public StoryBlock StoryGraph = new StoryBlock();
}
