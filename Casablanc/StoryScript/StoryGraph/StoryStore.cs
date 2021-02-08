using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "新剧情存储", menuName = "剧情/新剧情存储")]
public class StoryStore : ScriptableObject
{
    public List<StoryBlock> storyBlocks;
}
