using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "����ڵ����洢", menuName = "����/����ڵ����洢")]
public class StoryInfoDialogInfo : ScriptableObject
{
    private List<DialogNode> Enabled = new List<DialogNode>();




    public Dialog CurrentDialog;
    
}
