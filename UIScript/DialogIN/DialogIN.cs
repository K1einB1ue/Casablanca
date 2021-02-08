using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIN : MonoBehaviour
{
    public static DialogIN Instance;
    public List<GameObject> DialogNodes = new List<GameObject>();
    private void OnEnable() {
        Instance ??= this;
        NodesInit();
    }
    public void NodesInit() {
        for(int i = 0; i < 11; i++) {
            DialogNodes.Add(this.gameObject.transform.Find("对话选项" + i.ToString()).gameObject);
            DialogNodes[i].GetComponent<DialogIN_Node>().hash = i;
        }
        foreach(var node in DialogNodes) {
            node.GetComponent<DialogIN_Node>().Text = "";
        }
    }

    public void SelectOnlyOne(int hash) {
        for(int i = 0; i < 11; i++) {
            DialogNodes[i].GetComponent<DialogIN_Node>().Selected(hash == i);
        }
    }

    public void Disable(int hash) {
        DialogNodes[hash].GetComponent<DialogIN_Node>().Active = false;
    }
    public void Enable(int hash) {
        DialogNodes[hash].GetComponent<DialogIN_Node>().Active = true;
    }


}
