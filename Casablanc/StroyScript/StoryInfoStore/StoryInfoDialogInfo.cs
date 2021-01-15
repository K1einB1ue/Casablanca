using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "剧情节点界面存储", menuName = "剧情/剧情节点界面存储")]
public class StoryInfoDialogInfo : ScriptableObject
{
    private List<DialogNode> Enabled = new List<DialogNode>();
    private List<int> DialogHash = new List<int>();




    public DialogNode CurrentDialog;
    
}
