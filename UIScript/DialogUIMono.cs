using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUIMono : MonoBehaviour
{
    Queue<DialogNode> DialogNodes = new Queue<DialogNode>();
    GameObject DialogOutput;
    GameObject DialogInput;

    [SerializeField]
    private GameObject DialogIn;
    [SerializeField]
    private GameObject DialogOut;

    private void OnEnable() {
        this.DialogOutput = FindEx.Find(this.transform, "对话输出").gameObject;
        this.DialogInput = FindEx.Find(this.transform, "对话输入").gameObject;
    }

    private void EnqueueDialogNodes(DialogNode dialogNode) {





        this.DialogNodes.Enqueue(dialogNode);
    }
    
    
    private DialogNode DequeneDialogNodes() {
        DialogNode Out = this.DialogNodes.Dequeue();


        return Out;
    }


}
