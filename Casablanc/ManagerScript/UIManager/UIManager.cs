using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager :SingletonMono<UIManager>
{
    public static LinkedList<UI> UIEnable = new LinkedList<UI>();
    public static List<UI> StaticUI = new List<UI>();
    public static List<UI> ALLUI = new List<UI>();
    private static LinkedListNode<UI> start { get { if (UIEnable.First == null) return null;return UIEnable.First; } }
    bool[] Enable;


    private void Update() {
        UIS.INIT();
        LinkListUpdate();
        StaticListUpdate();
    } 

    public static void AwakeALL() {
        for (int i = 0; i < StaticUI.Count; i++) {
            StaticUI[i].Awake();
        }
        if (start != null) {
            LinkedListNode<UI> Pointer = start;
            start.Value.update();
            while (Pointer.Next != null) {
                Pointer = Pointer.Next;
                if (Pointer.Value.disable(Pointer, out LinkedListNode<UI> NEXT)) {
                    if (NEXT == null) {
                        break;
                    }
                    Pointer = NEXT;
                }
                Pointer.Value.Awake();
            }
        }
    }
    private void StaticListUpdate() {
        for (int i = 0; i < StaticUI.Count; i++) {           
            StaticUI[i].update();
            }
    }
    private void LinkListUpdate() {
        if (start != null) {
            LinkedListNode<UI> Pointer = start;
            start.Value.update();
            while (Pointer.Next != null) {
                Pointer = Pointer.Next;
                if(Pointer.Value.disable(Pointer, out LinkedListNode<UI> NEXT)) {
                    if (NEXT == null) {
                        break;
                    }
                    Pointer = NEXT;
                }
                Pointer.Value.update();
            }
        }
    }
    public void ItemCall(Item item) {
        
    }
    public static void GetStaticUIAttach(int ID,UI uI) {
        bool attachflag = false;
        foreach(UI uis in StaticUI) {
            if (uis.ID == ID) {
                uis.GetSonNode().Add(uI);
                attachflag = true;
                break;
            }
        }
        if (!attachflag) {
            Debug.LogError("不存在如此ID的静态UI");
        }
    }
    private IEnumerator WaitGetStaticUIAttach(int ID, UI uI) {
        yield return new WaitForFixedUpdate();
    }
}






