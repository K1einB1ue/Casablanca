using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "对话机", menuName = "剧情/对话机")]
public class DialogMachine : ScriptableObject
{
    public Dialog Dialog {
        get {
            if (dialog == null) {
                if (currentNode != null) {
                    return currentNode;
                }
                else if (currentGroup != null) {
                    return currentGroup;
                }
            }
            return dialog;
        }
        set {
            this.dialog = value;
            if(value is DialogNode) {
                this.currentNode = (DialogNode)value;
                this.currentGroup = null;
            }
            else if(value is DialogGroup) {
                this.currentGroup = (DialogGroup)value;
                this.currentNode = null;
            }
        }
    }
    private Dialog dialog;

    [Header("只是为了Debug")]
    public DialogNode currentNode;
    public DialogGroup currentGroup;

    public void Clear() {
        dialog = null;
        currentNode = null;
        currentGroup = null;
    }


}


[Serializable]
public class DialogMachineGroup
{
    [SerializeField]
    private List<DialogMachine> GroupStore = new List<DialogMachine>();
    private HashSet<DialogMachine> Group = new HashSet<DialogMachine>();
    private static StringBuilder stringBuilder = new StringBuilder();

    public bool PackUp(params DialogMachine[] dialogMachines) {
        bool flag = true;
        for(int i = 0; i < dialogMachines.Length; i++) {
            flag = flag && Group.Add(dialogMachines[i]);
        }
        return flag;
    }
    public bool PackUp(IEnumerable<DialogMachine> dialogMachines) {
        bool flag = false;
        foreach (var dialogMachine in dialogMachines) {
            flag = flag || Group.Add(dialogMachine);
        }
        return flag;
    }
    public bool Remove(params DialogMachine[] dialogMachines) {
        bool flag = false;
        for(int i=0;i< dialogMachines.Length; i++) {
            flag = flag || Group.Remove(dialogMachines[i]);
        }
        return flag;
    }
    public bool Remove(IEnumerable<DialogMachine> dialogMachines) {
        bool flag = true;
        foreach (var dialogMachine in dialogMachines) {
            flag = flag && Group.Remove(dialogMachine);
        }
        return flag;
    }

    public bool Match(DialogMachineGroup Key) {
        bool flag = true;
        foreach(var single in this.Group) {
            flag = flag && Key.Group.Contains(single);
        }
        return flag;
    }

    public override string ToString() {
        stringBuilder.Clear();
        stringBuilder.Append("(");
        bool flag = false;      

        foreach(var dialog in Group) {
            if (flag) {
                stringBuilder.Append(",");
            }
            flag = true;
            stringBuilder.Append(dialog.name);
        }

        stringBuilder.Append(")");
        return stringBuilder.ToString();
    }
    public void Init() {
        foreach (var single in GroupStore) {
            this.Group.Add(single);
        }
    }
    public void Save() {
        this.GroupStore.Clear();
        foreach (var single in Group) {
            this.GroupStore.Add(single);
        }
    }

    public Dialog Dialog {
        get {
            Dialog Sum = null;
            foreach(var dialogmachine in Group) {
                if (dialogmachine.Dialog != null) {
                    if (Sum == null) {
                        Sum = dialogmachine.Dialog;
                    }
                    else {
                        Sum = Sum.Combine(dialogmachine.Dialog);
                    }
                }
            }
            return Sum;
        }
    }
}