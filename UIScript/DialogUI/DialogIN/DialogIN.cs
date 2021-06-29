using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIN : MonoBehaviour
{
    public GameObject Instance;
    public static bool Init = false;
    public List<DialogINNode> DialogIN_Nodes = new List<DialogINNode>();
    private void OnEnable() {
        NodesInit();
    }


    public void DialogUpdate() {
        int i = 0;
        if (CharacterManager.Main.Dialog != null) {          
            for (; i < DialogIN_Nodes.Count; i++) {
                this.DialogIN_Nodes[i].SetNode(i < CharacterManager.Main.Dialog.Count, i < CharacterManager.Main.Dialog.Count ? CharacterManager.Main.Dialog[i] : "");                
            }
        }
        else {
            for (; i < DialogIN_Nodes.Count; i++) {
                this.DialogIN_Nodes[i].SetNode(false, "");
            }
        }
    }



    public void NodesInit() {
        if (!Init) {
            for (int i = 0; i < 11; i++) {
                DialogIN_Nodes.Add(this.gameObject.transform.Find("对话选项" + i.ToString()).gameObject.GetComponent<DialogINNode>());
                DialogIN_Nodes[i].hash = i;
            }
            Init = true;
            EventManager.StoryChannelManager.OnMainCharacterDialogChange.AddListener(this.DialogUpdate);
        }
        
    }

    public void SelectOnlyOne(int hash) {

        CharacterManager.Main.Dialog.SelectDialog(hash);
        for (int i = 0; i < 11; i++) {
            DialogIN_Nodes[i].Selected(hash == i);
        }
    }

}
