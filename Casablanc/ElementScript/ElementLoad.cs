using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementLoad : ScriptableObject
{
    [SerializeField]
    public List<ElementStore> elementlist;

    

    [HideInInspector]
    //public Dictionary<ElementType, ElementStore> ElementStatics;
    public Dictionary<string, ElementStore> ElementStatics;

    private void OnEnable() {
        //ElementStatics = new Dictionary<ElementType, ElementStore>();
        //foreach (var element in elementlist) {
        //    if (element != null) {
        //        if (ElementStatics.TryGetValue(element.ElementType, out var itemStore)) {
        //            Debug.LogError("��Ʒ��̬�������ظ���Ʒ!����!");
        //        }
        //        else {
        //            ElementStatics[element.ElementType] = element;
        //        }
        //    }
        //}


        ElementStatics = new Dictionary<string, ElementStore>();
        foreach (var element in elementlist) {
            if (element != null) {
                if (ElementStatics.TryGetValue(element.name, out var itemStore)) {
                    Debug.LogError("��Ʒ��̬�������ظ���Ʒ!����!");
                }
                else {
                    ElementStatics[element.name] = element;
                }
            }
        }

    }

    //public ElementStore this[ElementType Type]
    //{
    //    get {
    //        if (ElementStatics.TryGetValue(Type, out var elementStore)) {
    //            return elementStore;
    //        }
    //        else {
    //            Debug.LogError("�������Ʒ����");
    //            return null;
    //        }
    //    }
    //}


    public ElementStore this[string type]
    {
        get {
            if (ElementStatics.TryGetValue(type, out var elementStore)) {
                return elementStore;
            }
            else {
                Debug.LogError("�������Ʒ����");
                return null;
            }
        }
    }
}
