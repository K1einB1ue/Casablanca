using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DialogMachineGroupPack : ScriptableObject 
{
    private List<DialogMachine> DialogMachines = new List<DialogMachine>();
    private static StringBuilder stringBuilder = new StringBuilder();
    public DialogMachineGroup Instance {
        get {
            if (instance == null) {
                instance = new DialogMachineGroup(this);
                instance.PackUp(DialogMachines.ToArray());
            }
            return instance;
        }
    }
    [NonSerialized]
    private DialogMachineGroup instance = null;

    public int Count => DialogMachines.Count;

    public void Clear() {
        this.DialogMachines.Clear();
    }
    public void StoreAdd(DialogMachine dialogMachine) {
        if (!DialogMachines.Contains(dialogMachine)) {
            DialogMachines.Add(dialogMachine);
        }
    }

    public override string ToString() {
        stringBuilder.Clear();
        stringBuilder.Append("(");
        bool flag = false;

        foreach (var dialog in DialogMachines) {
            if (flag) {
                stringBuilder.Append(",");
            }
            flag = true;
            stringBuilder.Append(dialog.name);
        }

        stringBuilder.Append(")");
        return stringBuilder.ToString();
    }

    public void Rename() {
        this.name = this.ToString();
    }
}

