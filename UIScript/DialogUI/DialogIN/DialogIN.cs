using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIN : MonoBehaviour
{
    public GameObject Instance;
    public List<DialogINNode> DialogIN_Nodes = new List<DialogINNode>();
    private void OnEnable() {
        NodesInit();
    }

    public void Update() {
        this.DialogUpdate();
    }
    public void DialogUpdate() {
        if (CharacterManager.Main.Dialog != null) {
            int i = 0;
            for (; i < CharacterManager.Main.Dialog.Count; i++) {
                this.DialogIN_Nodes[i].Active = true;
                this.DialogIN_Nodes[i].Text = CharacterManager.Main.Dialog[i];

            }
            for (; i < DialogIN_Nodes.Count; i++) {
                this.DialogIN_Nodes[i].Active = false;
                this.DialogIN_Nodes[i].Text = "";
            }
        }
        else {
            for (int i = 0; i < DialogIN_Nodes.Count; i++) {
                this.DialogIN_Nodes[i].Active = false;
                this.DialogIN_Nodes[i].Text = "";
            }
        }
    }



    public void NodesInit() {
        for(int i = 0; i < 11; i++) {
            DialogIN_Nodes.Add(this.gameObject.transform.Find("对话选项" + i.ToString()).gameObject.GetComponent<DialogINNode>());
            DialogIN_Nodes[i].hash = i;
            
        }
        foreach(var node in DialogIN_Nodes) {
            node.Text = "";
            node.Active = false;
        }
    }

    public void SelectOnlyOne(int hash) {
        CharacterManager.Main.Dialog.SelectDialog(hash);
        for (int i = 0; i < 11; i++) {
            DialogIN_Nodes[i].Selected(hash == i);
        }
    }

    public void Disable(int hash) {
        DialogIN_Nodes[hash].Active = false;
    }
    public void Enable(int hash) {
        DialogIN_Nodes[hash].Active = true;
    }


}
